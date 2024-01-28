namespace Press.Tests;

[TestFixture]
public class MachineTest
{

    Tool tA = new(name: "A", width: 10);
    Tool tB = new(name: "B", width: 30);
    Tool tC = new(name: "C", width: 20);


    [Test]
    public void TestNoProblem()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 2)
            ];
        List<RecipeItem> recipeItems =
            [ new("A", 0)
            , new("A", 10)
            ];
        Recipe recipe = new(recipeItems);
        Machine machine = new(machineStorage);
        Problems loadResult = machine.LoadRecipe(recipe);
        Assert.That
            (Problems.NoProblem == loadResult
            , "The recipe should be loaded");
    }

    [Test]
    public void AssertNotEnoughTools()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 1)
            ];
        List<RecipeItem> recipeItems =
            [ new("A", 0)
            , new("A", 0)
            ];
        Recipe recipe = new(recipeItems);
        Machine machine = new(machineStorage);
        Problems loadResult = machine.LoadRecipe(recipe);
        Assert.That
            (loadResult == Problems.NotEnoughTools
            , "The recipe should not be loaded as there are not enough tools");
    }
    [Test]
    public void AssertOverlappingTools()
    {

        List<ToolStorage> machineStorage =
            [
            new(tA, 2),
            ];
        List<RecipeItem> recipeItems =
            [
            new("A", 0),
            new("A", 9),
            ];
        Recipe recipe = new(recipeItems);
        Machine machine = new(machineStorage);
        Problems loadResult = machine.LoadRecipe(recipe);
        Assert.That
            (loadResult == Problems.OverlappingTools
            , "The recipe should not be loaded as there are overlapping tools");
    }

    [Test]
    public void ClearMachineRestoreTheStorage()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 2)
            ];
        List<RecipeItem> recipeItems =
            [ new("A", 0)
            , new("A", 10)
            ];
        Recipe recipe = new(recipeItems);
        Machine machine = new(machineStorage);
        Problems loadResult = machine.LoadRecipe(recipe);
        Assert.That
            (Problems.NoProblem == loadResult
            , "The recipe should be loaded");
        machine.Clear();
        Assert.That
            (machineStorage.SequenceEqual(machine.Storage.ToolStorages())
            , "The storage should be restored");
    }

    [Test]
    public void AssertPressIsOrdered()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 2)
            ];
        List<RecipeItem> recipeItems =
            [ new("A", 10)
            , new("A", 0)
            ];
        Recipe recipe = new(recipeItems);
        Machine machine = new(machineStorage);
        Problems loadResult = machine.LoadRecipe(recipe);
        Assert.That
            (Problems.NoProblem == loadResult
            , "The recipe should be loaded");
        Assert.That
            (machine.Press.Tools.SequenceEqual
                ([
                new(tA, 0),
                new(tA, 10),
                ])
            , "The press should be ordered");
    }
}

[TestFixture]
public class MachineTestsInsert
{
    Tool tA = new(name: "A", width: 10);
    Tool tB = new(name: "B", width: 30);
    Tool tC = new(name: "C", width: 20);

    [Test]
    public void AssertInsert()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 2)
            ];
        Machine machine = new(machineStorage);
        Problems insertResult = machine.Insert(new RecipeItem("A", 0));
        Assert.That
            (Problems.NoProblem == insertResult
            , "The tool should be inserted");
    }

    [Test]
    public void AssertInsertNotEnoughTools()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 1)
            ];
        Machine machine = new(machineStorage);
        Problems insertResult = machine.Insert(new RecipeItem("A", 0));
        Assert.That
            (Problems.NoProblem == insertResult
            , "The tool should be inserted");
        insertResult = machine.Insert(new RecipeItem("A", 0));
        Assert.That
            (Problems.NotEnoughTools == insertResult
            , "The tool should not be inserted as there are not enough tools");
    }

    [Test]
    public void AssertInsertOverlappingTools()
    {
        List<ToolStorage> machineStorage =
            [ new(tA, 2)
            ];
        Machine machine = new(machineStorage);
        Problems insertResult = machine.Insert(new RecipeItem("A", 0));
        Assert.That
            (Problems.NoProblem == insertResult
            , "The tool should be inserted");
        insertResult = machine.Insert(new RecipeItem("A", 9));
        Assert.That
            (Problems.OverlappingTools == insertResult
            , "The tool should not be inserted as there are overlapping tools");
    }
}