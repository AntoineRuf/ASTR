public class AttackBuff : Buff
{
    private float _factor;
    public string Tooltip {
        get { return "This hero has increased attack and will deal more damage."; }
        set {}
    }

    public string Name {
        get { return "AttackBuff"; }
        set {}
    }


    public AttackBuff(int duration, float factor)
    {
        Duration = duration;
        _factor = factor;
    }

    public int Duration { get; set; }
    public void Apply(Unit unit)
    {
        unit.AttackFactor += _factor;
    }

    public void Undo(Unit unit)
    {
        unit.AttackFactor -= _factor;
    }

    public Buff Clone()
    {
        return new AttackBuff(Duration, _factor);
    }
}
