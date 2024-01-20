namespace ChronoFlow.API.Modules.EventsModule.Responses
{
    public class AnalyticsResponse
    {
        public AnalyticsResponse(HashSet<EventAnalyticsModule> events, int totalCount, int totalHours)
        {
            Events = events;
            TotalCount = totalCount;
            TotalHours = totalHours;
        }

        public HashSet<EventAnalyticsModule> Events { get; set; }
        public int TotalCount { get; set; }
        public int TotalHours { get; set; }
    }
}