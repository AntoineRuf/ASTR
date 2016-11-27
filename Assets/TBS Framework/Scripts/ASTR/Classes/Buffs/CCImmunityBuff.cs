using System;

public class CCImmunityBuff : Buff
{
    private float _factor;
    public string Tooltip {
        get { return "This hero is immune to Crowd Control."; }
        set {}
    }

    public string Name {
        get { return "CCImmunityBuff"; }
        set {}
    }


    public CCImmunityBuff(int duration, float factor)
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
            throw new NotImplementedException();
        }
    }

    public void Apply(Unit unit)
    {
        unit.CCImmunity = 1;
    }

    public void Undo(Unit unit)
    {
        unit.CCImmunity = 0;
    }

    public Buff Clone()
    {
        return new CCImmunityBuff(Duration, _factor);
    }

    public void Trigger(Unit unit)
    {
    }
}
