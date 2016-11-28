using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.TBS_Framework.Scripts.ASTR;
using Assets.TBS_Framework.Scripts.ASTR.Classes.Buffs;

/// <summary>
/// Base class for all units in the game.
/// </summary>
public abstract class Unit : MonoBehaviour
{
    /// <summary>
    /// UnitClicked event is invoked when user clicks the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitClicked;
    /// <summary>
    /// UnitSelected event is invoked when user clicks on unit that belongs to him. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitSelected;
    public event EventHandler UnitDeselected;
    /// <summary>
    /// UnitHighlighted event is invoked when user moves cursor over the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitHighlighted;
    public event EventHandler UnitDehighlighted;
    public event EventHandler UnitForTargetSelected;
    public event EventHandler UnitForTargetDeselected;
    public event EventHandler<AttackEventArgs> UnitAttacked;
    public event EventHandler<AttackEventArgs> UnitDestroyed;
    public event EventHandler<MovementEventArgs> UnitMoved;

    public UnitState UnitState { get; set; }
    public void SetState(UnitState state)
    {
        UnitState.MakeTransition(state);
    }

    public List<Buff> Buffs { get; private set; }

    public int TotalHitPoints { get; set; }
    public int TotalMovementPoints;
    protected int TotalActionPoints;

    /// <summary>
    /// Cell that the unit is currently occupying.
    /// </summary>
    public Cell Cell { get; set; }
    public List<Skill> Skills;
    public string UnitName;
    public int HitPoints;
    public int AttackRange;
    public float AttackFactor;
    public float DefenceFactor;
    public int Initiative;
    public string Image;
    public int CCImmunity;
    /// <summary>
    /// The different directions a unit can point to
    /// </summary>
    public enum _directions
    {
        up,
        up_right,
        down_right,
        down,
        down_left,
        up_left
    };
    public _directions Facing;
    /// <summary>
    /// Determines how far on the grid the unit can move.
    /// </summary>
    public int MovementPoints;
    /// <summary>
    /// Determines speed of movement animation.
    /// </summary>
    public float MovementSpeed;
    /// <summary>
    /// Determines how many attacks unit can perform in one turn.
    /// </summary>
    public int ActionPoints;

    /// <summary>
    /// Indicates the player that the unit belongs to. Should correspoond with PlayerNumber variable on Player script.
    /// </summary>
    public int PlayerNumber;


    /// <summary>
    /// Indicates the team that the unit belongs to. Should correspoond with TeamNumber variable on Player script.
    /// </summary>
    public int TeamNumber;

    /// <summary>
    /// Indicates if movement animation is playing.
    /// </summary>
    public bool isMoving { get; set; }

    private static IPathfinding _pathfinder = new AStarPathfinding();

    /// <summary>
    /// Method called after object instantiation to initialize fields etc.
    /// </summary>
    public virtual void Initialize()
    {
        Buffs = new List<Buff>();
        Skills = new List<Skill>();
        foreach(var skill in GameControl.playerData[CustomUnitGenerator.CurrentUnit].Skills)
        {
            Skills.Add(skill);
        }
        UnitState = new UnitStateNormal(this);
    }

