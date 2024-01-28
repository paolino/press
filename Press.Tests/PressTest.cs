namespace Press.Tests;

[TestFixture]
public class PressTest
{

    Tool tA = new(name: "A", width: 10);
    Tool tB = new(name: "B", width: 30);
    Tool tC = new(name: "C", width: 20);


    [Test]
    public void CanFindInEmptyPress()
    {
        Press press = new();
        int place = press.FindPlace(10);
        Assert.That (place == 0 , "There is space at 0 in an empty press");
    }

    [Test]
    public void CanFindInPressWithOneTool()
    {
        Press press = new();
        press.Insert(new Placed (tA,0));
        int place = press.FindPlace(10);
        Assert.That (place == 10 , "There is space at 10 in a press with a tool at 0");
    }

    [Test]
    public void CanFindSpaceBeforeThePresentTool()
    {
        Press press = new();
        press.Insert(new Placed (tA,10));
        int place = press.FindPlace(10);
        Assert.That (place == 0 , "There is space at 0 in a press with a tool at 10");
    }

    [Test]
    public void CanFindSpaceBetweenTwoTools()
    {
        Press press = new();
        press.Insert(new Placed (tA,0));
        press.Insert(new Placed (tA,20));
        int place = press.FindPlace(10);
        Assert.That (place == 10 , "There is space at 10 in a press with a tool at 0 and 20");
    }

    public void FoundSpaceIsLefJustifiedInAHole()
    {
        Press press = new();
        press.Insert(new Placed (tA,0));
        press.Insert(new Placed (tA,21));
        int place = press.FindPlace(10);
        Assert.That (place == 10 , "There is space at 10 in a press with a tool at 0 and 20");
    }
}