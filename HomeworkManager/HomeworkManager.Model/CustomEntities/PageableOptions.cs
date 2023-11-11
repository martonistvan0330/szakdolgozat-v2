namespace HomeworkManager.Model.CustomEntities;

public class PageableOptions
{
    public PageData? PageData { get; set; }
    public SortOptions? SortOptions { get; set; }
    public string? SearchText { get; set; }
}