using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class QuickDash : Skill
    {
        public override string Name
        {
            get { return "Quick Dash"; }
            set { }
        }

        public override string Tooltip
        {
            get
            {
                return "Dash to a far location, dealing damage to anything in the unit's path. Dash range is fixed.";
            }

            set
            {
                base.Tooltip = value;
            }
        }

        public override int MinRange
        {
            get { return 5; }
            set { base.MinRange = value; }
        }

        public override int MaxRange
        {
            get { return 5; }
            set { base.MaxRange = value; }
        }

        public override int MinDamage
        {
            get { return 20; }
            set { base.MinDamage = value; }
        }

        public override int MaxDamage
        {
            get { return 25; }
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
            get { return true; }
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
                Vector3 casterCubeCoord = caster.ConvertToCube(caster.Cell.OffsetCoord);
                Vector3 cellCubeCoord = caster.ConvertToCube(currentCell.OffsetCoord);
                Vector3 direction = (cellCubeCoord - casterCubeCoord)/5;
                Vector3 CellAffectedCubeCoord = casterCubeCoord;
                Vector2 CellAffectedOffsetCoord = new Vector2();
                List<Cell> affectedCells = new List<Cell>();
                for( int i = 0; i<= 4; i++)
                {
                    CellAffectedCubeCoord += direction;
                    CellAffectedOffsetCoord = caster.ConvertToOffsetCoord(CellAffectedCubeCoord);
                    Cell cellToAdd = cellGrid.Cells.Find(c => c.OffsetCoord == CellAffectedOffsetCoord);
                    if (cellToAdd != null)
                    {
                        affectedCells.Add(cellToAdd);
                    }
                }
                foreach(Cell c in affectedCells)
                {
                    if (c.IsTaken)
                    {
                        int damage = UnityEngine.Random.Range(MinDamage, MaxDamage + 1);
                        caster.DealDamage2(c.Occupent, damage);
                    }
                }
                caster.Dash(currentCell, affectedCells, cellGrid.trapmanager);
            }

            caster.ActionPoints--;
            SetCooldown();
        }

    }
}
