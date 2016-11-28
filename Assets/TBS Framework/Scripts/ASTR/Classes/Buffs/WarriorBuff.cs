using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR.Classes.Buffs
{
    class WarriorBuff : Buff
    {
        public int Duration
        {
            get
            {
                return 0;
            }

            set
            {
            }
        }

        public bool isDot
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        public string Name
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        public string Tooltip
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        public void Apply(Unit unit)
        {
            unit.AttackFactor = 1;
            float Lifepercentage = (float)unit.HitPoints /unit.TotalHitPoints;
            unit.AttackFactor += Lifepercentage;
        }

        public Buff Clone()
        {
            return this;
        }

        public void Trigger(Unit unit)
        {
        }

        public void Undo(Unit unit)
        {
        }
    }
}
