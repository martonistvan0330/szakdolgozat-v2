﻿namespace HomeworkManager.Model.Entities;

public class ManagedGroup
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}