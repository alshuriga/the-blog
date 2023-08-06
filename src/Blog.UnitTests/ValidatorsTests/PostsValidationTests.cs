using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Validators;
using FluentValidation.TestHelper;
namespace Blog.UnitTests.Application.ValidatorsTests;

public class PostValidationTests
{

    [Fact]
    public void CreatePostDTOValidator_InvalidData()
    {
        //arrange
        var validator = new CreatePostDTOValidator();
        var testDTO = new CreatePostDTO()
        {
            Header = "",
            Text = "",
        };

        //act
        var res = validator.TestValidate(testDTO);

        //assert
        res.ShouldHaveValidationErrorFor(t => t.Header);
        res.ShouldHaveValidationErrorFor(t => t.Text);
    }

    [Fact]
    public void CreatePostDTOValidator_ValidData()
    {
        //arrange
        var validator = new CreatePostDTOValidator();
        var testDTO = new CreatePostDTO()
        {
            Header = "Header",
            Text = "Text",
            TagString = "abcd,efgh"
        };

        //act
        var res = validator.TestValidate(testDTO);

        //assert
        res.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdatePostDTOValidator_InvalidData()
    {
        //arrange
        var validator = new UpdatePostDTOValidator();
        var testDTO = new UpdatePostDTO()
        {
            Header = "",
            Text = ""
        };

        //act
        var res = validator.TestValidate(testDTO);

        //assert
        res.ShouldHaveValidationErrorFor(t => t.Header);
        res.ShouldHaveValidationErrorFor(t => t.Text);
    }

    [Fact]
    public void UpdatePostDTOValidator_ValidData()
    {
        //arrange
        var validator = new UpdatePostDTOValidator();
        var testDTO = new UpdatePostDTO()
        {
            Header = "Header",
            Text = "Text",
            TagString = "abcd,efgh,7ag"
        };

        //act
        var res = validator.TestValidate(testDTO);

        //assert
        res.ShouldNotHaveAnyValidationErrors();
    }


}
