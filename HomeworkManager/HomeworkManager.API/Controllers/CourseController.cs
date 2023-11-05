using HomeworkManager.API.Attributes;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities.Course;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Course")]
public class CourseController : ControllerBase
{
    private readonly ICourseManager _courseManager;

    public CourseController(ICourseManager courseManager)
    {
        _courseManager = courseManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseCard>>> GetAllAsync()
    {
        var allCoursesByUserResult = await _courseManager.GetAllCoursesByUserAsync(User.Identity?.Name);

        return allCoursesByUserResult.Match<ActionResult<IEnumerable<CourseCard>>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpGet("{courseId:int}")]
    public async Task<ActionResult<CourseModel?>> GetAsync(int courseId)
    {
        var getResult = await _courseManager.GetModelByUserAsync(courseId, User.Identity?.Name);

        return getResult.Match<ActionResult<CourseModel?>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateAsync(NewCourse newCourse)
    {
        var createResult = await _courseManager.CreateAsync(newCourse, User.Identity?.Name);

        return createResult.Match<ActionResult<int>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPut("{courseId:int}")]
    public async Task<ActionResult> UpdateAsync(int courseId, UpdateCourse updatedCourse)
    {
        var updateError = await _courseManager.UpdateAsync(courseId, updatedCourse, User.Identity?.Name);

        if (updateError is not null)
        {
            return Forbid();
        }

        return Ok();
    }

    [HttpGet("{courseId:int}/IsCreator")]
    public async Task<ActionResult<bool>> IsCreatorAsync(int courseId)
    {
        return await _courseManager.IsCreatorAsync(courseId, User.Identity?.Name);
    }

    [HttpGet("{courseId:int}/IsTeacher")]
    public async Task<ActionResult<bool>> IsTeacherAsync(int courseId)
    {
        return await _courseManager.IsTeacherAsync(courseId, User.Identity?.Name);
    }

    [HttpGet("NameAvailable")]
    public async Task<ActionResult<bool>> NameAvailableAsync(string name)
    {
        return await _courseManager.NameAvailableAsync(name);
    }
}