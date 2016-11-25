using UnityEngine;
using System.Collections.Generic;

public class ComeBackHere : Skill
{

    public override string Name
    {
        get { return "Come Back Here!"; }
        set { }
    }

    public override int MinRange
    {
        get { return 2; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 3; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 15; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 19; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 3; }
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
        get { return true; }
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

    // **TODO**
    // Comparer les positions de caster et receiver
    // Vérifier si receiver a le buff "insensible aux cc"
    // Déplacer receiver au contact de caster

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
