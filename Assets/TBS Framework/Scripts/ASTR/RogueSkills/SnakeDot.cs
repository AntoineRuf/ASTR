using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
class SnakeDot : Buff
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
                return "Poisoned";
            }

            set
            {
            }
        }

        public string Tooltip
        {
            get
            {
                return "This unit takes damage from a virulent poison each turn.";
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
            int damage = UnityEngine.Random.Range(4, 6);
            unit.DealDamage2(unit, damage);
        }

        public void Undo(Unit unit)
        {
        }
    }
