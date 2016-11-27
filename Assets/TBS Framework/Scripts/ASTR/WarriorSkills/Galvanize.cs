using UnityEngine;
using System.Collections.Generic;

public class Galvanize : Skill
{

    public override string Name
    {
        get { return "Galvanize"; }
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

    public override void Apply (Unit caster, List<Unit> receivers)
    {
        foreach (var receiver in receivers)
        {
            int heal = Random.Range(MinDamage, MaxDamage+1);
            Debug.Log("Heal de la cible: " + heal);
            receiver.HitPoints += heal;
            if (receiver.HitPoints > receiver.TotalHitPoints)
                receiver.HitPoints = receiver.TotalHitPoints;
            // receiver.Buffs.Add(new DefenceBuff(2, 0.2f));
            //cellGrid.HealthbarUpdate(receiver, receiver.HitPoints, FullHealthbar);
        }

        int selfHeal = Random.Range(MinDamage, MaxDamage+1);
        Debug.Log("Heal du caster: " + selfHeal);
        caster.HitPoints += selfHeal;

        if (caster.HitPoints > caster.TotalHitPoints)
            caster.HitPoints = caster.TotalHitPoints;
        // caster.Buffs.Add(new DefenceBuff(2, 0.2f));
        //cellGrid.HealthbarUpdate(caster, caster.HitPoints, FullHealthbar);

        caster.ActionPoints--;
        SetCooldown();
    }

    public override void Apply (Unit caster, List<Cell> cells, CellGrid cellGrid){}
}
