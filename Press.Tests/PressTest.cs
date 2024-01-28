namespace Press.Tests;

[TestFixture]
public class PressTestFindPLace
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

[TestFixture]
public class PressTestInsertWherePossible
{

    Tool tA = new(name: "A", width: 10);
    Tool tB = new(name: "B", width: 30);
    Tool tC = new(name: "C", width: 20);
    [Test]
    public void CanInsertInEmptyPress()
    {
        Press press = new();
        int place = press.InsertWherePossible(tA);
        Assert.That(place == 0, "There is space at 0 in an empty press");
        Assert.That(press.Tools.Count == 1, "There is one tool in the press");
        Assert.That(press.Tools.FindIndex(t => t.Name == "A") == 0, "The tool is at index 0");
    }

    [Test]
    public void CanInsertInPressWithOneTool()
    {
        Press press = new();
        press.Insert(new Placed (tA,0));
        int place = press.InsertWherePossible(tB);
        Assert.That(place == 10, "There is space at 10 in a press with a tool at 0");
        Assert.That(press.Tools.Count == 2, "There are two tools in the press");
        Assert.That(press.Tools.FindIndex(t => t.Name == "B") == 1, "The tool is at index 1");
        Assert.That(press.Tools[press.Tools.FindIndex(t => t.Name == "B")].Position == 10, "The tool position is 10");
    }

    [Test]
    public void CanInsertInPressBetweenTwoTools()
    {
        Press press = new();
        press.Insert(new Placed (tA,0));
        press.Insert(new Placed (tA,40));
        int place = press.InsertWherePossible(tB);
        Assert.That(place == 10, "There is space at 10 in a press with a tool at 0 and 20");
        Assert.That(press.Tools.Count == 3, "There are three tools in the press");
        Assert.That(press.Tools.FindIndex(t => t.Name == "B") == 1, "The tool is at index 1");
        Assert.That(press.Tools[press.Tools.FindIndex(t => t.Name == "B")].Position == 10, "The tool position is 10");
    }

    [Test]
    public void CanInsertInPressAtPosition0()
    {
        Press press = new();
        press.Insert(new Placed (tA,30));
        int place = press.InsertWherePossible(tB);
        Assert.That(place == 0, "There is space at 0 in a press with a tool at 30");
        Assert.That(press.Tools.Count == 2, "There are three tools in the press");
        Assert.That(press.Tools.FindIndex(t => t.Name == "B") == 0, "The tool is at index 0");
        Assert.That(press.Tools[press.Tools.FindIndex(t => t.Name == "B")].Position == 0, "The tool position is 0");
    }
}

[TestFixture]
public class PressTestExportRecipe
{

        Tool tA = new(name: "A", width: 10);
        Tool tB = new(name: "B", width: 30);
        Tool tC = new(name: "C", width: 20);
        [Test]
        public void CanExportEmptyPress()
        {
            Press press = new();
            Recipe recipe = press.ExportRecipe();
            Assert.That(recipe.Items.Count == 0, "The recipe is empty");
        }

        [Test]
        public void CanExportPressWithOneTool()
        {
            Press press = new();
            press.Insert(new Placed (tA,0));
            Recipe recipe = press.ExportRecipe();
            Assert.That(recipe.Items.Count == 1, "The recipe has one item");
            Assert.That(recipe.Items[0].Name == "A", "The recipe has one item named A");
            Assert.That(recipe.Items[0].Position == 0, "The recipe has one item at position 0");
        }

        [Test]
        public void CanExportPressWithTwoTools()
        {
            Press press = new();
            press.Insert(new Placed (tA,10));
            press.Insert(new Placed (tA,0));
            Recipe recipe = press.ExportRecipe();
            Assert.That(recipe.Items.Count == 2, "The recipe has two items");
            Assert.That(recipe.Items[0].Name == "A", "The recipe has one item named A");
            Assert.That(recipe.Items[0].Position == 0, "The recipe has one item at position 0");
            Assert.That(recipe.Items[1].Name == "A", "The recipe has one item named A");
            Assert.That(recipe.Items[1].Position == 10, "The recipe has one item at position 10");
        }

    [Test]
    public void CanExportPressWithTwoToolsOfDifferentWidth()
    {
        Press press = new();
        press.Insert(new Placed(tA, 0));
        press.Insert(new Placed(tB, 15));
        Recipe recipe = press.ExportRecipe();
        Assert.That(recipe.Items.Count == 2, "The recipe has two items");
        Assert.That(recipe.Items[0].Name == "A", "The recipe has one item named A");
        Assert.That(recipe.Items[0].Position == 0, "The recipe has one item at position 0");
        Assert.That(recipe.Items[1].Name == "B", "The recipe has one item named B");
        Assert.That(recipe.Items[1].Position == 15, "The recipe has one item at position 10");
    }
}
