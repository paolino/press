namespace Press;

/// <summary>
/// A press is a list of placed tools, that are always sorted by position
/// </summary>
public class Press
{
    public List<Placed> Tools { get; } = [];

    private bool CheckPrev(Placed tool, int index)

    {
        if (index == 0) return true; // there is no previous tool
        Placed prev = Tools[index - 1];
        return (prev.Position + prev.Width <= tool.Position);
    }

    /// <summary>
    /// Insert a tool in the recipe
    /// </summary>
    /// <param name="tool"></param>
    /// <returns>
    /// true if the tool was inserted
    /// false if the tool could not be inserted
    /// </returns>
    public Problems Insert(Placed tool)
    {
        int index = Tools.FindIndex(y => y.Position > tool.Position);
        if (index < 0) // no placed tool after this one
        {
            if (CheckPrev(tool, Tools.Count)) // respect the previous tool
            {
                Tools.Add(tool);
                return Problems.NoProblem;
            }
        }
        else
        {
            Placed next = Tools[index];
            if (CheckPrev(tool, index)) // respect the next tool
            {
                if (tool.Position + tool.Width <= next.Position)
                {
                    Tools.Insert(index, tool);
                    return Problems.NoProblem;
                }
            }
        }
        return Problems.OverlappingTools;
    }

    /// <summary>
    /// Insert a list of tools
    /// </summary>
    /// <param name="tools"></param>
    /// <returns>
    /// true if all tools were inserted
    /// false as soon as one tool could not be inserted
    /// </returns>
    public Problems Insert(List<Placed> tools)
    {
        foreach (Placed tool in tools)
            switch (Insert(tool))
            {
                case Problems.OverlappingTools:
                    return Problems.OverlappingTools;
            }
        return Problems.NoProblem;
    }

    /// <summary>
    /// Find a place for a tool of given width
    /// </summary>
    /// <param name="width"></param>
    /// <returns>
    /// The first position where the tool can be placed
    ///
    public int FindPlace(int width)
    {
        int position = 0;
        foreach (Placed tool in Tools)
        {
            if (position + width <= tool.Position) return position;
            position = tool.Position + tool.Width;
        }
        return position;
    }

    /// <summary>
    /// Insert a tool in the press where possible
    /// </summary>
    public int InsertWherePossible(Tool tool)
    {
        int place = FindPlace(tool.Width);
        Insert(new Placed(tool, place));
        return place;
    }

    /// <summary>
    /// Export a recipe that represent the press
    /// </summary>
    public Recipe ExportRecipe()
    {
        List<RecipeItem> items = [];
        foreach (Placed tool in Tools)
            items.Add(new RecipeItem(tool.Name, tool.Position));
        return new Recipe(items);
    }
}
