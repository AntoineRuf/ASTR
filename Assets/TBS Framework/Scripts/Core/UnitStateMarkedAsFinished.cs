using UnityEngine;

public class UnitStateMarkedAsFinished : UnitState
{
    public UnitStateMarkedAsFinished(Unit unit) : base(unit)
    {      
    }

    public override void Apply()
    {
        _unit.MarkAsFinished();
        _unit.GetComponentInChildren<Animator>().SetBool("Idle", true);
        _unit.GetComponentInChildren<Animator>().SetBool("Attack", false);
    }

    public override void MakeTransition(UnitState state)
    {
        if(state is UnitStateNormal)
        {
            state.Apply();
            _unit.UnitState = state;
        }
    }
}

