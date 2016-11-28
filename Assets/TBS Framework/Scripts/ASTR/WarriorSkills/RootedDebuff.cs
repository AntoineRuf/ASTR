using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class RootedDebuff : Buff
{
    private int originalMovement;

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
            return "Rooted";
        }

        set
        {
        }
    }

    public string Tooltip
    {
        get
        {
            return "This unit can't move this turn.";
        }

        set
        {
        }
    }

    public void Apply(Unit unit)
    {
        originalMovement = unit.TotalMovementPoints;
        unit.TotalMovementPoints = 0;
        unit.MovementPoints = 0;
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
        unit.TotalMovementPoints = originalMovement;
        unit.MovementPoints = originalMovement;
    }
}
