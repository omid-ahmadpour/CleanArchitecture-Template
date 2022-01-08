using AutoMapper;
using CleanTemplate.Api.Controllers.v1.Users.Requests;
using CleanTemplate.Application.Users.Command.CreateUser;
using CleanTemplate.Application.Users.Command.Login;

namespace CleanTemplate.Api.AutoMapperProfiles.Users
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
