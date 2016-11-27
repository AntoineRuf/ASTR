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

    string Buff.Tooltip
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    string Buff.Name
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    int Buff.Duration
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    bool Buff.isDot
    {
        get
        {
            return false;
        }

        set
        {
            throw new NotImplementedException();
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

    void Buff.Apply(Unit unit)
    {
        throw new NotImplementedException();
    }

    void Buff.Undo(Unit unit)
    {
        throw new NotImplementedException();
    }

    void Buff.Trigger(Unit unit)
    {
    }

    Buff Buff.Clone()
    {
        throw new NotImplementedException();
    }
}
