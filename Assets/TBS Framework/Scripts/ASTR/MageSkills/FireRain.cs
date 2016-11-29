using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class FireRain : Skill
{

    public override string Name
    {
        get { return "Fire Rain"; }
        set { }
    }
    public override string Tooltip
    {
        get
        {
            return "Casts 3 fire missiles that hit random units in the targeted area";
        }

        set
        {
            base.Tooltip = value;
        }
    }
    public override int MinRange
    {
        get { return 2; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 7; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 7; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 9; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 5; }
        set { }
    }
    

    public override bool CanTargetEmptyCell
    {
        get { return true; }
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
        get { return 2; }
        set { }
    }

    // **TODO** Implémenter les dégâts supplémentaires au centre.

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellgrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Skill", true);
        anim.SetBool("Idle", false);

        for (int i = 0; i < 3; ++i)
        {
            int randomReceiver = UnityEngine.Random.Range(0, receivers.Count);
            int damage = UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
            caster.DealDamage2(receivers[randomReceiver], damage);
        }


        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {

        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Skill", true);
        anim.SetBool("Idle", false);

        List<Unit> receivers = new List<Unit>();
        foreach (var currentCell in cells)
        {
            if (currentCell.Occupent != null)
            {
                receivers.Add(currentCell.Occupent);
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            int randomReceiver = UnityEngine.Random.Range(0, receivers.Count);
            int damage = UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
            caster.DealDamage2(receivers[randomReceiver], damage);
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
