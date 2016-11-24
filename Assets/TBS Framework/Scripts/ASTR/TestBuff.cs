public class TestBuff : Buff
{
    private int _factor;

    public string Tooltip
    {
        get
        {
            return "Target get seuf protection";
        }
        set
        {

        }
    }

    public string Name
    {
        get
        {
            return "TestBuff";
        }
        set
        {

        }
    }


    public TestBuff(int duration, int factor)
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