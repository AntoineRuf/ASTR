using UnityEngine;
using System.Collections.Generic;

public class EvasiveManeuver : Skill
{

    public override string Name
    {
        get { return "Evasive Maneuver"; }
        set { }
    }

    public override string Tooltip
    {
        get
        {
            return "Basic rogue attack. Throw daggers to a cell";
        }

        set
        {
            base.Tooltip = value;
        }
    }

    public override int MinRange
    {
        get { return 1; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 3; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 14; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 17; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 1; }
        set { }
    }

    public override bool CanTargetEmptyCell
    {
        get { return false; }//testing purpose
        set { }
    }

    public override bool CanTargetEnemies
    {
        get { return true; }
        set { }
    }

    public override bool CanTargetAllies
    {
        get { return false; }
        set { }
    }

    public override bool AlignmentNeeded
    {
        get { return true; }
        set { }
    }

    public override int isAoE
    {
        get { return 1; }
        set { }
    }

    public override int AoERange
    {
        get { return 0; }
        set { }
    }

    public override void Apply(Unit caster, Unit receiver){}

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid){}

    public override void Apply (Unit caster, List<Unit> receivers)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        foreach (var receiver in receivers)
        {
            int damage = Random.Range(MinDamage, MaxDamage+1);
            caster.DealDamage2(receiver, damage);
        }

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {

        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        foreach (var currentCell in cells)
        {
            if (currentCell.Occupent != null)
            {
                int damage = Random.Range(MinDamage, MaxDamage+1);
                caster.DealDamage2(currentCell.Occupent, damage);
            }
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
