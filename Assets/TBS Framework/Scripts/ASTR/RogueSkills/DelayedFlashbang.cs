using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class DelayedFlashbang : Skill
    {
        public override string Name
        {
            get { return "Delayed Flashbang"; }
            set { }
        }

        public override string Tooltip
        {
            get
            {
                return "Primes a phosphore explosive on target cell. Once triggered, blinds all units around it. Detonates after 2 turns.";
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
            get { return 2; }
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
            get { return 6; }
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

        public override void Apply(Unit caster, List<Unit> receivers)
        {
        }

        public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
        {
            foreach (var currentCell in cells)
            {
                FlashbangTrap flshtrp = new FlashbangTrap(currentCell, cellGrid.trapmanager, caster);
                cellGrid.trapmanager.AddTrap(flshtrp);
            }

            caster.ActionPoints--;
            SetCooldown();
        }
    }
}

