using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Assets.TBS_Framework.Scripts.ASTR;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

/// <summary>
/// CellGrid class keeps track of the game, stores cells, units and players objects. It starts the game and makes turn transitions. 
/// It reacts to user interacting with units or cells, and raises events related to game progress. 
/// </summary>
public class CellGrid : MonoBehaviour
{
    public event EventHandler GameStarted;
    public event EventHandler GameEnded;
    public event EventHandler TurnEnded;


    // UI objects declaration
    GameObject canvas;
    Transform SkillPanel;
    Transform CooldownPanel;
    Transform BuffPanel;
    Transform FullHealthbar;
    Transform EmptyHealthbar;
    Transform HealthText;
    Transform MouseOverPannel;
    Transform Portrait;
    // ---

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
    public TrapManager trapmanager;

    void Start()
    {
        // UI initialization
        canvas = GameObject.Find("UnitCanvas");
        SkillPanel = canvas.transform.FindChild("SkillPanel");
        CooldownPanel = canvas.transform.FindChild("CooldownPanel");
        BuffPanel = canvas.transform.FindChild("BuffsPanel");
        FullHealthbar = canvas.transform.FindChild("Healthbar").transform.FindChild("fullHealthbar");
        EmptyHealthbar = canvas.transform.FindChild("Healthbar").transform.FindChild("emptyHealthbar");
        HealthText = canvas.transform.FindChild("Healthbar").transform.FindChild("HealthText");
        MouseOverPannel = canvas.transform.FindChild("MouseOverPannel");
        Portrait = canvas.transform.FindChild("Portrait");
        // ---
        Turn = 0;
        trapmanager = new TrapManager();
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
        UpdateUnitUI();
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
            UnitList[Turn].transform.GetChild(2).GetChild(i).GetComponent<SpriteRenderer>().sprite = 
                UnitList[Turn].transform.GetChild(2).GetChild(i).GetComponent<OnClickDirectionChoice>().Start;
        }
        
        UnitListRefresh(UnitList);
        Turn = (Turn + 1) % Units.Count();

