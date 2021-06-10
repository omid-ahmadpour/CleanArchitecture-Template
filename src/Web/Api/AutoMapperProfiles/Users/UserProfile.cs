using Api.Controllers.v1.Users.Requests;
using Application.Users.Command.CreateUser;
using Application.Users.Command.Login;
using AutoMapper;

namespace Api.AutoMapperProfiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SingUpRequest, CreateUserCommand>();

            CreateMap<LoginRequest, LoginCommand>();
        }
    }
}
