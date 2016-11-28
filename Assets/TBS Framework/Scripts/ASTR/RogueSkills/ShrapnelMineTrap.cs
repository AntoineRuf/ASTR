using Assets.TBS_Framework.Scripts.ASTR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
    class ShrapnelMineTrap : Trap
    {
        public override int Duration { get { return 4; } set { } }

        public override Cell Location { get; set; }

        public override TrapManager trapmanager { get; set; }

        public override Unit Owner { get; set; }

        public ShrapnelMineTrap(Cell loc, TrapManager trman, Unit own)
        {
            Location = loc;
            trapmanager = trman;
            Owner = own;
        }

        public override void Apply(Unit unit)
        {
            unit.DealDamage2(unit, 30);
        }

        public override void Expires()
        { }

        public override Trap Clone()
        {
            return this;
        }
    }