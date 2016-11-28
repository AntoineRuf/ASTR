/// <summary>
/// Buff represents an "upgrade" to a unit.
/// </summary>
public interface Buff
{
    string Tooltip { get; set; }
    string Name { get; set; }
    /// <summary>
    /// Determines how long the buff should last (expressed in turns). If set to negative number, buff will be permanent.
    /// </summary>
    int Duration { get; set; }
    bool isDot { get; set; }
    /// <summary>
    /// Describes how the unit should be upgraded.
    /// </summary>
    void Apply(Unit unit);
    /// <summary>
    /// Returns units stats to normal.
    /// </summary>
    void Undo(Unit unit);
    void Trigger(Unit unit);

    /// <summary>
    /// Returns deep copy of the object.
    /// </summary>
    Buff Clone();
}