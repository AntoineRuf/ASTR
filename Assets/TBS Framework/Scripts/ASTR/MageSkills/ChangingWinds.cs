using UnityEngine;
using System.Collections.Generic;

public class ChangingWinds : Skill
{

    public override string Name
    {
        get { return "Changing Winds"; }
        set { }
    }

    public override string Tooltip
    {
        get
        {
            return "Heals allies in a small radius around the caster";
        }

        set
        {
            base.Tooltip = value;
        }
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
        get { return 12; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 17; }
        set { base.MaxDamage = value; }
    }

    public override int Cooldown
    {
        get { return 4; }
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
        get { return 2; }
        set { }
    }

    // **TODO** Implémenter les dégâts supplémentaires au centre.

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellgrid)
    {
        Animator anim = caster.GetComponentInChildren<Animator>();
        anim.SetBool("Skill", true);
        anim.SetBool("Idle", false);
        
        foreach (var receiver in receivers)
        {
            int heal = Random.Range(MinDamage, MaxDamage + 1);
            receiver.HitPoints += heal;
            if (receiver.HitPoints > receiver.TotalHitPoints)
                receiver.HitPoints = receiver.TotalHitPoints;
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
            int randomReceiver = Random.Range(0, receivers.Count);
            int damage = Random.Range(MinDamage, MaxDamage + 1);
            caster.DealDamage2(receivers[randomReceiver], damage);
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
