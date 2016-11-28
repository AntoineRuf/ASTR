using UnityEngine;

public class UnitStateMarkedAsFinished : UnitState
{
    public UnitStateMarkedAsFinished(Unit unit) : base(unit)
    {      
    }

    public override void Apply()
    {
        _unit.MarkAsFinished();
        Animator anim = _unit.GetComponentInChildren<Animator>();
        anim.SetBool("Idle", true);
        anim.SetBool("Attack", false);
        anim.SetBool("Skill", false);
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

