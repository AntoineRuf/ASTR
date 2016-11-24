using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Assets.TBS_Framework.Scripts.ASTR;

class CellGridStateSkillSelected : CellGridState
{
    private Skill _skill;
    private Unit _unit;
    private List<Unit> _unitsInRange;
    private List<Unit> _unitsAffected;
    private List<Cell> _cellsInRange;
    private List<Cell> _cellsAffected;
    private List<Skill> _skills;

    public override void OnStateEnter()
    {
        if (_unit.ActionPoints <= 0) return;
            foreach (var currentCell in _cellGrid.Cells)
            {
                if (!_unit.Cell == currentCell && _skill.CanTargetSelf) // unit can't target herself
                    continue;
                if (_unit.Cell.IsCellTargetable(currentCell, _skill.MinRange, _skill.MaxRange, false))
                {
                    if (_skill.AlignmentNeeded)
                    {
                        if (_unit.Cell.isCellInLine(currentCell, _skill.MaxRange))
                        {
                            currentCell.MarkAsSkillRange();
                            _cellsInRange.Add(currentCell);
                        }
                    }
                    else
                    {
                        currentCell.MarkAsSkillRange();
                        _cellsInRange.Add(currentCell);
                    }
                }
            }

        if (_skill.CanTargetEnemies || _skill.CanTargetAllies)
        {
            foreach (var currentUnit in _cellGrid.Units)
            {
                if (currentUnit.PlayerNumber.Equals(_unit.PlayerNumber) && (!_skill.CanTargetSelf))
                    continue;

                if (currentUnit.TeamNumber.Equals(_unit.TeamNumber) && (!_skill.CanTargetAllies))
                    continue;

                if (!(currentUnit.TeamNumber.Equals(_unit.TeamNumber)) && (!_skill.CanTargetEnemies))
                    continue;

                if (_unit.Cell.IsCellTargetable(currentUnit.Cell, _skill.MinRange, _skill.MaxRange, false))
                {
                    if (_skill.AlignmentNeeded)
                    {
                        if (_unit.Cell.isCellInLine(currentUnit.Cell, _skill.MaxRange))
                        {
                            currentUnit.SetState(new UnitStateMarkedAsReachableEnemy(currentUnit));
                            _unitsInRange.Add(currentUnit);
                        }
                    }
                    else
                    {
                        currentUnit.SetState(new UnitStateMarkedAsReachableEnemy(currentUnit));
                        _unitsInRange.Add(currentUnit);
                    }                    
                }
            }
        }
    }

    public CellGridStateSkillSelected(CellGrid cellGrid, Skill skill, Unit unit) : base(cellGrid)
    {
        _unitsInRange = new List<Unit>();
        _unitsAffected = new List<Unit>();
        _cellsInRange = new List<Cell>();
        _cellsAffected = new List<Cell>();
    _unit = unit;
        _skill = skill;
    }

    public override void OnUnitClicked(Unit unit)
    {  
        if (unit.Equals(_unit) || unit.isMoving)
            return;

        if (_unitsInRange.Contains(unit) && _unit.ActionPoints > 0)
        {
            Debug.Log("Units affected : " + _unitsAffected.Count());    
            _skill.Apply(_unit, _unitsAffected);
            _cellGrid.AddSkillCooldownGUI(_unit.Skills.FindIndex(s => (s.Name == _skill.Name)));
            _cellGrid.EndTurn(); // Add rogue condition here
        }
        
    }

    public override void OnCellClicked(Cell cell)
    {
        if (_unit.ActionPoints > 0 && _cellsInRange.Contains(cell) && _skill.CanTargetEmptyCell)
        {
            _skill.Apply(_unit, _cellsAffected, _cellGrid);
            _cellGrid.AddSkillCooldownGUI(_unit.Skills.FindIndex(s => (s.Name == _skill.Name)));
            _cellGrid.EndTurn(); // Add rogue condition here
        }
        else if (_cellsInRange.Contains(cell)) { }
        else
        {
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, _unit);
            _unitsAffected.Clear();
        }
    }

    public override void OnCellSelected(Cell cell)
    {
        if (_cellsInRange.Contains(cell) && _skill.CanTargetEmptyCell)
        {
            if (_skill.isAoE == 0) //spell cast is NOT an AoE
            { 
                cell.MarkAsSkillRangeSelected();
                _cellsAffected.Add(cell);
                if (cell.Occupent != null) _unitsAffected.Add(cell.Occupent);
            }
            else // spell cast is an AoE
            {
                foreach (var currentCell in _cellGrid.Cells.FindAll(c => c.GetDistance(cell) <= _skill.AoERange))
                {
                    currentCell.MarkAsSkillRangeSelected();
                    _cellsAffected.Add(currentCell);
                    if (currentCell.Occupent != null) {
                        _unitsAffected.Add(currentCell.Occupent);
                    }
                }
            }
        }
    }


    public override void OnCellDeselected(Cell cell)
    {
        foreach (var currentCell in _cellsAffected)
        {
            if (_cellsInRange.Contains(currentCell))
                currentCell.MarkAsSkillRange();
            else currentCell.UnMark();
        }
        _cellsAffected.Clear();
        _unitsAffected.Clear();
        if (_cellsInRange.Contains(cell))
        {
            cell.MarkAsSkillRange();
        }
        
    }

    public override void OnUnitForTargetSelected(Unit unit)
    {
        if (_cellsInRange.Contains(unit.Cell))
        {
            if (_skill.isAoE == 0) // spell cast is NOT an AoE
                unit.Cell.MarkAsSkillRangeSelected();
            else // spell cast is an AoE
            {
                foreach (var currentCell in _cellGrid.Cells.FindAll(c => c.GetDistance(unit.Cell) <= _skill.AoERange))
                {
                    currentCell.MarkAsSkillRangeSelected();
                    _cellsAffected.Add(currentCell);
                    if (currentCell.Occupent != null)
                    {
                        _unitsAffected.Add(currentCell.Occupent);
                    }
                }
            }
        }
    }

    public override void OnUnitForTargetDeselected(Unit unit)
    {
        OnCellDeselected(unit.Cell);
    }

    public override void OnStateExit()
    {
        foreach (var currentUnit in _cellGrid.Units)
        {
            currentUnit.SetState(new UnitStateNormal(currentUnit));
        }
        foreach (var currentCell in _cellsInRange)
        {
            currentCell.UnMark();
        }
        foreach (var currentCell in _cellsAffected)
        {
            currentCell.UnMark();
        }
        _cellsInRange.Clear();
        _cellsAffected.Clear();
        _unitsAffected.Clear();
        _unitsInRange.Clear();
    }
}


