﻿using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.API.Validation.Course;
using HomeworkManager.API.Validation.Group;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Course/{courseId:int}/Group")]
public class GroupController : ControllerBase
{
    private readonly CourseIdValidator _courseIdValidator;
    private readonly IGroupManager _groupManager;
    private readonly IUserManager _userManager;
    
    public GroupController
    (
        CourseIdValidator courseIdValidator,
        IGroupManager groupManager,
        IUserManager userManager
    )
    {
        _courseIdValidator = courseIdValidator;
        _groupManager = groupManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupListRow>>> GetAllAsync(int courseId, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var allGroupsByUserResult = await _groupManager.GetAllAsync(courseId, cancellationToken);

        return allGroupsByUserResult.ToActionResult();
    }

    [HttpGet("{groupName}/Exist")]
    public async Task<ActionResult<bool>> ExistsAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }

        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync(groupName, cancellationToken);

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        return await _groupManager.ExistsWithNameAsync(courseId, groupName, cancellationToken);
    }

    [HttpGet("{groupName}")]
    public async Task<ActionResult<GroupModel?>> GetAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync(groupName, cancellationToken);

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var getResult = await _groupManager.GetModelAsync(courseId, groupName, cancellationToken);

        return getResult.ToActionResult();
    }

    [HttpGet("{groupName}/Teacher")]
    public async Task<ActionResult<Pageable<UserListRow>>> GetTeachersAsync(int courseId, string groupName,
        [FromQuery] PageableOptions pageableOptions, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync(groupName, cancellationToken);

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var getTeachersResult = await _groupManager.GetTeachersAsync(courseId, groupName, pageableOptions, cancellationToken);

        return getTeachersResult.ToActionResult();
    }

    [HttpGet("{groupName}/Student")]
    public async Task<ActionResult<Pageable<UserListRow>>> GetStudentsAsync(int courseId, string groupName,
        [FromQuery] PageableOptions pageableOptions, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync(groupName, cancellationToken);

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var getStudentsResult = await _groupManager.GetStudentsAsync(courseId, groupName, pageableOptions, cancellationToken);

        return getStudentsResult.ToActionResult();
    }

    [HttpGet("{groupName}/Teacher/Addable")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync
        (
            groupName,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken
        );

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var getTeachersResult = await _groupManager.GetAddableTeachersAsync(courseId, groupName, cancellationToken);

        return getTeachersResult.ToActionResult();
    }

    [HttpGet("{groupName}/Student/Addable")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync
        (
            groupName,
            options => { options.IncludeRuleSets("Default", "IsTeacher"); },
            cancellationToken
        );

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var getStudentsResult = await _groupManager.GetAddableStudentsAsync(courseId, groupName, cancellationToken);

        return getStudentsResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateAsync(int courseId, NewGroup newGroup, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync
        (
            courseId,
            options => { options.IncludeRuleSets("Default", "IsTeacher"); },
            cancellationToken
        );

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }

        var newGroupValidator = new NewGroupValidator(courseId, _groupManager);

        var newGroupValidationResult = await newGroupValidator.ValidateAsync(newGroup, cancellationToken);

        if (!newGroupValidationResult.IsValid)
        {
            return newGroupValidationResult.ToActionResult();
        }
        
        var createResult = await _groupManager.CreateAsync(newGroup, courseId, cancellationToken);

        return createResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPut("{groupName}")]
    public async Task<ActionResult> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync
        (
            groupName,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken
        );

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }

        var updatedGroupValidator = new UpdatedGroupValidator(courseId, groupName,_groupManager);
        
        var updatedGroupValidationResult = await updatedGroupValidator.ValidateAsync(updatedGroup, cancellationToken);

        if (!updatedGroupValidationResult.IsValid)
        {
            return updatedGroupValidationResult.ToActionResult();
        }
        
        var updateResult = await _groupManager.UpdateAsync(courseId, groupName, updatedGroup, cancellationToken);

        return updateResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost("{groupName}/Teacher/Add")]
    public async Task<ActionResult> AddTeachersAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync
        (
            groupName,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken
        );

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var addResult = await _groupManager.AddTeachersAsync(courseId, groupName, userIds, cancellationToken);

        return addResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost("{groupName}/Student/Add")]
    public async Task<ActionResult> AddStudentsAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var groupNameValidator = new GroupNameValidator(courseId, _groupManager, _userManager);
        
        var groupNameValidationResult = await groupNameValidator.ValidateAsync
        (
            groupName,
            options => { options.IncludeRuleSets("Default", "IsTeacher"); },
            cancellationToken
        );

        if (!groupNameValidationResult.IsValid)
        {
            return groupNameValidationResult.ToActionResult();
        }
        
        var addResult = await _groupManager.AddStudentsAsync(courseId, groupName, userIds, cancellationToken);

        return addResult.ToActionResult();
    }

    [HttpGet("{groupName}/IsInGroup")]
    public async Task<ActionResult<bool>> IsInGroupAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        return await _groupManager.IsInGroupAsync(courseId, groupName, cancellationToken);
    }

    [HttpGet("{groupName}/IsCreator")]
    public async Task<ActionResult<bool>> IsCreatorAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        return await _groupManager.IsCreatorAsync(courseId, groupName, cancellationToken);
    }

    [HttpGet("{groupName}/IsTeacher")]
    public async Task<ActionResult<bool>> IsTeacherAsync(int courseId, string groupName, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        return await _groupManager.IsTeacherAsync(courseId, groupName, cancellationToken);
    }

    [HttpGet("NameAvailable")]
    public async Task<ActionResult<bool>> NameAvailableAsync(int courseId, string name, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        return await _groupManager.NameAvailableAsync(courseId, name, cancellationToken);
    }
}