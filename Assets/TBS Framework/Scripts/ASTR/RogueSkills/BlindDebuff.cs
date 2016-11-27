using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class BlindDebuff : Buff
    {
        public int Duration
        {
            get
            {
                return 2;
            }

            set
            {
                throw new NotImplementedException();
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
                return "Blinded";
            }

            set
            {
            }
        }

        public string Tooltip
        {
            get
            {
                return "This unit has a high chance of missing its next attack.";
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
