namespace Stars.API.Helpers;

public static class ScheduleHelper
{
    private static Dictionary<int, (TimeOnly, TimeOnly)> _lessonDate = new()
    {
        { 1, (new TimeOnly(08, 00, 0), new TimeOnly(09, 20, 0)) },
        { 2, (new TimeOnly(09, 35, 0), new TimeOnly(10, 55, 0)) },
        { 3, (new TimeOnly(11, 25, 0), new TimeOnly(12, 45, 0)) },
        { 4, (new TimeOnly(12, 55, 0), new TimeOnly(14, 15, 0)) },
    };

    public static Dictionary<int, (TimeOnly, TimeOnly)> Schedule => _lessonDate;
}
