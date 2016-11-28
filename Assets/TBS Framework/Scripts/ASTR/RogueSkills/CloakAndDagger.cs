using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
class CloakAndDagger : Skill
{
    public override string Name
    {
        get { return "Backstab"; }
        set { }
    }

    public override string Tooltip
    {
        get
        {
            return "Inflicts serious damage to a single target. Is only effective if backstabbing.";
        }

        set
        {
            base.Tooltip = value;
        }
    }

    public override int MinRange
    {
        get { return 1; }
        set { base.MinRange = value; }
    }

    public override int MaxRange
    {
        get { return 1; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 28; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 35; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 5; }
        set { }
    }

    public override bool CanTargetEmptyCell
    {
        get { return false; }
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
        get { return 0; }
        set { }
    }

    public override int AoERange
    {
        get { return 0; }
        set { }
    }

    public override void Apply(Unit caster, Unit receiver) { }

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid) { }

    public override void Apply(Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);
        foreach (Unit u in receivers)
        {
            if (caster.isBehind(u))
            {
                int damage = UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
                caster.DealDamage2(u, damage);
            }
            else
            {
                int damage = 0;
                caster.DealDamage2(u, damage);
            }
        }
        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
    {
    }
}
