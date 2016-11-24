using UnityEngine;
using System.Collections;

public class BasicAttack : Skill {

    public override void Apply(Unit caster, Unit receiver)
    {
        if(caster.Equals(receiver))
        {
            return;
        }
        Debug.Log("CRITICAL INT : " + caster.FacingComparison(receiver));
        caster.GetComponentInChildren<Animator>().SetBool("Attack", true);
        caster.DealDamage(receiver);
        receiver.printDamage(caster.AttackFactor);
        caster.ActionPoints--;

    }

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid) { return; }
}
