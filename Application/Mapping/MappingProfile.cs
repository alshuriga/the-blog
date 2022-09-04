using AutoMapper;
using Blog.Core.Entities;
using Blog.Application.Features;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Tags.DTO;
using Blog.Application.Features.Commentaries;
using Blog.Application.Mapping.Resolvers.Posts;
using Blog.Application.Features.Posts.DTO.Common;
using Blog.Application.Interfaces.Common;

namespace Blog.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {     
        CreateMap<Post, PostDTO>();
        CreateMap<IPostDTO, Post>().ForMember(dest => dest.Tags, opts => opts.MapFrom<TagStringToTagsResolver>());
        CreateMap<UpdatePostDto, Post>();
        CreateMap<Post, UpdatePostDto>().ForMember(dest => dest.TagString, opts => opts.MapFrom<TagToTagStringResolver>());

        CreateMap<Post, PostListVM>().ForMember(dest => dest.CommentariesCount, opts => opts.MapFrom<CommentsCountResolver>());

        CreateMap<Tag, TagDTO>();

        CreateMap<Commentary, CommentaryDTO>();
        CreateMap<CreateCommentaryDTO, Commentary>();
    }
}
