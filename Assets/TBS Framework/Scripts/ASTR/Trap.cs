using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TBS_Framework.Scripts.ASTR
{
    public abstract class Trap
    {
        public virtual int Duration { get; set; }

        public virtual Cell Location { get; set; }

        public virtual TrapManager trapmanager { get; set; }

        public virtual Unit Owner { get; set; }

        public virtual void Apply(Unit unit)
        { }

        public virtual void Expires()
        { }

        public virtual Trap Clone()
        {
            return this;
        }
    }
}
