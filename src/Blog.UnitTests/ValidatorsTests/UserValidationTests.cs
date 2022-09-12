using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Validators;
using Blog.Application.Features.User.DTO;
using Blog.Application.Features.User.Validators;
using Blog.Application.Interfaces;
using Blog.Application.Models;
using FluentValidation.TestHelper;
using Moq;

namespace Blog.UnitTests.Application.ValidatorsTests;

public class UserValidationTests
{

    [Fact]
    public async Task UserSignUpValidator_InvalidData()
    {
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetUserByNameAsync(It.IsAny<string>())).ReturnsAsync(new User()); //mock of returning existing user with provided username
        userServiceMock.Setup(s => s.ListUsersAsync((string?)null)).ReturnsAsync(new List<User>() { new User() { Email = "test@example.com" } }); //mock of returning existing user with provided email

        //arrange
        var validator = new UserSignUpValidator(userServiceMock.Object);
        var testDTO = new UserSignUpDTO()
        {
           Username = "abcde",
           Password = "abcd",
           RepeatPassword = "efgh",
           Email = "test@example.com"
        };

        //act
        var res = await validator.TestValidateAsync(testDTO);

        //assert
        res.ShouldHaveValidationErrorFor(t => t.Username);
        res.ShouldHaveValidationErrorFor(t => t.Password);
        res.ShouldNotHaveValidationErrorFor(t => t.RepeatPassword);
    }

    [Fact]
    public async Task CreatePostDTOValidator_ValidData()
    {
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(s => s.GetUserByNameAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        userServiceMock.Setup(s => s.ListUsersAsync((string?)null)).ReturnsAsync(new List<User>());


        //arrange
        var validator = new UserSignUpValidator(userServiceMock.Object);
        var testDTO = new UserSignUpDTO()
        {
            Username = "user",
            Password = "abcde",
            RepeatPassword = "abcde",
            Email = "test@example.com"
        };

        //act
        var res = await validator.TestValidateAsync(testDTO);

        //assert
        res.ShouldNotHaveAnyValidationErrors();
    }
}
