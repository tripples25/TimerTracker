namespace ChronoFlow.API.Modules.EventsModule
{
    public class EventAnalyticsModule
    {
        private string name { get; set; }
        private int timeInMinutes { get; set; }
        private int timeInHours { get; set; }
        private int timeInSeconds { get; set; }
        private int count { get; set; }
        private int totalCount { get; set; }
        //private int arithmeticMeanTimeInS { get; set; }
        public EventAnalyticsModule(string name, int timeInMinutes, int timeInHours, int timeInSeconds, int count)
        {
            this.name = name;
            this.timeInMinutes = timeInMinutes;
            this.timeInHours = timeInHours;
            this.timeInSeconds = timeInSeconds;
            this.count = count;

        }

    }
}
