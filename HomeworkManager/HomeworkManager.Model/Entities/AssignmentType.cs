using HomeworkManager.Model.Constants;

namespace HomeworkManager.Model.Entities;

public class AssignmentType
{
    public AssignmentTypeId AssignmentTypeId { get; set; }
    public required string Name { get; set; }
}