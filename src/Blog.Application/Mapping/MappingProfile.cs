using AutoMapper;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Likes;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Tags.DTO;
using Blog.Application.Features.User.DTO;
using Blog.Application.Mapping.Resolvers.Posts;
using Blog.Application.Models;
using Blog.Core.Entities;

namespace Blog.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDTO>();
        CreateMap<UpdatePostDTO, Post>().ForMember(dest => dest.Tags, opts => opts.MapFrom<TagStringToTagsResolver>());
        CreateMap<Post, UpdatePostDTO>().ForMember(dest => dest.TagString, opts => opts.MapFrom<TagToTagStringResolver>());

        CreateMap<CreatePostDTO, Post>().ForMember(dest => dest.Tags, opts => opts.MapFrom<TagStringToTagsResolver>());
        CreateMap<Post, CreatePostDTO>().ForMember(dest => dest.TagString, opts => opts.MapFrom<TagToTagStringResolver>());

        CreateMap<Post, PostListVM>().ForMember(dest => dest.CommentariesCount, opts => opts.MapFrom<CommentsCountResolver>());

        CreateMap<Tag, TagDTO>();

        CreateMap<Commentary, CommentaryDTO>();
        CreateMap<CreateCommentaryDTO, Commentary>();

        CreateMap<User, UserListDTO>();

        CreateMap<Like, LikeDTO>();
        CreateMap<CreateDeleteLikeDTO, Like>();
    }
}
