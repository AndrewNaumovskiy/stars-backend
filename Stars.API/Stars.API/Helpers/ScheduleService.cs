using Microsoft.Extensions.Caching.Memory;
using Stars.API.Models.RequestModels;

namespace Stars.API.Helpers;

public class ScheduleService
{
    private const string Key = "schedule";

    private readonly IMemoryCache _cache;
    private int _hourOffset = 2;

    private Dictionary<int, (TimeOnly, TimeOnly)> _lessonDate = new()
    {
        { 1, (new TimeOnly(08, 00, 0), new TimeOnly(09, 20, 0)) },
        { 2, (new TimeOnly(09, 35, 0), new TimeOnly(10, 55, 0)) },
        { 3, (new TimeOnly(11, 25, 0), new TimeOnly(12, 45, 0)) },
        { 4, (new TimeOnly(12, 55, 0), new TimeOnly(14, 15, 0)) },
    };

    public ScheduleService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Dictionary<int, (TimeOnly, TimeOnly)> Schedule()
    {
        if(_cache.TryGetValue(Key, out Dictionary<int, (TimeOnly, TimeOnly)> schedule))
        {
            return schedule;
        }

        return _lessonDate;
    }

    public void SetSchedule(ScheduleModel body)
    {
        Dictionary<int, (TimeOnly, TimeOnly)> temp = new()
        {
            { 1, (body.FirstStart,  body.FirstEnd )},
            { 2, (body.SecondStart, body.SecondEnd )},
            { 3, (body.ThirdStart,  body.ThirdEnd )},
            { 4, (body.FourthStart, body.FourthEnd )},
        };

        _cache.Set(Key, temp, TimeSpan.FromHours(16));
    }

    public ScheduleRequestModel GetSchedule()
    {
        var now = DateTime.Now;
        var refDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

        var schedule = Schedule();
        return new ScheduleRequestModel()
        {
            FirstStart = refDate + schedule[1].Item1.ToTimeSpan(),
            FirstEnd = refDate + schedule[1].Item2.ToTimeSpan(),
            SecondStart = refDate + schedule[2].Item1.ToTimeSpan(),
            SecondEnd = refDate + schedule[2].Item2.ToTimeSpan(),
            ThirdStart = refDate + schedule[3].Item1.ToTimeSpan(),
            ThirdEnd = refDate + schedule[3].Item2.ToTimeSpan(),
            FourthStart = refDate + schedule[4].Item1.ToTimeSpan(),
            FourthEnd = refDate + schedule[4].Item2.ToTimeSpan(),
        };
    }

    public int GetHourOffset()
    {
        return _hourOffset;
    }

    public void SetHourOffset(int offset)
    {
        _hourOffset = offset;
    }
}
