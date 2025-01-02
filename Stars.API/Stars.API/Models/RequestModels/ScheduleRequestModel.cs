namespace Stars.API.Models.RequestModels;

public class ScheduleModel
{
    public TimeOnly FirstStart { get; set; }
    public TimeOnly FirstEnd { get; set; }

    public TimeOnly SecondStart { get; set; }
    public TimeOnly SecondEnd { get; set; }

    public TimeOnly ThirdStart { get; set; }
    public TimeOnly ThirdEnd { get; set; }

    public TimeOnly FourthStart { get; set; }
    public TimeOnly FourthEnd { get; set; }

    public ScheduleModel(ScheduleRequestModel body)
    {
        FirstStart = TimeOnly.FromDateTime(body.FirstStart);
        FirstEnd = TimeOnly.FromDateTime(body.FirstEnd);

        SecondStart = TimeOnly.FromDateTime(body.SecondStart);
        SecondEnd = TimeOnly.FromDateTime(body.SecondEnd);

        ThirdStart = TimeOnly.FromDateTime(body.ThirdStart);
        ThirdEnd = TimeOnly.FromDateTime(body.ThirdEnd);

        FourthStart = TimeOnly.FromDateTime(body.FourthStart);
        FourthEnd = TimeOnly.FromDateTime(body.FourthEnd);
    }
}

public class ScheduleRequestModel
{
    public DateTime FirstStart { get; set; }
    public DateTime FirstEnd { get; set; }

    public DateTime SecondStart { get; set; }
    public DateTime SecondEnd { get; set; }

    public DateTime ThirdStart { get; set; }
    public DateTime ThirdEnd { get; set; }

    public DateTime FourthStart { get; set; }
    public DateTime FourthEnd { get; set; }

    public ScheduleRequestModel() { }
}
