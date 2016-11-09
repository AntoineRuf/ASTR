using UnityEngine;
using System.Collections;
using System;

namespace Assets.TBS_Framework.Scripts.ASTR
{
    public class AttackBuff : Buff
    {
        public int Duration { get { return Duration; } set { Duration = -1; } }

        public void Apply(Unit unit)
        {
            unit.AttackFactor *= 2;
        }

        public Buff Clone()
        {
            return this;
        }

        public void Undo(Unit unit)
        {
            unit.AttackFactor /= 2;
        }
    }
}
