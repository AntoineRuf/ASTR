using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
    class FanOfKnives : Skill
    {
        public override string Name
        {
            get { return "Fan of Knives"; }
            set { }
        }

        public override string Tooltip
        {
            get
            {
                return "Sends a flurry of knives on three target cells. Units hit take more damage from all sources for a short time.";
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
            get { return 10; }
            set { base.MinDamage = value; }
        }

        public override int MaxDamage
        {
            get { return 15; }
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
            get { return 3; }
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
            foreach (Unit u in receivers)
            {
                Animator anim = caster.GetComponentInChildren<Animator>();
                anim.SetBool("Attack", true);
                int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
                caster.DealDamage2(u, damage);
                VulnerabilityDebuff vulnebuff = new VulnerabilityDebuff();
                u.Buffs.Add(vulnebuff);
                vulnebuff.Apply(u);
            }
            caster.ActionPoints--;
            SetCooldown();
        }

        public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
        {
            Animator anim = caster.GetComponentInChildren<Animator>();
            anim.SetBool("Attack", true);
            anim.SetBool("Idle", false);
            foreach (var currentCell in cells)
            {
                if (currentCell.Occupent != null)
                {
                    int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
                    VulnerabilityDebuff vulnebuff = new VulnerabilityDebuff();
                    currentCell.Occupent.Buffs.Add(vulnebuff);
                    vulnebuff.Apply(currentCell.Occupent);
                }
            }

            caster.ActionPoints--;
            SetCooldown();
        }
    }

