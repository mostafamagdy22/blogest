using AutoMapper;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.domain.Entities;

namespace blogest.application.Interfaces.services
{
    public class MappingApplicationProfile : Profile
    {
        public MappingApplicationProfile()
        {
            CreateMap<Post, CreatePostResponseDto>()
            .ForMember(dest => dest.postId, opt => opt.MapFrom(src => src.PostId))
            .ReverseMap();

            CreateMap<Post, CreatePostCommand>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishedAt))
            .ReverseMap();

            CreateMap<User, SignUpCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ReverseMap();

            CreateMap<Comment, CreateCommentCommand>()
            .ForMember(opt => opt.Content, dist => dist.MapFrom(src => src.Content))
            .ForMember(opt => opt.PostId, dist => dist.MapFrom(src => src.PostId))
            .ForMember(opt => opt.UserId, dist => dist.MapFrom(src => src.UserId))
            .ReverseMap();
        }
    }
}