namespace ChronoFlow.API.Modules.EventsModule.Responses
{
    public class EventDateFilterResponse
    {
        public EventDateFilterResponse(string name, TimeSpan? time)
        {
            Name = name;
            Time = time;
        }

        public string Name { get; set; }
        public TimeSpan? Time { get; set; }
    }
}