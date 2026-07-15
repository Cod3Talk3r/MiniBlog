using AutoMapper;
using MiniBlog.Application.DTOs;
using MiniBlog.Domain.Entities;

namespace MiniBlog.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDto>()
                .ForCtorParam("AuthorName", opt => opt.MapFrom(src => src.Author.UserName))
                .ForCtorParam("CommentsCount", opt => opt.MapFrom(src => src.Comments.Count));

            CreateMap<Comment, CommentDto>()
                .ForCtorParam("AuthorName", opt => opt.MapFrom(src => src.Author.UserName));
        }
    }
}