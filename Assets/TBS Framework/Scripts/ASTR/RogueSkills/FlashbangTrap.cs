using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class FlashbangTrap : Trap
    {
        public override int Duration { get { return 4; } set { } }

        public override Cell Location { get; set; }

        public override TrapManager trapmanager { get; set; }

        public override Unit Owner { get; set; }

        public FlashbangTrap(Cell loc, TrapManager trman, Unit own)
        {
            Location = loc;
            trapmanager = trman;
            Owner = own;
        }

        public override void Apply(Unit unit)
        {
            List<Cell> affectedCells = new List<Cell>();
            affectedCells = trapmanager.cellGrid.Cells.FindAll(c => c.GetDistance(unit.Cell) <= 2);
            BlindDebuff bldbf = new BlindDebuff();
            foreach(Cell c in affectedCells)
            {
                if(c.IsTaken)
                {
                    c.Occupent.Buffs.Add(bldbf);
                }
            }
        }

        public override void Expires()
        {
            
        }

        public override Trap Clone()
        {
            return this;
        }
    }
}
