using AutoMapper;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.domain.Entities;
using blogest.infrastructure.Identity;

namespace blogest.infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AppUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts))
            .ForMember(dest => dest.RefreshTokens, opt => opt.MapFrom(src => src.RefreshTokens))
            .ReverseMap();

            CreateMap<Post, GetPostResponse>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.PublishAt, opt => opt.MapFrom(src => src.PublishedAt))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

           
        }
    }
}