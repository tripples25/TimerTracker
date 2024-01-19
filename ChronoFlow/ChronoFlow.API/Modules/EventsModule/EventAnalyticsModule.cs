namespace ChronoFlow.API.Modules.EventsModule
{
    public class EventAnalyticsModule
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        //private int arithmeticMeanTimeInS { get; set; }
        public EventAnalyticsModule(string name, TimeSpan time, int count)
        {
            this.Name = name;
            this.Time = time;
            this.Count = count;

        }

    }
}
