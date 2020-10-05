using AutoMapper;
using WebApi.Entities;
using WebApi.Requests.Installations;
using WebApi.Requests.Users;
using WebApi.Responses.Installation;
using WebApi.Responses.Users;

namespace WebApi.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateInstallationRequest, Installation>();
            CreateMap<Installation, CreateInstallationResponseData>();

            CreateMap<GetInstallationRequest, Installation>();
            CreateMap<Installation, GetInstallationResponseData>();

            CreateMap<UpdateInstallationRequest, Installation>();
            CreateMap<Installation, UpdateInstallationResponseData>();


            CreateMap<LoginUserRequest, User>();
            CreateMap<User, LoginUserResponseData>();

            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, RegisterUserResponseData>();

            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UpdateUserResponseData>();
        }
    }
}
