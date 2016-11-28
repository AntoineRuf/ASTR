using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SecondWindBuff : Buff
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
            return "Steadfast";
        }

        set
        {
        }
    }

    public string Tooltip
    {
        get
        {
            return "This unit is immune to knockbacks and pulls.";
        }

        set
        {
        }
    }

    public void Apply(Unit unit)
    {
        unit.CCImmunity = 1;
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
        unit.CCImmunity = 0;
    }
}
