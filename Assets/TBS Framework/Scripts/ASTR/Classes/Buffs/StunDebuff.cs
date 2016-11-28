public class StunDebuff : Buff
{
    private float _factor;
    private int _initialMovementPoints;
    public string Tooltip {
        get { return "This hero is stuned and cannot move or perform attacks."; }
        set {}
    }

    public string Name {
        get { return "Stun Debuff"; }
        set {}
    }


    public StunDebuff(int duration, float factor)
    {
        Duration = duration;
        _factor = 0.0f;
    }

    public int Duration { get; set; }
    public void Apply(Unit unit)
    {
        _initialMovementPoints = unit.TotalMovementPoints;
        unit.ActionPoints = 0;
        unit.TotalMovementPoints = 0;
    }

    public void Undo(Unit unit)
    {
        unit.TotalMovementPoints = _initialMovementPoints;
        unit.ActionPoints = 1;
    }

    public Buff Clone()
    {
        return new SlowedDebuff(Duration, _factor);
    }
}
