using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Commentaries.Validators;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Validators;
using Blog.Application.Features.User.DTO;
using Blog.Application.Features.User.Validators;
using Blog.Tests.Mocks;
using FluentValidation.TestHelper;
namespace Blog.UnitTests.Application.ValidatorsTests;

public class CommentariesValidationTests
{

    [Fact]
    public async Task CreateCommentaryDTOValidator_InvalidData()
    {
        //arrange
        var postsRepo = RepoMocks.GetPostRepoMock();
        var validator = new CreateCommentaryDTOValidator(postsRepo.Object);
        var testDTO = new CreateCommentaryDTO()
        {
            Text = "",
            PostId = 999 //non existing id
        };

        //act
        var res = await validator.TestValidateAsync(testDTO);

        //assert
        res.ShouldHaveValidationErrorFor(t => t.Text);
        res.ShouldHaveValidationErrorFor(t => t.PostId);
    }
    [Fact]
    public async Task CreateCommentaryDTOValidator_ValidData()
    {
        //arrange
        var postsRepo = RepoMocks.GetPostRepoMock();
        var validator = new CreateCommentaryDTOValidator(postsRepo.Object);
        var testDTO = new CreateCommentaryDTO()
        {
            Text = "This is a commentary for an existing post with ID 2.",
            PostId = 2
        };

        //act
        var res = await validator.TestValidateAsync(testDTO);

        //assert
        res.ShouldNotHaveAnyValidationErrors();
    }
}
