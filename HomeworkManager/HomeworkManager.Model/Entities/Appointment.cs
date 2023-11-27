namespace HomeworkManager.Model.Entities;

public class Appointment
{
    public int AppointmentId { get; set; }
    public required DateTime Date { get; set; }
    public required int TimeInMinutes { get; set; }

    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    public Guid TeacherId { get; set; }
    public User Teacher { get; set; } = null!;

    public Guid? StudentId { get; set; }
    public User? Student { get; set; }
}