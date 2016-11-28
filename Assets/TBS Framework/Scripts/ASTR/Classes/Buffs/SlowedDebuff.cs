using System;

public class SlowedDebuff : Buff
{
    private float _factor;
    public string Tooltip {
        get { return "This hero is Slowed and can't move as much."; }
        set {}
    }

    public string Name {
        get { return "Slowed"; }
        set {}
    }
    
    public int Duration { get { return 1; } set { } }

    public bool isDot
    {
        get
        {
            return true;
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public void Apply(Unit unit)
    {
    }

    public void Undo(Unit unit)
    {
    }

    public Buff Clone()
    {
        return this;
    }

    public void Trigger(Unit unit)
    {
        unit.MovementPoints -= 1;
    }
}
