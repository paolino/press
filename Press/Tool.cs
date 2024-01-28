namespace Press;

/// <summary>
/// A tool is a thing that can has a name and a width
/// </summary>
public class Tool(string name, int width)
{
    public string Name { get; set; } = name;
    public int Width { get; set; } = width;

    public override string ToString()
    {
        return $"{Name}:{Width}";
    }
    public override bool Equals(object? obj)
    {
        return obj is Tool tool &&
               Name == tool.Name &&
               Width == tool.Width;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Width);
    }
}

/// <summary>
/// A placed tool is a tool that has a position
/// </summary>
public class Placed(Tool tool, int position) : Tool(tool.Name, tool.Width)
{
    public int Position { get; } = position;
    public override string ToString()
    {
        return $"{Name}:{Width}@{Position}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Placed placed &&
               Name == placed.Name &&
               Width == placed.Width &&
               Position == placed.Position;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Width, Position);
    }
}