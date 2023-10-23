namespace HomeworkManager.Model.CustomEntities;

public class Pageable<T>
{
    public required IEnumerable<T> Items { get; set; }
    public required int TotalCount { get; set; }
}