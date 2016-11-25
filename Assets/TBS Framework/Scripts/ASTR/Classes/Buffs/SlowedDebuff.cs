public class SlowedDebuff : Buff
{
    private float _factor;
    public string Tooltip {
        get { return "This hero is Slowed and can't move as much."; }
        set {}
    }

    public string Name {
        get { return "SlowedDebuff"; }
        set {}
    }


    public SlowedDebuff(int duration, float factor)
    {
        Duration = duration;
        _factor = 0.0f;
    }

    public int Duration { get; set; }
    public void Apply(Unit unit)
    {
        if (unit.TotalMovementPoints >= 1)
        {
            unit.TotalMovementPoints -= 1;
            _factor = 1.0f;
        }
    }

    public void Undo(Unit unit)
    {
        if (_factor == 1.0f) unit.TotalMovementPoints += 1;
    }

    public Buff Clone()
    {
        return new SlowedDebuff(Duration, _factor);
    }
}
