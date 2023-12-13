namespace ChronoFlow.API.Models;

public class CreateOrUpdateResponse : IResponse
{
    public Guid Id { get; set; }
    public bool IsCreated { get; set; }
}