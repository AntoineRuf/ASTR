using UnityEngine;
using System.Collections.Generic;

public class Headvice : Skill
{

    public override string Name
    {
        get { return "Headvice"; }
        set { }
    }

    public override string Tooltip
    {
        get
        {
            return "Stun the target, dealing them lesser damage";
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
        get { return 5; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 7; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 6; }
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

    // **TODO** Implémenter les dégâts supplémentaires au centre.

    public override void Apply (Unit caster, List<Unit> receivers)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Skill", true);
        anim.SetBool("Idle", false);
        
        foreach (var receiver in receivers)
        {
            int damage = Random.Range(MinDamage, MaxDamage + 1);
            caster.DealDamage2(receiver, damage);
            StunDebuff StunDebuff = new StunDebuff(1, 0f);
            receiver.Buffs.Add(StunDebuff);
            StunDebuff.Apply(receiver);
        }


        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {

    }
}
