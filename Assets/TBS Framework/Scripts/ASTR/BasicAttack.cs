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
        caster.DealDamage(receiver);
        receiver.printDamage(caster.AttackFactor);
        caster.ActionPoints--;
    }

    public override  void Apply(Unit caster, Cell receiver, CellGrid cellGrid) { return; }
    public int Range { get; set; }
    public int Damage { get; set; }
    public int Cooldown { get; set; }
    public bool Targetable { get; set; }
}
