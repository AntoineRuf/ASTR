using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR.Classes.Buffs
{
    class StunDebuff : Buff
    {
        public int Duration
        {
            get
            {
                return 1;
            }

            set
            {
            }
        }

        public bool isDot
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        public string Name
        {
            get
            {
                return "Stunned";
            }

            set
            {
            }
        }

        public string Tooltip
        {
            get
            {
                return "This unit is disoriented. It won't do anything on its next turn.";
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
            unit.ActionPoints = 0;
            unit.MovementPoints = 0;
        }

        public void Undo(Unit unit)
        {
        }
    }
}
