using System.Collections.Generic;
using UnityEngine;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class EvasiveManeuver : Skill
    {
        public override string Name
        {
            get { return "Evasive Maneuvers"; }
            set { }
        }

        public override string Tooltip
        {
            get
            {
                return "Dashes on a target, deals damage, and use it as a foothold to jump away.";
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
            get { return 18; }
            set { base.MinDamage = value; }
        }

        public override int MaxDamage
        {
            get { return 25; }
            set { base.MaxDamage = value; }
        }

        public override int Cooldown
        {
            get { return 4; }
            set { }
        }

        public override bool CanTargetEmptyCell
        {
            get { return false; }
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
            foreach (Unit u in receivers)
            {
               int damage = Random.Range(MinDamage, MaxDamage + 1);
               caster.DealDamage2(u, damage);
                Vector3 casterCubeCoord = caster.ConvertToCube(caster.Cell.OffsetCoord);
                Vector3 receiverCubeCoord = u.ConvertToCube(u.Cell.OffsetCoord);
                Vector3 direction = receiverCubeCoord - casterCubeCoord;
                Vector3 up = new Vector3(0, 1, -1);
                Vector3 down = new Vector3(0, -1, 1);
                Vector3 upR = new Vector3(+1, 0, -1);
                Vector3 upL = new Vector3(-1, 1, 0);
                Vector3 downR = new Vector3(+1, -1, 0);
                Vector3 downL = new Vector3(-1, 0, 1);
                Vector2 displacement = new Vector2();
                if(direction == up)
                {
                    displacement = caster.ConvertToOffsetCoord(3 * down);
                }
                else if(direction == down)
                {
                    displacement = caster.ConvertToOffsetCoord(3 * up);
                }
                else if (direction == upR)
                {
                    displacement = caster.ConvertToOffsetCoord(3 * downL);
                }
                else if (direction == upL)
                {
                    displacement = caster.ConvertToOffsetCoord(3 * downR);
                }
                else if (direction == downL)
                {
                    displacement = caster.ConvertToOffsetCoord(3 * upR);
                }
                else if (direction == downR)
                {
                    displacement = caster.ConvertToOffsetCoord(3 * upL);
                }
                caster.Cell.OffsetCoord += displacement;
                caster.transform.position = caster.Cell.transform.position;
            }
            caster.ActionPoints--;
            SetCooldown();

        }

        public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
        {
            foreach (var currentCell in cells)
            {
                if (currentCell.Occupent != null)
                {
                    if (caster.isBehind(currentCell.Occupent))
                    {
                        int damage = Random.Range(MinDamage, MaxDamage + 1);
                        caster.DealDamage2(currentCell.Occupent, damage);
                    }
                    else
                    {
                        int damage = 0;
                        caster.DealDamage2(currentCell.Occupent, damage);
                    }
                }
            }

            caster.ActionPoints--;
            SetCooldown();
        }
    }
}

