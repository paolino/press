namespace Press;
public enum Problems
{
    NotEnoughTools,
    OverlappingTools,
    NoProblem,
    ToolNotFound,
}


public class NotEnoughTools : Exception
{
    public NotEnoughTools() {}
}

public class OverlappingTools : Exception
    {
        public OverlappingTools() {}
    }

public class ToolNotFound : Exception
    {
        public ToolNotFound() {}
    }
