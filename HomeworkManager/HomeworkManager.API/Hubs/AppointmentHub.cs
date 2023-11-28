using HomeworkManager.API.Hubs.ClientInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace HomeworkManager.API.Hubs;

public class AppointmentHub : Hub<IAppointmentHubClient>
{
    public async Task JoinRoom(int assignmentId)
    {
        string roomName = $"Assignment_{assignmentId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        await Clients.Client(Context.ConnectionId).Refresh();
    }
}