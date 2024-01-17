using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.EventsModule;

namespace ChronoFlow.API.Modules.UserModule.Response
{
    public class AnalyticsResponse
    {
        public AnalyticsResponse(HashSet<EventAnalyticsModule> events, int totalCount, int totalHours)
        {
            Events = events;
            this.totalCount = totalCount;
            this.totalHours = totalHours;
        }

        private HashSet<EventAnalyticsModule> Events { get; set; }
        private int totalCount {  get; set; }
        private int totalHours { get; set; }
    }
}
