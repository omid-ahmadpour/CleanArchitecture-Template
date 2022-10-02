using AutoMapper;
using CleanTemplate.Api.Controllers.v1.Users.Requests;
using CleanTemplate.Application.Users.Command.CreateUser;
using CleanTemplate.Application.Users.Command.Login;
using CleanTemplate.Application.Users.Command.RefreshToken;

namespace CleanTemplate.Api.AutoMapperProfiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SingUpRequest, CreateUserCommand>();

            CreateMap<LoginRequest, LoginCommand>();
            CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
        }
    }
}
