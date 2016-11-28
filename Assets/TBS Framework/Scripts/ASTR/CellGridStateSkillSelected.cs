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
        if (unit.isMoving)
            return;

        if ((_unitsInRange.Contains(unit) || (unit.Equals(_unit) && _skill.MinRange == 0)) && _unit.ActionPoints > 0)
        {
            Debug.Log("Units affected : " + _unitsAffected.Count());
            _skill.Apply(_unit, _unitsAffected, _cellGrid);
            //foreach (var unitToUpdate in _unitsAffected) {
            //_cellGrid.HealthbarUpdate(unitToUpdate, unitToUpdate.HitPoints, _cellGrid.FullHealthbar);
            //}
            _cellGrid.AddSkillCooldownGUI(_unit.Skills.FindIndex(s => (s.Name == _skill.Name)));
            _cellGrid.EndTurn(); // Add rogue condition here
            _unitsAffected.Clear();
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
            else if (_skill.isAoE == 1) //Circle AoE
            {
                foreach (var currentCell in _cellGrid.Cells.FindAll(c => c.GetDistance(cell) <= _skill.AoERange))
                {
                    currentCell.MarkAsSkillRangeSelected();
                    _cellsAffected.Add(currentCell);
                    if (currentCell.Occupent != null)
                    {
                        _unitsAffected.Add(currentCell.Occupent);
                    }
                }
            }
            else if (_skill.isAoE == 3) //Perpendicular Line AoE
            {
                Vector3 cellCubeCoord = _unit.ConvertToCube(cell.OffsetCoord);
                Vector3 unitCubeCoord = _unit.ConvertToCube(_unit.Cell.OffsetCoord);
                Vector3 direction = (cellCubeCoord - unitCubeCoord) / (cell.GetDistance(_unit.Cell));
                UnityEngine.Debug.Log(direction.ToString());
                Vector3 up = new Vector3(0, 1, -1);
                Vector3 down = new Vector3(0, -1, 1);
                Vector3 upR = new Vector3(+1, 0, -1);
                Vector3 upL = new Vector3(-1, 1, 0);
                Vector3 downR = new Vector3(+1, -1, 0);
                Vector3 downL = new Vector3(-1, 0, 1);
                if (direction == up)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downR);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downL);
                    _cellsAffected.Add(cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == down)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upR);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upL);
                    _cellsAffected.Add(cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == upR)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upL);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + down);
                    _cellsAffected.Add(cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == upL)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upR);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + down);
                    _cellsAffected.Add(cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == downR)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + up);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downL);
                    _cellsAffected.Add(cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == downL)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + up);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downR);
                    _cellsAffected.Add(cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                foreach (Cell c in _cellsAffected)
                {
                    c.MarkAsSkillRangeSelected();
                    if (c.Occupent != null)
                    {
                        _unitsAffected.Add(c.Occupent);
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
            if (_skill.isAoE == 0)
            { // spell cast is NOT an AoE
                unit.Cell.MarkAsSkillRangeSelected();
                _unitsAffected.Add(unit);
            }
            else if (_skill.isAoE == 1)// spell cast is an AoE
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
            else if (_skill.isAoE == 3)
            {
                Vector3 cellCubeCoord = _unit.ConvertToCube(unit.Cell.OffsetCoord);
                Vector3 unitCubeCoord = _unit.ConvertToCube(_unit.Cell.OffsetCoord);
                Vector3 direction = (cellCubeCoord - unitCubeCoord) / (unit.Cell.GetDistance(_unit.Cell));
                Debug.Log(direction.ToString());
                Vector3 up = new Vector3(0, 1, -1);
                Vector3 down = new Vector3(0, -1, 1);
                Vector3 upR = new Vector3(+1, 0, -1);
                Vector3 upL = new Vector3(-1, 1, 0);
                Vector3 downR = new Vector3(+1, -1, 0);
                Vector3 downL = new Vector3(-1, 0, 1);
                if (direction == up)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downR);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downL);
                    _cellsAffected.Add(unit.Cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == down)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upR);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upL);
                    _cellsAffected.Add(unit.Cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == upR)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upL);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + down);
                    _cellsAffected.Add(unit.Cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == upL)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + upR);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + down);
                    _cellsAffected.Add(unit.Cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == downR)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + up);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downL);
                    _cellsAffected.Add(unit.Cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                else if (direction == downL)
                {
                    Vector2 cellToAdd1OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + up);
                    Vector2 cellToAdd2OffsetCoord = _unit.ConvertToOffsetCoord(cellCubeCoord + downR);
                    _cellsAffected.Add(unit.Cell);
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd1OffsetCoord));
                    if (_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord) != null)
                        _cellsAffected.Add(_cellGrid.Cells.Find(c => c.OffsetCoord == cellToAdd2OffsetCoord));
                }
                foreach (Cell c in _cellsAffected)
                {
                    c.MarkAsSkillRangeSelected();
                    if (c.Occupent != null)
                    {
                        _unitsAffected.Add(c.Occupent);
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
