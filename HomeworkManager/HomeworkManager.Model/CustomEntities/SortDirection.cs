namespace HomeworkManager.Model.CustomEntities;

public enum SortDirection
{
    Ascending,
    Descending
}

public static class SortDirectionExtensions
{
    public static SortDirection ToSortDirection(this string? sortDirection)
    {
        return sortDirection switch
        {
            "asc" => SortDirection.Ascending,
            "desc" => SortDirection.Descending,
            _ => throw new InvalidOperationException()
        };
    }
}