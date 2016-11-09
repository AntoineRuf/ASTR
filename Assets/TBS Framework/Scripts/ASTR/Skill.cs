/// <summary>
/// Active Skills are used by units
/// </summary>
public abstract class Skill
{
    int Range { get; set; }
    int Damage { get; set; }
    int Cooldown { get; set; }
    public bool Targetable;

    public abstract void Apply(Unit caster, Unit receiver);

    public abstract void Apply(Unit caster, Cell receiver, CellGrid cellGrid);

}