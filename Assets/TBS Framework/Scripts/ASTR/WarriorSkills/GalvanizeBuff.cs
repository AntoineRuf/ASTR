using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class GalvanizeBuff : Buff
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
            return "Galvanized";
        }

        set
        {
        }
    }

    public string Tooltip
    {
        get
        {
            return "This unit takes 20% less damage due to increased morale.";
        }

        set
        {
        }
    }

    public void Apply(Unit unit)
    {
        unit.DefenceFactor += 0.2f;
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
       unit.DefenceFactor -= 0.2f;
    }
}
