using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.API.Validation.Course;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Course")]
public class CourseController : ControllerBase
{
    private readonly CourseIdValidator _courseIdValidator;
    private readonly ICourseManager _courseManager;
    private readonly NewCourseValidator _newCourseValidator;

    public CourseController
    (
        CourseIdValidator courseIdValidator,
        ICourseManager courseManager,
        NewCourseValidator newCourseValidator
    )
    {
        _courseIdValidator = courseIdValidator;
        _courseManager = courseManager;
        _newCourseValidator = newCourseValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseCard>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var allCoursesByUserResult = await _courseManager.GetAllAsync(cancellationToken);

        return allCoursesByUserResult.ToActionResult();
    }

    [HttpGet("{courseId:int}/Exist")]
    public async Task<ActionResult<bool>> ExistsAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _courseManager.ExistsWithIdAsync(courseId, cancellationToken);
    }

    [HttpGet("{courseId:int}")]
    public async Task<ActionResult<CourseModel>> GetAsync(int courseId, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var getResult = await _courseManager.GetModelAsync(courseId, cancellationToken);

        return getResult.ToActionResult();
    }

    [HttpGet("{courseId:int}/Teacher")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetTeachersAsync(int courseId, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var getTeachersResult = await _courseManager.GetTeachersAsync(courseId, cancellationToken);

        return getTeachersResult.ToActionResult();
    }

    [HttpGet("{courseId:int}/Student")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetStudentsAsync(int courseId, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(courseId, cancellationToken);

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var getStudentsResult = await _courseManager.GetStudentsAsync(courseId, cancellationToken);

        return getStudentsResult.ToActionResult();
    }
    
    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpGet("{courseId:int}/Teacher/Addable")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync
        (
            courseId,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken
        );

        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var getTeachersResult = await _courseManager.GetAddableTeachersAsync(courseId, cancellationToken);

        return getTeachersResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpGet("{courseId:int}/Student/Addable")]
    public async Task<ActionResult<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(
            courseId,
            options => { options.IncludeRuleSets("Default", "IsTeacher"); },
            cancellationToken);
        
        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var getStudentsResult = await _courseManager.GetAddableStudentsAsync(courseId, cancellationToken);

        return getStudentsResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateAsync(NewCourse newCourse, CancellationToken cancellationToken)
    {
        var newCourseValidationResult = await _newCourseValidator.ValidateAsync(newCourse, cancellationToken);

        if (!newCourseValidationResult.IsValid)
        {
            return newCourseValidationResult.ToActionResult();
        }
        
        var createResult = await _courseManager.CreateAsync(newCourse, cancellationToken);

        return createResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPut("{courseId:int}")]
    public async Task<ActionResult> UpdateAsync(int courseId, UpdatedCourse updatedCourse, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(
            courseId,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken);
        
        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }

        var updatedCourseValidator = new UpdatedCourseValidator(courseId, _courseManager);
        
        var updatedCourseValidationResult = await updatedCourseValidator.ValidateAsync(updatedCourse, cancellationToken);

        if (!updatedCourseValidationResult.IsValid)
        {
            return updatedCourseValidationResult.ToActionResult();
        }
        
        var updateResult = await _courseManager.UpdateAsync(courseId, updatedCourse, cancellationToken);

        return updateResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost("{courseId:int}/Teacher/Add")]
    public async Task<ActionResult> AddTeachersAsync(int courseId, ICollection<Guid> userIds, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(
            courseId,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken);
        
        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var addResult = await _courseManager.AddTeachersAsync(courseId, userIds, cancellationToken);

        return addResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost("{courseId:int}/Student/Add")]
    public async Task<ActionResult> AddStudentsAsync(int courseId, ICollection<Guid> userIds, CancellationToken cancellationToken)
    {
        var courseIdValidationResult = await _courseIdValidator.ValidateAsync(
            courseId,
            options => { options.IncludeRuleSets("Default", "IsTeacher"); },
            cancellationToken);
        
        if (!courseIdValidationResult.IsValid)
        {
            return courseIdValidationResult.ToActionResult();
        }
        
        var addResult = await _courseManager.AddStudentsAsync(courseId, userIds, cancellationToken);

        return addResult.ToActionResult();
    }

    [HttpGet("{courseId:int}/IsInCourse")]
    public async Task<ActionResult<bool>> IsInCourseAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _courseManager.IsInCourseAsync(courseId, cancellationToken);
    }

    [HttpGet("{courseId:int}/IsCreator")]
    public async Task<ActionResult<bool>> IsCreatorAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _courseManager.IsCreatorAsync(courseId, cancellationToken);
    }

    [HttpGet("{courseId:int}/IsTeacher")]
    public async Task<ActionResult<bool>> IsTeacherAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _courseManager.IsTeacherAsync(courseId, cancellationToken);
    }

    [HttpGet("NameAvailable")]
    public async Task<ActionResult<bool>> NameAvailableAsync(string name, CancellationToken cancellationToken)
    {
        return await _courseManager.NameAvailableAsync(name, cancellationToken);
    }
}