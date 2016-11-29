using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TwinDaggers : Skill
{

    public override string Name
    {
        get { return "Twin Daggers"; }
        set { }
    }

    public override string Tooltip
    {
        get
        {
            return "Basic Rogue attack. Throw daggers to a cell. Deal more damage up close.";
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
        get { return true; }//testing purpose
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
        get { return 0; }
        set { }
    }

    public override int AoERange
    {
        get { return 0; }
        set { }
    }

    public override void Apply(Unit caster, Unit receiver){}

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid){}

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        foreach(Unit u in receivers)
        {
            if (caster.Cell.GetDistance(u.Cell) == 1)
            {
                int damage = UnityEngine.Random.Range(17, 21);
                caster.DealDamage2(u, damage);
            }
            else
            {
                int damage = UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
                caster.DealDamage2(u, damage);
            }
        }
        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {
    }
}
