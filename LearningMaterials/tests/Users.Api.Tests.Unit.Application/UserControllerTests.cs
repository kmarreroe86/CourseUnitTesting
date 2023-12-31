﻿using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Users.Api.Controllers;
using Users.Api.Models;
using Users.Api.Services;
using Users.Api.Mappers;
using Xunit;
using System.Threading.Tasks;
using NSubstitute.ReturnsExtensions;
using System.Linq;
using System.Collections.Generic;
using Users.Api.Contracts;

namespace Users.Api.Tests.Unit.Application;
public class UserControllerTests
{

    private readonly UserController _sut;
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserControllerTests()
    {
        _sut = new UserController(_userService);
    }

    [Fact]
    public async Task GetById_ReturnOkAndObject_WhenUserExists()
    {

        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe"
        };
        _userService.GetByIdAsync(Arg.Any<Guid>()).Returns(user);
        var userResponse = user.ToUserResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetById(user.Id);


        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(userResponse);

    }

    [Fact]
    public async Task GetById_ReturnNotFoundAndNoBody_WhenUserNotExists()
    {

        // Arrange
        _userService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult)await _sut.GetById(Guid.NewGuid());


        // Assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAll_ShoudReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userService.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ShoudReturnUsers_WhenUsersExist()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "John Doe"
        };
        var users = new[] { user };
        var userResponse = users.Select(u => u.ToUserResponse());
        _userService.GetAllAsync().Returns(users);

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be(200);
        result.Value.As<IEnumerable<UserResponse>>().Should().BeEquivalentTo(userResponse);
    }


    [Fact]
    public async Task Create_ShouldCreateUser_WhenCreateUserRequestIsValid()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest
        {
            FullName = "Jonh Doe"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = createUserRequest.FullName
        };
        _userService.CreateAsync(Arg.Do<User>(x => user = x)).Returns(true); // Call the acction passed to. sets user = x, whatever value is created in _userService.CreateAsync


        // Act
        var result = (CreatedAtActionResult)await _sut.Create(createUserRequest);

        // Assert
        var expectedUserResponse = user.ToUserResponse();
        result.StatusCode.Should().Be(201);
        result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedUserResponse);
        result.RouteValues!["id"].Should().BeEquivalentTo(user.Id);

        // result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedUserResponse, options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenCreateUserRequestIsInvalid()
    {
        // Arrange
        _userService.CreateAsync(Arg.Any<User>()).Returns(false);

        // Act
        var result = (BadRequestResult)await _sut.Create(new CreateUserRequest());

        // Assert
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task DeleteById_ReturnOk_WhenUserWasDeleted()
    {

        // Arrange
        _userService.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);

        // Act
        var result = (OkResult)await _sut.DeleteById(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteById_ReturnNotFound_WhenUserNotExists()
    {

        // Arrange
        _userService.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.DeleteById(Guid.NewGuid());


        // Assert
        result.StatusCode.Should().Be(404);
    }
}
