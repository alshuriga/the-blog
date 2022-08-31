using AutoMapper;
using Blog.Core.Entities;
using Blog.Application.Features;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Tag.DTO;
using Blog.Application.Features.Commentary;
using Blog.Application.Mapping.Resolvers;

namespace Blog.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDTO>();
        CreateMap<CreatePostDTO, Post>();
        CreateMap<UpdatePostDto, Post>();
        CreateMap<Post, PostListDTO>().ForMember(dest => dest.CommentariesCount, opts => opts.MapFrom<CommentsCountResolver>());

        CreateMap<Tag, TagDTO>();

        CreateMap<Commentary, CommentaryDTO>();
        CreateMap<Commentary, CreateCommentaryDTO>();
    }
}
