using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR.RogueSkills
{
    class LethalToxinBuff : Buff
    {
        public int Duration
        {
            get
            {
                return 3;
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
                return "Lethal Toxin";
            }

            set
            {
            }
        }

        public string Tooltip
        {
            get
            {
                return "This unit is using a deadly toxin. It does more damage and can critically strike if backstabbing.";
            }

            set
            {
            }
        }

        public void Apply(Unit unit)
        {
            unit.AttackFactor += 0.2f;
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
            unit.AttackFactor -= 0.2f;
        }
    }
}
