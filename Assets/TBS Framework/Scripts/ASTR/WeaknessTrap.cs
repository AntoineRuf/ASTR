using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR
{
    class WeaknessTrap : Trap
    {
        public WeaknessTrap(Cell cell, TrapManager trmanager, Unit unit)
        {
            Duration = 3;
            Location = cell;
            trapmanager = trmanager;
            Owner = unit;
        }

        
        public override void Apply(Unit unit)
        {
            unit.AttackFactor /= 2;
            unit.TrapNotice("Weakness Trap !");
        }

    }
}
