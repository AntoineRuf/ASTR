using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class LethalToxin : Skill
    {
        public override string Name
        {
            get { return "Lethal Toxin"; }
            set { }
        }

        public override string Tooltip
        {
            get
            {
                return "Coats the user's weapon with a dangerous neurotoxin. Boost damage, as well as critical chance when backstabbing.";
            }

            set
            {
                base.Tooltip = value;
            }
        }

        public override int MinRange
        {
            get { return 0; }
            set { base.MinRange = value; }
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
            get { return 8; }
            set { }
        }

        public override bool CanTargetEmptyCell
        {
            get { return false; }//testing purpose
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
            get { return 1; }
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
            Animator anim = caster.GetComponentInChildren<Animator>();
            anim.SetBool("Skill", true);
            anim.SetBool("Idle", false);
            LethalToxinBuff lthbuff = new LethalToxinBuff();
            lthbuff.Apply(caster);
            caster.Buffs.Add(lthbuff);
            caster.ActionPoints--;
            SetCooldown();
        }

        public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
        {
        }
    }
}
