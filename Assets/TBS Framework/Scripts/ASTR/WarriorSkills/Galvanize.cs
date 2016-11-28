using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Galvanize : Skill
{

    public override string Name
    {
        get { return "Galvanize"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Heals the Warrior and its target, and buffs their defense."; }
        set { }
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
        get { return 5; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 9; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 3; }
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

    // **TODO** Faire fonctionner le buff.

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Skill", true);
        anim.SetBool("Idle", false);

        GalvanizeBuff casterBuff = new GalvanizeBuff();
        GalvanizeBuff receiverBuff = new GalvanizeBuff();

        foreach (var receiver in receivers)
        {
            int heal = UnityEngine.Random.Range(MinDamage, MaxDamage+1);
            receiver.HitPoints += heal;
            if (receiver.HitPoints > receiver.TotalHitPoints)
                receiver.HitPoints = receiver.TotalHitPoints;
            receiverBuff.Apply(receiver);
            receiver.Buffs.Add(receiverBuff);
        }

        int selfHeal = UnityEngine.Random.Range(MinDamage, MaxDamage+1);
        caster.HitPoints += selfHeal;
        if (caster.HitPoints > caster.TotalHitPoints)
            caster.HitPoints = caster.TotalHitPoints;
        casterBuff.Apply(caster);
        caster.Buffs.Add(casterBuff);

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid){}
}
