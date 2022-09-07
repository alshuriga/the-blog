using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Validators;
using Blog.Application.Features.User.DTO;
using Blog.Application.Features.User.Validators;
using FluentValidation.TestHelper;
namespace Blog.UnitTests.Application.ValidatorsTests;

public class UserValidationTests
{

    [Fact]
    public void CreatePostDTOValidator_InvalidData()
    {
        //arrange
        var validator = new UserSignUpValidator();
        var testDTO = new UserSignUpDTO()
        {
           Username = "",
           Password = "abcd",
           RepeatPassword = "efgh"
        };

        //act
        var res = validator.TestValidate(testDTO);

        //assert
        res.ShouldHaveValidationErrorFor(t => t.Username);
        res.ShouldHaveValidationErrorFor(t => t.Password);
        res.ShouldNotHaveValidationErrorFor(t => t.RepeatPassword);
    }

    [Fact]
    public void CreatePostDTOValidator_ValidData()
    {
        //arrange
        var validator = new UserSignUpValidator();
        var testDTO = new UserSignUpDTO()
        {
            Username = "user",
            Password = "abcd",
            RepeatPassword = "abcd"
        };

        //act
        var res = validator.TestValidate(testDTO);

        //assert
        res.ShouldNotHaveAnyValidationErrors();
    }
}
