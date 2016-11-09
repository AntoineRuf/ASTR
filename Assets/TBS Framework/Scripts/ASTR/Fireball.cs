using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireball : Skill
{
    
    public override void Apply(Unit caster, Unit receiver)
    {
        if (caster.Equals(receiver))
        {
            return;
        }
        caster.DealDamage(receiver);
        receiver.printDamage(caster.AttackFactor);
        caster.ActionPoints--;
    }

    public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid)
    {
        
        List<Cell> AoE = new List<Cell>();
        Debug.Log("AoE : " + AoE.Count);
        AoE = receiver.GetNeighbours(cellGrid.Cells);
        AoE.Add(receiver);
        Debug.Log("AoE : " + AoE.Count);
        for(int i = 0; i < AoE.Count; ++i)
        {
            if (AoE[i].Occupent != null)
            {
                Debug.Log("FIREBAAAAAAAALL : "+ i);
                caster.DealDamage2(AoE[i].Occupent);
                AoE[i].Occupent.printDamage(caster.AttackFactor);
            }
        }
        caster.ActionPoints--;
    }

    public int Range { get; set; }
    public int Damage { get; set; }
    public int Cooldown { get; set; }
}
