namespace Press.Tests;
[TestFixture]
public class MyTestClass
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