using UnityEngine;
using System.Collections.Generic;

public class ShatteringForce : Skill
{

    public override string Name
    {
        get { return "Shattering Force"; }
        set { }
    }

    public override string Tooltip
    {
        get { return "Shatters the ground, immobilising any unit in front of the Warrior."; }
        set { }
    }

    public override int MinRange
    {
        get { return 1; }
        set { base.MinRange = value;  }
    }

    public override int MaxRange
    {
        get { return 1; }
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
        get { return 2; }
        set { }
    }

    public override int AoERange
    {
        get { return 4; }
        set { }
    }

    public override void Apply(Unit caster, Unit receiver)
    {
        // Comparer les positions du caster et du receiver
        // Rendre les 4 cases dans l'alignement impassables si elles sont passables (trap?)
        // Debuff chaque unité dans la zone (mouvement => O)
        // Stocker la référence des cellules affectées
        // Rendre aux cellules affectées leur état normal au début du prochain tour
    }

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid)
    {
        if (receiver.Occupent != null)
        {
          // Comparer les positions du caster et du receiver
          // Rendre les 4 cases dans l'alignement derrière receiver si passables (trap?)
          // Debuff chaque unité dans la zone (mouvement => O)
          // Stocker la référence des cellules affectées
          // Rendre aux cellules affectées leur état normal au début du prochain tour
        }
    }

    public override void Apply (Unit caster, List<Unit> receivers, CellGrid cellGrid)
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
                currentCell.Occupent.Buffs.Add(new RootedDebuff(1, currentCell.Occupent.TotalMovementPoints));
            }
            else
            {
                // **TODO** Voir comment implémenter les murs.
            }
        }

        caster.ActionPoints--;
        SetCooldown();
    }
}
