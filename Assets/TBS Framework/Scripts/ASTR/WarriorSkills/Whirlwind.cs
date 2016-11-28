using UnityEngine;
using System.Collections.Generic;

public class Whirlwind : Skill
{

    public override string Name
    {
        get { return "Whirlwind"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Spin in a circle, hitting anyone around."; }
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
        get { return 2; }
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
        get { return 1; }
        set { }
    }

    public override void Apply(Unit caster, Unit receiver){}

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid){}

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
    {
        Debug.Log("Wololo");

        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Attack", true);
        anim.SetBool("Idle", false);

        float damageBonus = 0.2f * (receivers.Count - 1);
        Debug.Log(damageBonus);
        caster.AttackFactor += damageBonus;
        foreach (var receiver in receivers)
        {
            if (!caster.Equals(receiver)) {
                int damage = Random.Range(MinDamage, MaxDamage+1);
                caster.DealDamage2(receiver, damage);
            }
        }
        caster.AttackFactor -= damageBonus;

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid){
    }
}
