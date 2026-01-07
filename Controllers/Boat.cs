using System.Drawing;

namespace SignalBook.Controllers;

public class Boat
{
    public string Number { get; set; }
    public string Operator { get; set; }
    public string ColourString { get; set; }
    public Color Colour => Color.FromName(ColourString);
}