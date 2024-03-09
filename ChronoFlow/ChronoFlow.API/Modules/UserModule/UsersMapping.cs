using AutoMapper;
using ChronoFlow.API.DAL.Entities;

namespace ChronoFlow.API.Modules.UserModule;

public class UsersMapping : Profile
{
    public UsersMapping()
    {
        CreateMap<UserEntity, UserEntity>();
    }
}