        // updating the unitGUI
        UpdateUnitUI();

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
        //CellGridState = new CellGridStateSkillSelected(this, "twinDaggers", UnitList[Turn]);
    }

    public void FireballSelection()
    {
        //CellGridState = new CellGridStateSkillSelected(this, "fireball", UnitList[Turn]);
    }

    public void MovekSelection()
    {
        CellGridState = new CellGridStateUnitSelected(this, UnitList[Turn]);
    }

    public void WeaknessTrapSelection()
    {
        //CellGridState = new CellGridStateSkillSelected(this, "Weakness Trap", UnitList[Turn]);
    }

    public void OnSkillClicked(Skill skill, int cooldown)
    {
        if (cooldown != 0)
            CellGridState = new CellGridStateWaitingForInput(this);
        else 
            CellGridState = new CellGridStateSkillSelected(this, skill, UnitList[Turn]);
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
        List<Unit> resultList = unitList;
        Unit itemToRemove = unitList.SingleOrDefault(u => u.Initiative == 0);
        if (itemToRemove != null)
            resultList.Remove(itemToRemove);
        return resultList;
    }

    // UI FUNCTIONS
    private void UpdateUnitUI()
    {
        Unit currentUnit = UnitList[Turn];
        //updating name
        NameUpdate(currentUnit, canvas.transform.FindChild("UnitName"));
        //updating portrait
        PortraitUpdate(currentUnit, Portrait, currentUnit.TeamNumber);

        //updating skill lists & cooldowns
        SkillsUpdate(currentUnit);
        
        //updating buffs list
        BuffsUpdate(currentUnit, BuffPanel);

        //updating Healthbar
        HealthbarUpdate(currentUnit, HealthText, FullHealthbar);
    }

    public void MouseEnterUnitUI()
    {
        MouseOverPannel.gameObject.SetActive(true);
    }

    public void MouseExitUnitUI()
    {
        MouseOverPannel.gameObject.SetActive(false);
    }

    public void AddSkillCooldownGUI(int childNumber)
    {
        Unit currentUnit = UnitList[Turn];
        GameObject canvas = GameObject.Find("CurrentUnitCanvas");
        // skill is disabled
        CooldownPanel.GetChild(childNumber).GetComponent<Button>().interactable = false;
        CooldownPanel.GetChild(childNumber).GetComponent<Image>().color = new Color(171, 171, 171, 168);
        CooldownPanel.GetChild(childNumber).GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/SkillsImages/CD" + currentUnit.Skills[childNumber].CurrentCooldown + ".png");
    }

    public void printBuffTooltip (string name, string text, int duration, Image parent)
    {
        Transform tooltip = Instantiate(AssetDatabase.LoadAssetAtPath<Transform>("Assets/TBS Framework/Prefabs/ASTR/BuffTooltip.prefab")) as Transform;
        tooltip.FindChild("Name").GetComponent<Text>().text = name;
        tooltip.FindChild("Description").GetComponent<Text>().text = text;
        tooltip.FindChild("CD").GetComponent<Text>().text = string.Format("{0}", duration);
        tooltip.transform.SetParent(parent.transform);
        tooltip.localPosition = new Vector3(110, -70, 0);
    }

    public void printSkillTooltip(string name, int minDamage, int maxDamage, int minRange, int maxRange, int CD, string text, Transform parent)
    {
        Transform tooltip = Instantiate(AssetDatabase.LoadAssetAtPath<Transform>("Assets/TBS Framework/Prefabs/ASTR/SkillTooltip.prefab")) as Transform;
        tooltip.FindChild("Name").GetComponent<Text>().text = name;
        tooltip.FindChild("Description").GetComponent<Text>().text = text;
        tooltip.FindChild("CD").GetComponent<Text>().text = string.Format("{0}", CD);
        tooltip.FindChild("Range").GetComponent<Text>().text = string.Format("{0}-{1}", minRange, maxRange);
        tooltip.FindChild("Damage").GetComponent<Text>().text = string.Format("{0}-{1}", minDamage, maxDamage);
        tooltip.transform.SetParent(parent.transform);
        tooltip.localPosition = new Vector3(150, 0, 0);
    }

    public void deleteBuffTooltip(Image parent)
    {
        Transform child = parent.transform.GetChild(0);
        Destroy(child.gameObject);
    }

    public void deleteSkillTooltip(Transform parent)
    {
        Transform child = parent.transform.GetChild(0);
        Destroy(child.gameObject);
    }

    public void BuffsUpdate(Unit currentUnit, Transform BuffPanel)
    {
        foreach (Transform child in BuffPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < currentUnit.Buffs.Count(); ++i)
        {
            Image BuffImage = Instantiate(AssetDatabase.LoadAssetAtPath<Image>("Assets/TBS Framework/Prefabs/ASTR/" + currentUnit.Buffs[i].Name + ".prefab")) as Image;
            BuffImage.rectTransform.SetParent(BuffPanel);
            BuffImage.rectTransform.localPosition = new Vector3(-147 + 40*i, 17, 0);

            // buffs tooltips
            EventTrigger trigger = BuffImage.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            string currentBuffName = currentUnit.Buffs[i].Name;
            string currentBuffTooltip = currentUnit.Buffs[i].Tooltip;
            int currentBuffDuration = currentUnit.Buffs[i].Duration;
            entry.callback.AddListener((eventData) => { printBuffTooltip(currentBuffName, currentBuffTooltip, currentBuffDuration, BuffImage); });
            trigger.triggers.Clear();
            trigger.triggers.Add(entry);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) => { deleteBuffTooltip(BuffImage); });
            trigger.triggers.Add(exit);
        }
    }

    public void SkillsUpdate(Unit currentUnit)
    {
        for (int i = 0; i < currentUnit.Skills.Count(); ++i)
        {
            // setting the skill images
            SkillPanel.GetChild(i).GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/SkillsImages/" + currentUnit.Skills[i].Name + ".png");
            // updating skills cooldowns
            if (currentUnit.Skills[i].CurrentCooldown > 0)
            {
                currentUnit.Skills[i].CurrentCooldown--;
                if (currentUnit.Skills[i].CurrentCooldown == 0) // skill is enabled
                {
                    CooldownPanel.GetChild(i).GetComponent<Button>().interactable = true;
                    CooldownPanel.GetChild(i).GetComponent<Image>().sprite = null;
                    CooldownPanel.GetChild(i).GetComponent<Image>().color = new Color(171, 171, 171, 0);
                }
                else { // skill is disabled
                    CooldownPanel.GetChild(i).GetComponent<Button>().interactable = false;
                    CooldownPanel.GetChild(i).GetComponent<Image>().color = new Color(171, 171, 171, 168);
                    CooldownPanel.GetChild(i).GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/SkillsImages/CD" + currentUnit.Skills[i].CurrentCooldown + ".png");
                }
            }
            else // skill is enabled
            {
                CooldownPanel.GetChild(i).GetComponent<Button>().interactable = true;
                CooldownPanel.GetChild(i).GetComponent<Image>().sprite = null;
                CooldownPanel.GetChild(i).GetComponent<Image>().color = new Color(171, 171, 171, 0);
            }
            Skill newSkill = currentUnit.Skills[i];
            int newSkillCD = newSkill.CurrentCooldown;
            // setting the OnClick() method for each skill
            CooldownPanel.GetChild(i).GetComponent<Button>().onClick.AddListener(() => OnSkillClicked(newSkill, newSkillCD));
            // skills tooltips
            EventTrigger trigger = CooldownPanel.GetChild(i).GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            string currentSkillName = currentUnit.Skills[i].Name;
            string currentSkillTooltip = currentUnit.Skills[i].Tooltip;
            int currentSkillCD = currentUnit.Skills[i].Cooldown;
            int currentSkillMinRange = currentUnit.Skills[i].MinRange;
            int currentSKillMaxRange = currentUnit.Skills[i].MaxRange;
            int currentSkillMinDamage = currentUnit.Skills[i].MinDamage;
            int currentSKillMaxDamage = currentUnit.Skills[i].MaxDamage;
            Transform currentSkillObject = SkillPanel.GetChild(i);
            entry.callback.AddListener((eventData) => { printSkillTooltip(currentSkillName, currentSkillMinDamage, currentSKillMaxDamage, currentSkillMinRange, currentSKillMaxRange, currentSkillCD, currentSkillTooltip, currentSkillObject); });
            trigger.triggers.Clear();
            trigger.triggers.Add(entry);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) => { deleteSkillTooltip(currentSkillObject); });
            trigger.triggers.Add(exit);
        }
    }

    public void HealthbarUpdate(Unit currentUnit, Transform HealthText, Transform FullHealthbar)
    {
        float hpScale = (float)currentUnit.HitPoints / currentUnit.TotalHitPoints;
        HealthText.GetComponent<Text>().text = string.Format("{0} {1} {2}", currentUnit.HitPoints, "/", currentUnit.TotalHitPoints);
        FullHealthbar.GetComponent<Image>().rectTransform.localScale = new Vector3(hpScale, 1, 1);
    }

    public void NameUpdate(Unit currentUnit, Transform textComponent)
    {
        textComponent.GetComponentInChildren<Text>().text = currentUnit.UnitName;
    }

    public void PortraitUpdate(Unit currentUnit, Transform Portrait, int teamNumber)
    {
        Portrait.FindChild("Image").GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/ASTR/Portraits/" + currentUnit.Image + ".png");
        if (teamNumber == currentUnit.TeamNumber)
        {
            Portrait.FindChild("GlowEnemy").gameObject.SetActive(false);
            Portrait.FindChild("GlowFriendly").gameObject.SetActive(true);
        }
        else
        {
            Portrait.FindChild("GlowEnemy").gameObject.SetActive(true);
            Portrait.FindChild("GlowFriendly").gameObject.SetActive(false);
        }
    }

    // END OF UI FUNCTIONS
}
