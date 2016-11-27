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
    /// <summary>
    /// Boolean to check if the buff is a DoT. If yes, the method Trigger is used to apply its effects each turn.
    bool isDot { get; set; }
    /// </summary>
    /// <summary>
    /// Describes how the unit should be upgraded.
    /// </summary>
    void Apply(Unit unit);
    /// <summary>
    /// Returns units stats to normal.
    /// </summary>
    void Undo(Unit unit);
    ///<summary>
    /// Apply effects of the buff over time.
    /// </summary>
    void Trigger(Unit unit);
    /// <summary>
    /// Returns deep copy of the object.
    /// </summary>
    Buff Clone();
}