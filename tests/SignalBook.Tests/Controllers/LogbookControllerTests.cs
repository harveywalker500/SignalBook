using SignalBook.Library.Controllers;

namespace SignalBookTests.Controllers;

[TestClass]
public sealed class LogbookControllerTests
{
    public Boat[] TestBoats =
    [
        new Boat { Number = "U-1", RadioOperator = "A", ColourString = "Red" },
        new Boat { Number = "U-2", RadioOperator = "B", ColourString = "Green" },
        new Boat { Number = "U-3", RadioOperator = "C", ColourString = "Gold" },
        new Boat { Number = "U-4", RadioOperator = "D", ColourString = "CadetBlue" }
    ];
    LogbookController _logbookController = new LogbookController();

    [TestInitialize]
    public void Initialize()
    {
    }



    /* This is broken at the moment, but I'll keep it in for now
     [TestMethod]
    public void LoadConfig_CustomConfig()
    {
        _logbookController.LoadConfig("TestConfig.json");
        CollectionAssert.AreEqual(_logbookController.Boats, TestBoats);
    }
    */
}
