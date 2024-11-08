namespace Stars.API.Models.RequestModels
{
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

        public ScheduleModel(ScheduleRequestModel body, int hourOffset)
        {
            FirstStart = TimeOnly.FromDateTime(body.FirstStart.AddHours(hourOffset));
            FirstEnd = TimeOnly.FromDateTime(body.FirstEnd.AddHours(hourOffset));

            SecondStart = TimeOnly.FromDateTime(body.SecondStart.AddHours(hourOffset));
            SecondEnd = TimeOnly.FromDateTime(body.SecondEnd.AddHours(hourOffset));

            ThirdStart = TimeOnly.FromDateTime(body.ThirdStart.AddHours(hourOffset));
            ThirdEnd = TimeOnly.FromDateTime(body.ThirdEnd.AddHours(hourOffset));

            FourthStart = TimeOnly.FromDateTime(body.FourthStart.AddHours(hourOffset));
            FourthEnd = TimeOnly.FromDateTime(body.FourthEnd.AddHours(hourOffset));
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

        public int HourOffset { get; set; }

        public ScheduleRequestModel() { }
    }
}
