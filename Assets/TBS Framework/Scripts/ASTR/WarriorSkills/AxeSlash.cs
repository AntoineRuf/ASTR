using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class AxeSlash : Skill
{

    public override string Name
    {
        get { return "Axe Slash"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Basic attack. Slashes in front of the warrior."; }
        set { }
    }

    public override int MinRange
    {
        get { return 1; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 1; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 21; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 25; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 0; }
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
        get { return false; }
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

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        foreach (var receiver in receivers)
        {
            int damage = UnityEngine.Random.Range(MinDamage, MaxDamage+1);
            caster.DealDamage2(receiver, damage);
        }

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {
        
    }
}
