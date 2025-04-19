using ApiWithAuth.Models.Dtos.Tweet;
using ApiWithAuth.Models.Dtos.User;
using ApiWithAuth.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ApiWithAuth;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TweetAddDto, Tweet>();
        CreateMap<Tweet, TweetDto>();
        CreateMap<IdentityUser, UserDto>();
    }
}