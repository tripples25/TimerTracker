namespace ChronoFlow.API.DAL.Entities.Response;

public class CreateOrUpdateResponse : IResponse
{
    public Guid Id { get; set; }
    public bool IsCreated { get; set; }
    //public string EntityType { get; set; }
}