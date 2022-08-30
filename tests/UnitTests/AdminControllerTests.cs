using MiniBlog.Web.Controllers;
using MiniBlog.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MiniBlog.Core.Constants;
using MiniBlog.Web.Exceptions;

namespace MiniBlog.Tests;

public class AdminControllerTests
{
    private Mock<UserManager<IdentityUser>> UserManagerMock { get; set; } = new(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

    private Mock<RoleManager<IdentityRole>> RoleManagerMock { get; set; } = new(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

    private AdminController GetTestControllerWithMocks() => new AdminController(UserManagerMock.Object, RoleManagerMock.Object);


    [Fact]
    public void UserList_CallsAction_ReturnsViewWithCorrectModel()
    {
        var allUsers = GetUsers(10);
        var adminRole = new IdentityRole { Name = RolesConstants.ADMIN_ROLE };
        UserManagerMock.Setup(m => m.Users).Returns(allUsers.AsQueryable);
        //first 5 users has Admins role
        UserManagerMock.Setup(m => m.GetRolesAsync(It.Is<IdentityUser>(u => int.Parse(u.Id) < 5))).ReturnsAsync(new List<string> { RolesConstants.ADMIN_ROLE });
        //the rest of users do not have any roles;
        UserManagerMock.Setup(m => m.GetRolesAsync(It.Is<IdentityUser>(u => int.Parse(u.Id) >= 5))).ReturnsAsync(new List<string>());
        var controller = GetTestControllerWithMocks();

        var result = controller.UserList();

        UserManagerMock.Verify(m => m.GetRolesAsync(It.IsAny<IdentityUser>()), Times.Exactly(10));
        Assert.IsType<ViewResult>(result);
        Assert.Equal(5, (((ViewResult)result).Model as UserListsViewModel)?.AdminUsers.Count());
        Assert.Equal(5, (((ViewResult)result).Model as UserListsViewModel)?.BasicUsers.Count());
    }

    [Fact]
    public async void DeleteUser_PassCorrectUserId_DeletesUserAndRedirectsToList()
    {
        var userId = "1";
        var user = new IdentityUser { Id = userId };
        UserManagerMock.Setup(m => m.FindByIdAsync(It.Is<string>(s => s == userId))).ReturnsAsync(user);
        UserManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(false);
        UserManagerMock.Setup(m => m.GetUsersInRoleAsync(It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(new IdentityUser[10]);
        var controller = GetTestControllerWithMocks();

        var result = await controller.DeleteUser(userId);

        Assert.IsType<RedirectToActionResult>(result);
        UserManagerMock.Verify(m => m.DeleteAsync(It.Is<IdentityUser>(u => u == user)), Times.Once);
        Assert.Equal("UserList", ((RedirectToActionResult)result).ActionName);
    }


    [Fact]
    public async void DeleteUser_CorrectIdOfLastAdminUser_ThrowsApplicationException()
    {
        var userId = "1";
        var user = new IdentityUser { Id = userId };
        UserManagerMock.Setup(m => m.FindByIdAsync(It.Is<string>(s => s == userId))).ReturnsAsync(user);
        UserManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(true);
        UserManagerMock.Setup(m => m.GetUsersInRoleAsync(It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(new IdentityUser[1]);
        var controller = GetTestControllerWithMocks();
        controller.Url = Mock.Of<IUrlHelper>();
        var ex = await Assert.ThrowsAsync<MiniBlogWebException>(async () => await controller.DeleteUser(userId));
        UserManagerMock.Verify(m => m.DeleteAsync(It.Is<IdentityUser>(u => u == user)), Times.Never);
        Assert.Equal("You must have at least one account with admin rights", ex.Message);
    }

    [Fact]
    public async void SwitchAdmin_PassesCorrectAdminUserId_RemovesFromRoleAndRedirectsToList()
    {
        var userId = "1";
        var user = new IdentityUser { Id = userId };
        UserManagerMock.Setup(m => m.FindByIdAsync(It.Is<string>(s => s == userId))).ReturnsAsync(user);
        UserManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(true);
        UserManagerMock.Setup(m => m.GetUsersInRoleAsync(It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(new IdentityUser[10]);
        UserManagerMock.Setup(m => m.RemoveFromRoleAsync(It.Is<IdentityUser>(u => u == user), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(IdentityResult.Success);
        var controller = GetTestControllerWithMocks();
        var result = await controller.SwitchAdmin(userId);

        UserManagerMock.Verify(m => m.RemoveFromRoleAsync(It.Is<IdentityUser>(u => u == user), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE)), Times.Once);
        UserManagerMock.Verify(m => m.AddToRoleAsync(It.Is<IdentityUser>(u => u == user), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE)), Times.Never);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("UserList", ((RedirectToActionResult)result).ActionName);
    }

    
    [Fact]
    public async void SwitchAdmin_PassesCorrectNonAdminUserId_AddsToRoleAndRedirectsToList()
    {
        var userId = "1";
        var user = new IdentityUser { Id = userId };
        UserManagerMock.Setup(m => m.FindByIdAsync(It.Is<string>(s => s == userId))).ReturnsAsync(user);
        UserManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(false);
        UserManagerMock.Setup(m => m.GetUsersInRoleAsync(It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(new IdentityUser[10]);
        UserManagerMock.Setup(m => m.AddToRoleAsync(It.Is<IdentityUser>(u => u == user), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(IdentityResult.Success);
        var controller = GetTestControllerWithMocks();
        var result = await controller.SwitchAdmin(userId);

        UserManagerMock.Verify(m => m.AddToRoleAsync(It.Is<IdentityUser>(u => u == user), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE)), Times.Once);
        UserManagerMock.Verify(m => m.RemoveFromRoleAsync(It.Is<IdentityUser>(u => u == user), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE)), Times.Never);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("UserList", ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void SwitchAdmin_PassesLastAdminUserId_ThrowsApplicationException()
    {
        var userId = "1";
        var user = new IdentityUser { Id = userId };
        UserManagerMock.Setup(m => m.FindByIdAsync(It.Is<string>(s => s == userId))).ReturnsAsync(user);
        UserManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(true);
        UserManagerMock.Setup(m => m.GetUsersInRoleAsync(It.Is<string>(s => s == RolesConstants.ADMIN_ROLE))).ReturnsAsync(new IdentityUser[1]);
        var controller = GetTestControllerWithMocks();
        controller.Url = Mock.Of<IUrlHelper>(MockBehavior.Loose);
        var ex = await Assert.ThrowsAsync<MiniBlogWebException>(async () => await controller.SwitchAdmin(userId));
        UserManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(),  It.IsAny<string>()), Times.Never);
        UserManagerMock.Verify(m => m.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        Assert.Equal("You must have at least one account with admin rights", ex.Message);
    }




    //
    //private non-test methods
    //
    private IEnumerable<IdentityUser> GetUsers(int num)
    {
        IdentityUser[] users = new IdentityUser[num];
        for (int i = 0; i < num; i++)
        {
            users[i] = new IdentityUser { Id = $"{i}", UserName = $"User{i}", Email = $"email{i}@test.com" };
        }
        return users;
    }
}