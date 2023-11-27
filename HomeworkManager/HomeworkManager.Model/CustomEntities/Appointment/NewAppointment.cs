namespace HomeworkManager.Model.CustomEntities.Appointment;

public class NewAppointment
{
    public required int AssignmentId { get; set; }
    public required DateTime Date { get; set; }
    public required int PresentationLength { get; set; }
    public required string StartTime { get; set; }
    public required string EndTime { get; set; }
}