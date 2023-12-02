﻿namespace HomeworkManager.Model.CustomEntities.Appointment;

public class AppointmentModel
{
    public required int AppointmentId { get; set; }
    public required string Time { get; set; }
    public required string TeacherName { get; set; }
    public required string TeacherEmail { get; set; }
    public required bool IsAvailable { get; set; }
    public required bool IsMine { get; set; }
}