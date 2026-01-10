using System.Drawing;
using System.Text.Json.Serialization;

namespace SignalBook.Controllers;

public record Boat
{
    public string Number { get; set; }
    public string RadioOperator { get; set; }
    public string ColourString { get; set; }

    [JsonIgnore]
    public Color Colour => Color.FromName(ColourString);
}