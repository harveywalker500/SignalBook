using System.Drawing;
using System.Text.Json.Serialization;

namespace SignalBook.Controllers;

/// <summary>
/// A record class for a single boat.
/// </summary>
/// <remarks>
/// Each boat has an individual number, a radio operator's name and a colour.
/// Colour is defined from the JSON as a string, but converted to a System.Drawing.Color for use in the application.
/// </remarks>
public record Boat
{
    /// <summary>
    /// Number of the boat.
    /// </summary>
    public string Number { get; set; }
    /// <summary>
    /// The name of the radio operator for the boat.
    /// </summary>
    public string RadioOperator { get; set; }
    /// <summary>
    /// The string representation of the boat's colour.
    /// </summary>
    public string ColourString { get; set; }

    /// <summary>
    /// The System.Drawing.Color representation of the boat's colour used for logging.
    /// </summary>
    [JsonIgnore]
    public Color Colour => Color.FromName(ColourString);

    /// <summary>
    /// Override of ToString for easy logging.
    /// </summary>
    /// <returns>A string representation of the boat class.</returns>
    public override string ToString() => $"{Number}\nRadio Operator: {RadioOperator}\nColour: {Colour.Name}\n";
}