using System.Collections.Generic;
using UnityEngine;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class ShrapnelMine : Skill
    {
        public override string Name
        {
            get { return "Shrapnel Mine"; }
            set { }
        }

        public override string Tooltip
        {
            get
            {
                return "Primes an explosive device that deals 30 damage when triggered. Disappears after 3 turns.";
            }

            set
            {
                base.Tooltip = value;
            }
        }

        public override int MinRange
        {
            get { return 1; }
            set { base.MinRange = value; }
        }

        public override int MaxRange
        {
            get { return 3; }
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
            get { return true; }
            set { }
        }

        public override bool CanTargetEnemies
        {
            get { return false; }
            set { }
        }

        public override bool CanTargetAllies
        {
            get { return false; }
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

        public override void Apply(Unit caster, Unit receiver) { }

        public override void Apply(Unit caster, Cell receiver, CellGrid cellGrid) { }

        public override void Apply(Unit caster, List<Unit> receivers, CellGrid cellGrid)
        {
        }

        public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
        {
            foreach (var currentCell in cells)
            {
                Animator anim = caster.GetComponentInChildren<Animator>();
                anim.SetBool("Skill", true);
                anim.SetBool("Idle", false);
                ShrapnelMineTrap shrpmine = new ShrapnelMineTrap(currentCell, cellGrid.trapmanager, caster);
                cellGrid.trapmanager.AddTrap(shrpmine);
            }

            caster.ActionPoints--;
            SetCooldown();
        }
    }
}
