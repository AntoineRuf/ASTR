using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

/// <summary>
/// CellGrid class keeps track of the game, stores cells, units and players objects. It starts the game and makes turn transitions. 
/// It reacts to user interacting with units or cells, and raises events related to game progress. 
/// </summary>
public class CellGrid : MonoBehaviour
{
    public event EventHandler GameStarted;
    public event EventHandler GameEnded;
    public event EventHandler TurnEnded;
    
    private CellGridState _cellGridState;//The grid delegates some of its behaviours to cellGridState object.
    public CellGridState CellGridState
    {
        get
        {
            return _cellGridState;
        }
        set
        {
            if(_cellGridState != null)
                _cellGridState.OnStateExit();
            _cellGridState = value;
            _cellGridState.OnStateEnter();
        }
    }

    public int NumberOfPlayers { get; private set; }

    public Player CurrentPlayer
    {
        get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
    }
    // Each Player only has 1 Unit
    public int CurrentPlayerNumber { get; private set; }
    // Each Team have 1 or more Units
    public int CurrentTeamNumber { get; private set; }

    public Transform PlayersParent;

    public List<Player> Players { get; private set; }
    public List<Cell> Cells { get; private set; }
    public List<Unit> Units { get; private set; }
    public List<Unit> UnitList; // sorted list by decreasing Initiative
    public int Turn;

    void Start()
    {
        Turn = 0;
        Directions.Initialize(); // initialize static class Directions
        Players = new List<Player>();
        for (int i = 0; i < PlayersParent.childCount; i++)
        {
            var player = PlayersParent.GetChild(i).GetComponent<Player>();
            if (player != null)
                Players.Add(player);
            else
                Debug.LogError("Invalid object in Players Parent game object");
        }
        NumberOfPlayers = Players.Count;
        CurrentPlayerNumber = Players.Min(p => p.PlayerNumber);

        Cells = new List<Cell>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
            if (cell != null)
                Cells.Add(cell);
            else
                Debug.LogError("Invalid object in cells parent game object");
        }
      
        foreach (var cell in Cells)
        {
            cell.CellClicked += OnCellClicked;
            cell.CellHighlighted += OnCellHighlighted;
            cell.CellDehighlighted += OnCellDehighlighted;
        }
             
        var unitGenerator = GetComponent<IUnitGenerator>();
        if (unitGenerator != null)
        {
            Units = unitGenerator.SpawnUnits(Cells);
            foreach (var unit in Units)
            {
                unit.UnitClicked += OnUnitClicked;
                unit.UnitDestroyed += OnUnitDestroyed;
                unit.UnitForTargetSelected += OnUnitForTargetSelected;
                unit.UnitForTargetDeselected += OnUnitForTargetDeselected;
                unit.Cell.Occupent = unit;
            }
        }
        else
            Debug.LogError("No IUnitGenerator script attached to cell grid");

