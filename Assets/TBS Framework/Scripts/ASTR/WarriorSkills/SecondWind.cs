using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class SecondWind : Skill
{

    public override string Name
    {
        get { return "Second Wind"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Heals the warrior, and renders him immune to CC for 2 turns."; }
        set { }
    }

    public override int MinRange
    {
        get { return 0; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 0; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 0; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 0; }
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
        get { return false; }
        set { }
    }

    public override bool CanTargetAllies
    {
        get { return true; }
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

    public override void Apply(Unit caster, Unit receiver){}

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid){}

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        int selfHeal = (int)Mathf.Floor((caster.TotalHitPoints - caster.HitPoints) / 2);
        caster.HitPoints += selfHeal;
        if (caster.HitPoints > caster.TotalHitPoints)
            caster.HitPoints = caster.TotalHitPoints;
        SecondWindBuff Buff = new SecondWindBuff();
        caster.Buffs.Add(Buff);
        Buff.Apply(caster);

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid){}
}
