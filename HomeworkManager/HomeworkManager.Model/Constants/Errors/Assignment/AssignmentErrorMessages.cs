namespace HomeworkManager.Model.Constants.Errors.Assignment;

public static class AssignmentErrorMessages
{
    public const string ASSIGNMENT_NAME_NOT_AVAILABLE = "An assignment with the specified name already exists!";
    public const string ASSIGNMENT_WITH_ID_NOT_FOUND = "Assignment with the specified id does not exist.";
    public const string ASSIGNMENT_TYPE_REQUIRED = "Assignment type is required";
    public const string DESCRIPTION_REQUIRED = "Description is required";
    public const string DEADLINE_REQUIRED = "Deadline is required";
    public const string INVALID_DEADLINE = "Deadline has to be at least 2 weeks from today";
}