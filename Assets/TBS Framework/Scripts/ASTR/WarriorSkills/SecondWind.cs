using UnityEngine;
using System.Collections.Generic;

public class SecondWind : Skill
{

    public override string Name
    {
        get { return "Second Wind"; }
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
        get { return 4; }
        set { }
    }

    public override int CurrentCooldown
    {
        get { return 0; }
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

    public override void Apply (Unit caster, List<Unit> receivers)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        int selfHeal = (int)Mathf.Floor((caster.TotalHitPoints - caster.HitPoints) / 2);
        caster.HitPoints += selfHeal;
        if (caster.HitPoints > caster.TotalHitPoints)
            caster.HitPoints = caster.TotalHitPoints;
        // **TODO** Faire fonctionner le buff d'immunité.
        // caster.Buffs.Add(new CCImmunityBuff(1, 0.0f));

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid){}
}
