namespace BuckshotRoulette.Simplified.Contexts;

/// <summary>
/// Represents the possible states of a bullet in the magazine.
/// </summary>
public enum BulletType
{
    Real,
    Blank,
    Unknown
}

/// <summary>
/// Provides helper methods for the BulletType enum to mimic Java-style enum methods.
/// </summary>
public static class BulletTypeExtensions
{
    /// <summary>
    /// Returns the display name of the bullet type.
    /// </summary>
    public static string GetName(this BulletType bullet) => bullet switch
    {
        BulletType.Real => "Real",
        BulletType.Blank => "Blank",
        BulletType.Unknown => "Unknown",
        _ => throw new ArgumentOutOfRangeException(nameof(bullet))
    };

    /// <summary>
    /// Returns the ASCII representation for UI rendering.
    /// </summary>
    public static string GetChar(this BulletType bullet) => bullet switch
    {
        BulletType.Real => "●",
        BulletType.Blank => "○",
        BulletType.Unknown => "-",
        _ => throw new ArgumentOutOfRangeException(nameof(bullet))
    };
}