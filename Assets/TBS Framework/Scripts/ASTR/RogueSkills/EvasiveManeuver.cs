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

        public override void Apply(Unit caster, List<Unit> receivers, CellGrid cellGrid)
        {
            foreach (Unit u in receivers)
            {
               int damage = Random.Range(MinDamage, MaxDamage + 1);
               caster.DealDamage2(u, damage);
                Vector3 casterCubeCoord = caster.ConvertToCube(caster.Cell.OffsetCoord);
                Vector3 receiverCubeCoord = u.ConvertToCube(u.Cell.OffsetCoord);
                Vector3 directiontogo = -(Directions.NearestNeighborDirection(casterCubeCoord, receiverCubeCoord));
                Vector3 displacement = new Vector3();
                List<Cell> path = new List<Cell>();
                for(int i = 1; i >=3; i++)
                {
                    if(cellGrid.Cells.Find(c => c.OffsetCoord == Directions.ConvertToOffsetCoord(casterCubeCoord + directiontogo)) != null)
                    {
                        Cell celltoExamine = cellGrid.Cells.Find(c => c.OffsetCoord == Directions.ConvertToOffsetCoord(casterCubeCoord + directiontogo));
                        if (celltoExamine.IsTaken)
                        {
                            break;
                        }
                        else
                        {
                            displacement += directiontogo;
                        }
                    }
                    else
                    {
                        break;
                    }
                }


            }
            caster.ActionPoints--;
            SetCooldown();

        }

        public override void Apply(Unit caster, List<Cell> cells, CellGrid cellGrid)
        {
          
        }
    }
}

