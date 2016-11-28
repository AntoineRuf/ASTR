using System;

public class RootedDebuff : Buff
{
    private float _factor;
    public string Tooltip {
        get { return "This hero is rooted and can't move."; }
        set {}
    }

    public string Name {
        get { return "RootedDebuff"; }
        set {}
    }


    public RootedDebuff(int duration, float factor)
    {
        Duration = duration;
        _factor = factor;
    }

    public int Duration { get; set; }

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

    public void Apply(Unit unit)
    {
        unit.TotalMovementPoints = 0;
    }

    public void Undo(Unit unit)
    {
        unit.TotalMovementPoints = (int)_factor;
    }

    public Buff Clone()
    {
        return new RootedDebuff(Duration, _factor);
    }

    public void Trigger(Unit unit)
    {
    }
}
