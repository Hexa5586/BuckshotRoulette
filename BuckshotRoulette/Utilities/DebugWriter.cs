using System.Diagnostics;

namespace BuckshotRoulette.Simplified.Utilities;

/// <summary>
/// A simple TextWriter adapter that redirects output to the Debug window.
/// </summary>
public class DebugWriter : TextWriter
{
    public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
    public override void Write(string? value) => Debug.Write(value);
}