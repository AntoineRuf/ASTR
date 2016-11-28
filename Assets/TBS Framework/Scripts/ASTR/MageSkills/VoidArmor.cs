using UnityEngine;
using System.Collections.Generic;

public class VoidArmor : Skill
{

    public override string Name
    {
        get { return "Void Armor"; }
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
        get { return 0; }
        set { }
    }

    public override int AoERange
    {
        get { return 0; }
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
            DefenceBuff ArmorVoidBuff = new DefenceBuff(2, 1.5f);
            receiver.Buffs.Add(ArmorVoidBuff);
            ArmorVoidBuff.Apply(receiver);
            
        }


        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {

        
    }
}