    protected virtual void OnMouseDown()
    {
        if (UnitClicked != null)
            UnitClicked.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseEnter()
    {
        if (UnitHighlighted != null)
            UnitHighlighted.Invoke(this, new EventArgs());
        if (UnitForTargetSelected != null)
            UnitForTargetSelected.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {
        if (UnitDehighlighted != null)
            UnitDehighlighted.Invoke(this, new EventArgs());
        if (UnitForTargetDeselected != null)
            UnitForTargetDeselected.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Method is called at the start of each turn.
    /// </summary>
    public virtual void OnTurnStart()
    {
        MovementPoints = TotalMovementPoints;
        ActionPoints = TotalActionPoints;
        Cell.Occupent = this;
        if (Buffs.Any())
        {
            List<Buff> dotlist = Buffs.FindAll(b => b.isDot);
            if (dotlist != null)
            {
                foreach (Buff b in dotlist)
                {
                    b.Trigger(this);
                }
            }
        }
        if (TotalHitPoints == 120)
        {
            WarriorBuff warbuff = new WarriorBuff();
            warbuff.Apply(this);
        }
        SetState(new UnitStateMarkedAsFriendly(this));
        
    }
    /// <summary>
    /// Method is called at the end of each turn.
    /// </summary>
    public virtual void OnTurnEnd()
    {
        Buffs.FindAll(b => b.Duration == 0).ForEach(b => { b.Undo(this); });
        Buffs.RemoveAll(b => b.Duration == 0);
        Buffs.ForEach(b => { b.Duration--; });
        Animator anim = GetComponentInChildren<Animator>();
        SetState(new UnitStateNormal(this));
    }
    /// <summary>
    /// Method is called when units HP drops below 1.
    /// </summary>
    protected virtual void OnDestroyed()
    {
        Cell.IsTaken = false;
        MarkAsDestroyed();
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetBool("Idle", false);
        anim.SetBool("Dead", true);
        StartCoroutine(DestroyPlayer());
        Cell.Occupent = null;
    }

    protected virtual IEnumerator DestroyPlayer()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        Destroy(gameObject);
    }
    /// <summary>
    /// Method is called when unit is selected.
    /// </summary>
    public virtual void OnUnitSelected()
    {
        SetState(new UnitStateMarkedAsSelected(this));
        if (UnitSelected != null)
            UnitSelected.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// Method is called when unit is deselected.
    /// </summary>
    public virtual void OnUnitDeselected()
    {
        SetState(new UnitStateMarkedAsFriendly(this));
        if (UnitDeselected != null)
            UnitDeselected.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Method indicates if it is possible to attack unit given as parameter, from cell given as second parameter.
    /// </summary>
    public virtual bool IsUnitAttackable(Unit other, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= AttackRange)
            return true;

        return false;
    }


    /// <summary>
    /// Method deals damage to unit given as parameter.
    /// </summary>
    public virtual void DealDamage(Unit other)
    {
        if (isMoving)
            return;
        if (ActionPoints == 0)
            return;

        MarkAsAttacking(other);
        ActionPoints--;
        other.Defend(this, 1);

        if (ActionPoints == 0)
        {
            SetState(new UnitStateMarkedAsFinished(this));
            MovementPoints = 0;
        }
    }
    // ------ ASTR
    public virtual void DealDamage2(Unit other, int damage)
    {
        int dealtDamage = (int)Mathf.Floor(damage * AttackFactor);
        other.Defend(this, dealtDamage);
    }
    // ------------------

    /// <summary>
    /// Attacking unit calls Defend method on defending unit.
    /// </summary>
    protected virtual void Defend(Unit other, int dealtDamage)
    {
        MarkAsDefending(other);
        int receivedDamage = (int)Mathf.Floor (dealtDamage / DefenceFactor);
        HitPoints -= receivedDamage;
        printDamage(receivedDamage+1);
        if (other.Buffs.Find(b => b.Name == "Snake Venom") != null)
        {
            SnakeDot sndot = new SnakeDot();
            Buffs.Add(sndot);
        }
        if (UnitAttacked != null)
            UnitAttacked.Invoke(this, new AttackEventArgs(other, this, receivedDamage));

        if (HitPoints <= 0)
        {
            if (UnitDestroyed != null)
                UnitDestroyed.Invoke(this, new AttackEventArgs(other, this, receivedDamage));
            OnDestroyed();
        }

    }

    public virtual void Move(Cell destinationCell, List<Cell> path, TrapManager trapmanager)
    {
        if (isMoving)
            return;

        var totalMovementCost = path.Sum(h => h.MovementCost);
        if (MovementPoints < totalMovementCost)
            return;

        MovementPoints -= totalMovementCost;

        Cell.IsTaken = false;
        Cell.Occupent = null;
        bool trapfound = false;
        List<Cell> TrapPath = new List<Cell>();
        foreach(Cell c in path)
        {
            if (!trapfound)
            {
                TrapPath.Add(c);
            }
            if (trapmanager.findTrap(c))
            {
                trapmanager.Trigger(this);
                trapfound = true;
            }
        }
        TrapPath.Reverse();
        Cell = destinationCell;
        destinationCell.IsTaken = true;
        destinationCell.Occupent = this;

        if (MovementSpeed > 0 && trapfound)
            StartCoroutine(MovementAnimation(TrapPath));
        else if (MovementSpeed > 0 && !trapfound)
            StartCoroutine(MovementAnimation(path));
        else
            transform.position = Cell.transform.position;

        if (UnitMoved != null && trapfound)
        {
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, TrapPath));
        }
        else if (UnitMoved != null && !trapfound)
        {
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));
        }

    }

    public virtual void Dash(Cell destinationCell, List<Cell> path, TrapManager trapmanager)
    {
        Cell.IsTaken = false;
        Cell.Occupent = null;
        bool trapfound = false;
        List<Cell> TrapPath = new List<Cell>();
        foreach(Cell c in path)
        {
            if (!trapfound)
            {
                TrapPath.Add(c);
            }
            if (trapmanager.findTrap(c))
            {
                trapmanager.Trigger(this);
                trapfound = true;
            }
        }
        TrapPath.Reverse();
        Cell = destinationCell;
        Cell.IsTaken = true;
        Cell.Occupent = this;

        if (MovementSpeed > 0 && trapfound)
            StartCoroutine(RunAnimation(TrapPath));
        else if (MovementSpeed > 0 && !trapfound)
            StartCoroutine(RunAnimation(path));
        else
            transform.position = Cell.transform.position;

        if (UnitMoved != null && trapfound)
        {
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, TrapPath));
        }
        else if (UnitMoved != null && !trapfound)
        {
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));
        }
    }

    protected virtual IEnumerator MovementAnimation(List<Cell> path)
    {
        isMoving = true;
        Animator anim = this.GetComponentInChildren<Animator>();
        anim.SetBool("Idle", false);
        anim.SetBool("Walk", true);
        path.Reverse();
        foreach (var cell in path)
        {
            while (new Vector2(transform.position.x,transform.position.y) != new Vector2(cell.transform.position.x,cell.transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x,cell.transform.position.y,transform.position.z), Time.deltaTime * MovementSpeed);
                yield return 0;
            }
        }
        isMoving = false;
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", true);
    }

    protected virtual IEnumerator RunAnimation(List<Cell> path)
    {
        isMoving = true;
        Animator anim = this.GetComponentInChildren<Animator>();
        anim.SetBool("Idle", false);
        anim.SetBool("Run", true);
        foreach (var cell in path)
        {
            while (new Vector2(transform.position.x,transform.position.y) != new Vector2(cell.transform.position.x,cell.transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x,cell.transform.position.y,transform.position.z), Time.deltaTime * MovementSpeed * 2);
                yield return 0;
            }
        }
        isMoving = false;
        anim.SetBool("Run", false);
        anim.SetBool("Idle", true);
    }

    ///<summary>
    /// Method indicates if unit is capable of moving to cell given as parameter.
    /// </summary>
    public virtual bool IsCellMovableTo(Cell cell)
    {
        return !cell.IsTaken;
    }
    /// <summary>
    /// Method indicates if unit is capable of moving through cell given as parameter.
    /// </summary>
    public virtual bool IsCellTraversable(Cell cell)
    {
        return !cell.IsTaken;
    }
    /// <summary>
    /// Method returns all cells that the unit is capable of moving to.
    /// </summary>
    public List<Cell> GetAvailableDestinations(List<Cell> cells)
    {
        var ret = new List<Cell>();
        var cellsInMovementRange = cells.FindAll(c => IsCellMovableTo(c) && c.GetDistance(Cell) <= MovementPoints);

        var traversableCells = cells.FindAll(c => IsCellTraversable(c) && c.GetDistance(Cell) <= MovementPoints);
        traversableCells.Add(Cell);

        foreach (var cellInRange in cellsInMovementRange)
        {
            if (cellInRange.Equals(Cell)) continue;

            var path = FindPath(traversableCells, cellInRange);
            var pathCost = path.Sum(c => c.MovementCost);
            if (pathCost > 0 && pathCost <= MovementPoints)
                ret.AddRange(path);
        }
        return ret.FindAll(IsCellMovableTo).Distinct().ToList();
    }

    public List<Cell> FindPath(List<Cell> cells, Cell destination)
    {
        return _pathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
    }
    /// <summary>
    /// Method returns graph representation of cell grid for pathfinding.
    /// </summary>
    protected virtual Dictionary<Cell, Dictionary<Cell, int>> GetGraphEdges(List<Cell> cells)
    {
        Dictionary<Cell, Dictionary<Cell, int>> ret = new Dictionary<Cell, Dictionary<Cell, int>>();
        foreach (var cell in cells)
        {
            if (IsCellTraversable(cell) || cell.Equals(Cell))
            {
                ret[cell] = new Dictionary<Cell, int>();
                foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                {
                    ret[cell][neighbour] = neighbour.MovementCost;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// Gives visual indication that the unit is under attack.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsDefending(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is attacking.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsAttacking(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
    /// destroyed, so either instantiate some new object to indicate destruction or redesign Defend method.
    /// </summary>
    public abstract void MarkAsDestroyed();

    /// <summary>
    /// Method marks unit as current players unit.
    /// </summary>
    public abstract void MarkAsFriendly();
    /// <summary>
    /// Method mark units to indicate user that the unit is in range and can be attacked.
    /// </summary>
    public abstract void MarkAsReachableEnemy();
    /// <summary>
    /// Method marks unit as currently selected, to distinguish it from other units.
    /// </summary>
    public abstract void MarkAsSelected();
    /// <summary>
    /// Method marks unit to indicate user that he can't do anything more with it this turn.
    /// </summary>
    public abstract void MarkAsFinished();
    /// <summary>
    /// Method returns the unit to its base appearance
    /// </summary>
    public abstract void UnMark();


    // ---------------------------------- ASTR

    /// <summary>
    /// Method returns currentUnit in unitList
    /// </summary>
    public Unit FindCurrent(List<Unit> unitList)
    {
        return unitList.Find(u => this);
    }

    /// <summary>
    /// Method returns currentUnit index in unitList
    /// </summary>
    public int FindCurrentIndex(List<Unit> unitList)
    {
        return unitList.FindIndex(u => this);
    }

    /// <summary>
    /// Method returns the next Unit in the unitList
    /// </summary>
    public Unit FindNext(List<Unit> unitList)
    {
        int current = unitList.FindIndex(u => this);
        if (current + 1 < unitList.Count()) return unitList.Find(u => unitList[current + 1]);
        else return unitList[0];
    }

    /// <summary>
    /// Method returns the next Unit index in the unitList
    /// </summary>
    public int FindNextIndex(List<Unit> unitList)
    {
        if (FindCurrentIndex(unitList) + 1 < unitList.Count()) return FindCurrentIndex(unitList) + 1;
        else return 0; //unit was last in unitList
    }

    /// <summary>
    /// Method prints damage taken above the unit
    /// </summary>
    public void printDamage(int damage)
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        String damageToPrint = Mathf.Clamp(damage - this.DefenceFactor, 1, damage).ToString();
        StartCoroutine(ShowMessage(GetComponentInChildren<TextMesh>(), damageToPrint, 2));
    }

    public void TrapNotice(string TrapMessage)
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        StartCoroutine(ShowMessage(GetComponentInChildren<TextMesh>(), TrapMessage, 2));
    }

    IEnumerator ShowMessage(TextMesh textMesh, string message, float delay)
    {
        textMesh.text = message;
        textMesh.transform.rotation = Camera.main.transform.rotation;
        yield return new WaitForSeconds(delay);
        textMesh.text = "";
    }

    /// <summary>
    /// Change the direction the Unit is facing
    /// </summary>
    public void ChangeFacing(_directions dir)
    {
        Facing = dir;

        //disable facing points gameObjects
        transform.GetChild(2).gameObject.SetActive(false);

        //change FacileTile
        transform.GetChild(3).GetComponent<FacingTile>().ChangeFacingTile(dir);
    }

    /// <summary>
    /// Compare the attacker's position and the facing of the defender.
    /// </summary>
    public int FacingComparison(Unit defender)
    {
        int backstab = 3;
        int critical = 1;
        Vector2 attackerCoord = Cell.OffsetCoord;
        Vector2 defenderCoord = defender.Cell.OffsetCoord;

        switch(defender.Facing)
        {
            case _directions.up:

                if (attackerCoord.y > defenderCoord.y) // attacker behind defender
                {
                    if (isBehind(defender)) return backstab;
                    else return critical;
                }
                break;
            case _directions.up_right:
                if (attackerCoord.x < defenderCoord.x || attackerCoord.y > defenderCoord.y ) // attacker behind defender
                {
                    if (isBehind(defender)) return backstab;
                    else return critical;
                }
                break;
            case _directions.up_left:
                if (attackerCoord.x > defenderCoord.x || attackerCoord.y > defenderCoord.y) // attacker behind defender
                {
                    if (isBehind(defender)) return backstab;
                    else return critical;
                }
                break;
            case _directions.down:
                if (attackerCoord.y < defenderCoord.y) // attacker behind defender
                {
                    if (isBehind(defender)) return backstab;
                    else return critical;
                }
                break;
            case _directions.down_right:
                if (attackerCoord.x < defenderCoord.x || attackerCoord.y < defenderCoord.y) // attacker behind defender
                {
                    if (isBehind(defender)) return backstab;
                    else return critical;
                }
                break;
            case _directions.down_left:
                if (attackerCoord.x > defenderCoord.x || attackerCoord.y < defenderCoord.y) // attacker behind defender
                {
                    if (isBehind(defender)) return backstab;
                    else return critical;
                }
                break;
        }
        return 0;
    }

    /// <summary>
    /// Checks if a unit is behind another one using HexGrid Calculations
    /// </summary>
    public bool isBehind(Unit unit)
    {
        int range = 1; // max range
        Vector2 positionOffsetCoord = Cell.OffsetCoord;
        Vector2 unitPositionOffsetCoord = unit.Cell.OffsetCoord;
        Vector3 unitPositionCube = ConvertToCube(unitPositionOffsetCoord);
        Vector3 up = new Vector3(0, 1, -1);
        Vector3 down = new Vector3(0, -1, 1);
        Vector3 upL = new Vector3(+1, 0, -1);
        Vector3 upR = new Vector3(-1, 1, 0);
        Vector3 downL = new Vector3(+1, -1, 0);
        Vector3 downR = new Vector3(-1, 0, 1);
        Vector3 direction = new Vector3();
        switch (unit.Facing)
        {
            case _directions.up:
                direction = down;
                break;
            case _directions.up_right:
                direction = downL;
                break;
            case _directions.up_left:
                direction = downR;
                break;
            case _directions.down:
                direction = up;
                break;
            case _directions.down_right:
                direction = upL;
                break;
            case _directions.down_left:
                direction = upR;
                break;
        }
        for (int i = 0; i < range; ++i)
        {
            unitPositionCube += direction;
            if (ConvertToOffsetCoord(unitPositionCube).Equals(positionOffsetCoord))
            {
                return true;
            }

        }
        return false;

    }

    public void BuffUpdate()
    {
        for (int i = 0; i < Buffs.Count(); ++i)
        {
            Buffs[i].Duration--;
            if (Buffs[i].Duration == 0 )
            {
                Buffs.Remove(Buffs[i]);
            }
        }
    }

    /// <summary>
    /// Convert OffsetCoord to Cube
    /// </summary>
    public Vector2 ConvertToOffsetCoord(Vector3 v)
    {
        return new Vector2(v.x, (v.z + (v.x + (Mathf.Abs(v.x) % 2)) / 2));
    }

    /// <summary>
    /// Convert OffsetCoord to Cube
    /// </summary>
    public Vector3 ConvertToCube(Vector2 v)
    {
        float x = v.x;
        float z = v.y - (v.x + (Mathf.Abs(v.x) % 2)) / 2;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

}

public class MovementEventArgs : EventArgs
{
    public Cell OriginCell;
    public Cell DestinationCell;
    public List<Cell> Path;

    public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path)
    {
        OriginCell = sourceCell;
        DestinationCell = destinationCell;
        Path = path;
    }
}
public class AttackEventArgs : EventArgs
{
    public Unit Attacker;
    public Unit Defender;

    public int Damage;

    public AttackEventArgs(Unit attacker, Unit defender, int damage)
    {
        Attacker = attacker;
        Defender = defender;

        Damage = damage;
    }
}
