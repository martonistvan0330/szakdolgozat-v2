using HomeworkManager.API.Attributes;
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
    private readonly IGroupManager _groupManager;

    public GroupController(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupListRow>>> GetAllAsync(int courseId)
    {
        var allGroupsByUserResult = await _groupManager.GetAllByUserAsync(courseId, User.Identity?.Name);

        return allGroupsByUserResult.Match<ActionResult<IEnumerable<GroupListRow>>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpGet("{groupName}/Exist")]
    public async Task<ActionResult<bool>> ExistsAsync(int courseId, string groupName)
    {
        return await _groupManager.ExistsAsync(groupName, courseId);
    }

    [HttpGet("{groupName}")]
    public async Task<ActionResult<GroupModel?>> GetAsync(int courseId, string groupName)
    {
        var getResult = await _groupManager.GetModelByUserAsync(courseId, groupName, User.Identity?.Name);

        return getResult.Match<ActionResult<GroupModel?>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpGet("{groupName}/Teacher")]
    public async Task<ActionResult<Pageable<UserListRow>>> GetTeachersAsync(int courseId, string groupName,
        [FromQuery] PageableOptions pageableOptions)
    {
        var getTeachersResult = await _groupManager.GetTeachersAsync(courseId, groupName, User.Identity?.Name, pageableOptions);

        return getTeachersResult.Match<ActionResult<Pageable<UserListRow>>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpGet("{groupName}/Student")]
    public async Task<ActionResult<Pageable<UserListRow>>> GetStudentsAsync(int courseId, string groupName,
        [FromQuery] PageableOptions pageableOptions)
    {
        var getStudentsResult = await _groupManager.GetStudentsAsync(courseId, groupName, User.Identity?.Name, pageableOptions);

        return getStudentsResult.Match<ActionResult<Pageable<UserListRow>>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpGet("{groupName}/Teacher/Addable")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, string groupName)
    {
        var getTeachersResult = await _groupManager.GetAddableTeachersAsync(courseId, groupName, User.Identity?.Name);

        return getTeachersResult.Match<ActionResult<IEnumerable<UserListRow>>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpGet("{groupName}/Student/Addable")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, string groupName)
    {
        var getStudentsResult = await _groupManager.GetAddableStudentsAsync(courseId, groupName, User.Identity?.Name);

        return getStudentsResult.Match<ActionResult<IEnumerable<UserListRow>>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateAsync(int courseId, NewGroup newGroup)
    {
        var createResult = await _groupManager.CreateAsync(newGroup, courseId, User.Identity?.Name);

        return createResult.Match<ActionResult<int>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPut("{groupName}")]
    public async Task<ActionResult> UpdateAsync(int courseId, string groupName, UpdateGroup updatedGroup)
    {
        var updateError = await _groupManager.UpdateAsync(courseId, groupName, updatedGroup, User.Identity?.Name);

        if (updateError is not null)
        {
            return Forbid();
        }

        return Ok();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost("{groupName}/Teacher/Add")]
    public async Task<ActionResult> AddTeachersAsync(int courseId, string groupName, IEnumerable<Guid> userIds)
    {
        var addError = await _groupManager.AddTeachersAsync(courseId, groupName, User.Identity?.Name, userIds);

        if (addError is not null)
        {
            return Forbid();
        }

        return Ok();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost("{groupName}/Student/Add")]
    public async Task<ActionResult> AddStudentsAsync(int courseId, string groupName, IEnumerable<Guid> userIds)
    {
        var addError = await _groupManager.AddStudentsAsync(courseId, groupName, User.Identity?.Name, userIds);

        if (addError is not null)
        {
            return Forbid();
        }

        return Ok();
    }

    [HttpGet("{groupName}/IsInGroup")]
    public async Task<ActionResult<bool>> IsInGroupAsync(int courseId, string groupName)
    {
        return await _groupManager.IsInGroupAsync(groupName, courseId, User.Identity?.Name);
    }

    [HttpGet("{groupName}/IsCreator")]
    public async Task<ActionResult<bool>> IsCreatorAsync(int courseId, string groupName)
    {
        return await _groupManager.IsCreatorAsync(groupName, courseId, User.Identity?.Name);
    }

    [HttpGet("{groupName}/IsTeacher")]
    public async Task<ActionResult<bool>> IsTeacherAsync(int courseId, string groupName)
    {
        return await _groupManager.IsTeacherAsync(groupName, courseId, User.Identity?.Name);
    }

    [HttpGet("NameAvailable")]
    public async Task<ActionResult<bool>> NameAvailableAsync(int courseId, string name)
    {
        return await _groupManager.NameAvailableAsync(name, courseId);
    }
}