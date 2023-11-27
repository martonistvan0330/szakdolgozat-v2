namespace HomeworkManager.Model.CustomEntities.Appointment;

public class AppointmentRow
{
    public required DateTime Date { get; set; }
    public ICollection<AppointmentModel> AppointmentModels { get; set; } = new List<AppointmentModel>();
}