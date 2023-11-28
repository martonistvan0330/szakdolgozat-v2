namespace HomeworkManager.Shared.Services;

public static class TimeHelper
{
    public static int GetMinutes(string time)
    {
        string[] parts = time.Split(':');
        int hours = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);

        return hours * 60 + minutes;
    }

    public static string GetTime(int mins)
    {
        int hours = mins / 60;
        int minutes = mins % 60;
        return $"{hours:00}:{minutes:00}";
    }
}