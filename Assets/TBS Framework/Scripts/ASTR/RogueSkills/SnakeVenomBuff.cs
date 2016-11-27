using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class SnakeVenomBuff : Buff
    {
        public int Duration
        {
            get
            {
                return 4;
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
                return "Snake Venom";
            }

            set
            {
            }
        }

        public string Tooltip
        {
            get
            {
                return "This unit will poison any unit it deals damage to.";
            }

            set
            {
            }
        }

        public void Apply(Unit unit)
        {
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