        UnitList = UnitListSort(Units);
        CurrentPlayerNumber = UnitList[Turn].PlayerNumber;
        StartGame();
    }

    private void OnCellDehighlighted(object sender, EventArgs e)
    {
        CellGridState.OnCellDeselected(sender as Cell);
    }
    private void OnCellHighlighted(object sender, EventArgs e)
    {
        CellGridState.OnCellSelected(sender as Cell);
    } 
    private void OnCellClicked(object sender, EventArgs e)
    {
        CellGridState.OnCellClicked(sender as Cell);
    }

    private void OnUnitClicked(object sender, EventArgs e)
    {
        CellGridState.OnUnitClicked(sender as Unit);
    }
    private void OnUnitDestroyed(object sender, AttackEventArgs e)
    {
        Units.Remove(sender as Unit);
        var totalPlayersAlive = Units.Select(u => u.TeamNumber).Distinct().ToList(); //Checking if the game is over
        if (totalPlayersAlive.Count == 1)
        {
            if(GameEnded != null)
                GameEnded.Invoke(this, new EventArgs());
        }
    }

    private void OnUnitForTargetSelected(object sender, EventArgs e)
    {
        CellGridState.OnUnitForTargetSelected(sender as Unit);
    }

    private void OnUnitForTargetDeselected(object sender, EventArgs e)
    {
        CellGridState.OnUnitForTargetDeselected(sender as Unit);
    }

    /// <summary>
    /// Method is called once, at the beggining of the game.
    /// </summary>
    public void StartGame()
    {
        if(GameStarted != null)
            GameStarted.Invoke(this, new EventArgs());

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);
    }
    /// <summary>
    /// Method makes turn transitions. It is called by player at the end of his turn.
    /// </summary>
    public void EndTurn()
    {
        CellGridState = new CellGridStateTurnChanging(this);
        StartCoroutine(FacingChoice(UnitList[Turn]));
    }

    public void RealEndTurn()
    {
        for (int i = 0; i < 6; ++i)
        {
            UnitList[Turn].transform.GetChild(2).GetChild(i).GetComponent<OnClickDirectionChoice>().clicked = false;
            UnitList[Turn].transform.GetChild(2).GetChild(i).GetComponent<OnClickDirectionChoice>().hovering = false;
            UnitList[Turn].transform.GetChild(2).GetChild(i).GetComponent<SpriteRenderer>().color =
                UnitList[Turn].transform.GetChild(2).GetChild(i).GetComponent<OnClickDirectionChoice>().StartColor;
        }
        
        UnitListRefresh(UnitList);
        Turn = (Turn + 1) % Units.Count();

        if (Units.Select(u => u.PlayerNumber).Distinct().Count() == 1)
        {
            return;
        }

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnEnd(); });

        // -----------------   ASTR
        if (Turn == 0) UnitList = UnitListSort(Units); // Each Round, re-sort the UnitList
        CurrentPlayerNumber = UnitList[Turn].PlayerNumber; // Get the next playing unit and retrieve its player number

        //
        while (Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).Count == 0)
        {
            CurrentPlayerNumber = (CurrentPlayerNumber + 1) % Units.Count();
        }//Skipping players that are defeated.

        if (TurnEnded != null)
            TurnEnded.Invoke(this, new EventArgs());

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);
    }

    public void BasicAttackSelection()
    {
        CellGridState = new CellGridStateSkillSelected(this, "twinDaggers", UnitList[Turn]);
    }

    public void FireballSelection()
    {
        CellGridState = new CellGridStateSkillSelected(this, "fireball", UnitList[Turn]);
    }

    public void MovekSelection()
    {
        CellGridState = new CellGridStateUnitSelected(this, UnitList[Turn]);
    }

    /// <summary>
    /// Coroutine lets the unit choose its direction. Then call the 'real' EndTurn()
    /// </summary>
    IEnumerator FacingChoice(Unit unit)
    {
        bool directionChosen = false;

        // activating the directions gameObjects
        GameObject unitGO = unit.transform.GetChild(2).gameObject;
        unitGO.SetActive(true);

        // let the unit choose its direction
        List<GameObject> directions = new List<GameObject>();
        for (int i = 0; i < 6; ++i)
        {
            GameObject directionGO = unitGO.transform.GetChild(i).gameObject;
            directions.Add(directionGO);
        }
        String dGOName = "";

        while (!directionChosen)
        {
            foreach (GameObject dGO in directions)
            {
                if (dGO.GetComponent<OnClickDirectionChoice>().clicked)
                {
                    dGOName = dGO.name;
                    directionChosen = true;
                }
            }
            yield return null;
        }

        switch (dGOName)
        {
            case "Up":
                unit.ChangeFacing(Unit._directions.up);
                break;
            case "UpRight":
                unit.ChangeFacing(Unit._directions.up_right);
                break;
            case "UpLeft":
                unit.ChangeFacing(Unit._directions.up_left);
                break;
            case "DownRight":
                unit.ChangeFacing(Unit._directions.down_right);
                break;
            case "DownLeft":
                unit.ChangeFacing(Unit._directions.down_left);
                break;
            case "Down":
                unit.ChangeFacing(Unit._directions.down);
                break;

        }
        RealEndTurn();
    }

    /// <summary>
    /// Method sorts unitList by Initiative
    /// </summary>
    public List<Unit> UnitListSort(List<Unit> unitList)
    {
        return unitList.OrderByDescending(o => o.Initiative).ToList();
    }

    /// <summary>
    /// Updated unitList without dead units
    /// </summary>
    public List<Unit> UnitListRefresh(List<Unit> unitList)
    {
        return UnitListSort(Units);
        /*List<Unit> resultList = unitList;
        Unit itemToRemove = unitList.SingleOrDefault(u => u.Initiative == 0);
        if (itemToRemove != null)
            resultList.Remove(itemToRemove);
        return resultList;*/
    }


}
