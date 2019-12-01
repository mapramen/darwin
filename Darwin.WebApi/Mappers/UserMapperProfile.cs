namespace Darwin.WebApi.Mappers
{
    using AutoMapper;

    using Core = Darwin.Data.Models;
    using Api = Darwin.WebApi.Models;

    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<Core.User, Api.User>();
        }
    }
}