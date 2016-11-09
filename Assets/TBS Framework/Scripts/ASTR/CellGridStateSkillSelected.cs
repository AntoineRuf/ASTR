using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

class CellGridStateSkillSelected : CellGridState
{
    private Skill _skill;
    private Unit _unit;
    private List<Unit> _unitsInRange;
    private List<Skill> _skills;

    private Cell _unitCell;
    private bool _targetable;

    public override void OnStateEnter()
    {
        Debug.Log("Cette fonction est appelée à l'entrée dans l'état");
        _unitCell = _unit.Cell;

        if (_unit.ActionPoints <= 0) return;
        foreach (var currentUnit in _cellGrid.Units)
        {
            if (currentUnit.PlayerNumber.Equals(_unit.PlayerNumber))
                continue;

            if (_unit.IsUnitAttackable(currentUnit, _unit.Cell))
            {
                currentUnit.SetState(new UnitStateMarkedAsReachableEnemy(currentUnit));
                _unitsInRange.Add(currentUnit);
            }
        }
    }

    public CellGridStateSkillSelected(CellGrid cellGrid, string skillName, Unit unit) : base(cellGrid)
    {
        _unitsInRange = new List<Unit>();
        _unit = unit;
        if (skillName.Equals("basicAttack")) {
            _skill = new BasicAttack();
            _targetable = false;
        }
        if (skillName.Equals("fireball"))
        {
            _skill = new Fireball();
            _targetable = true;
        }
    }

    public override void OnUnitClicked(Unit unit)
    {  
        if (unit.Equals(_unit) || unit.isMoving)
            return;

        if (_unitsInRange.Contains(unit) && _unit.ActionPoints > 0)
        {
            _skill.Apply(_unit, unit);
            _cellGrid.EndTurn(); // Add rogue condition here
        }
        
    }

    public override void OnCellClicked(Cell cell)
    {
        if (_unit.ActionPoints > 0 && _targetable)
        {
            _skill.Apply(_unit, cell, _cellGrid);
            _cellGrid.EndTurn(); // Add rogue condition here
        }
    }

    public override void OnStateExit()
    {
        foreach (var currentUnit in _cellGrid.Units)
        {
            currentUnit.SetState(new UnitStateNormal(currentUnit));
        }
    }
}


