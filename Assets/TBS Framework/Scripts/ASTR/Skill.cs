using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Active Skills are used by units, and are the bread and butter of combat.
/// </summary>
/// 
[Serializable]
public abstract class Skill
{

    public Skill()
    {
        CurrentCooldown = 0;
    }
    // The skill's name
    public virtual string Name { get; set; }

    // The skill's minimum casting range, in number of tiles.
    public virtual int MinRange { get; set; }

    // The skill's minimum casting range, in number of tiles.
    public virtual int MaxRange { get; set; }

    // The skill minimum damage, in HP points.
    public virtual int MinDamage { get; set; }

    // The skill maximum damage, in HP points.
    public virtual int MaxDamage { get; set; }

    // The skill cooldown time, in number of rounds.
    public virtual int Cooldown { get; set; }

    public virtual int CurrentCooldown { get; set; }

    // The Skill AoE Range, in number of tiles
    public virtual int AoERange { get; set; }

    public virtual string Tooltip { get; set; }

    // The skill targetting rules. In order:
    // - SightNeeded (does the caster need to see the target?)
    // - AlignmentNeeded (does the caster need to be aligned with the target?)
    // - CanTargetEmptyCell (can the skill be cast on an empty cell?)
    // - CanTargetSelf (can the skill be cast on the caster?)
    // - CanTargetAllies (can the skill be cast on allies?)
    // - CanTargetEnemies (can the skill be cast on enemies?)
    // - Is an AoE Spell ( 0 -> No, 1 -> Yes, 2 -> Yes, but in Line only)
    public virtual bool SightNeeded { get; set; }
    public virtual bool AlignmentNeeded { get; set; }
    public virtual bool CanTargetEmptyCell { get; set; }
    public virtual bool CanTargetSelf { get; set; }
    public virtual bool CanTargetAllies { get; set; }
    public virtual bool CanTargetEnemies { get; set; }
    /// <summary>
    /// Returns 0 if not, 1 if yes, 2 if yes but in line only
    /// </summary>
    public virtual int isAoE { get; set; }

    public virtual void Apply(Unit caster, Unit receiver) { }

    public virtual void Apply(Unit caster, List<Unit> receivers, CellGrid cellGrid) { }

    public virtual void Apply(Unit caster, List<Cell> receivers, CellGrid cellGrid) { }

    public virtual void Apply(Unit caster, Cell receiver, CellGrid cellGrid) { }

    public virtual void SetCooldown()
    {
        CurrentCooldown = Cooldown;
    }
}
