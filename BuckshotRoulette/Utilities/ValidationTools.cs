namespace BuckshotRoulette.Simplified.Utilities;

public static class ValidationTools
{
    public static int EnsureNonNegative(int value) =>
        value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");

    public static double ValidateProbability(double value) =>
        (value >= 0 && value <= 1) ? value : throw new ArgumentOutOfRangeException(nameof(value), "Probability must be between 0 and 1.");

}
