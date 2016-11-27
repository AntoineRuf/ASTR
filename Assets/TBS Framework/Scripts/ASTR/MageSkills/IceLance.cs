using UnityEngine;
using System.Collections.Generic;

public class FrozenLance : Skill
{

    public override string Name
    {
        get { return "Frozen Lance"; }
        set { }
    }

    public override int MinRange
    {
        get { return 1; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 4; }
        set { base.MaxRange = value; }
    }

    public override int MinDamage
    {
        get { return 27; }
        set { base.MinDamage = value; }
    }

    public override int MaxDamage
    {
        get { return 30; }
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
        get { return true; }//testing purpose
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
        get { return true; }
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

    // **TODO** Faire fonctionner le débuff.

    public override void Apply (Unit caster, List<Unit> receivers)
    {

        foreach (var receiver in receivers)
        {
            int damage = Random.Range(MinDamage, MaxDamage+1);
            caster.DealDamage2(receiver, damage);
            // receiver.Buffs.Add(new SlowedDebuff(1, 0.0f));
        }

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid)
    {
        foreach (var currentCell in cells)
        {
            if (currentCell.Occupent != null)
            {
                int damage = Random.Range(MinDamage, MaxDamage+1);
                caster.DealDamage2(currentCell.Occupent, damage);
                // receiver.Buffs.Add(new SlowedDebuff(1, 0.0f));
            }
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
