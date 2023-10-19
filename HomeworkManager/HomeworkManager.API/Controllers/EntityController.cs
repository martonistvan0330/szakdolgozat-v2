using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Enitity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[ApiController]
[Route("api/Entity")]
public class EntityController : ControllerBase
{
    private readonly IEntityManager _entityManager;

    private readonly ILogger<EntityController> _logger;

    public EntityController(
        ILogger<EntityController> logger,
        IEntityManager entityManager
    )
    {
        _logger = logger;
        _entityManager = entityManager;
    }

    [HttpGet("{entityId}")]
    public async Task<ActionResult<EntityModel>> GetAll(int entityId)
    {
        var entity = await _entityManager.GetOrNullAsync(entityId);

        if (entity is null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntityModel>>> GetAll()
    {
        return Ok(await _entityManager.GetAllAsync());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(EntityModel entityModel)
    {
        return await _entityManager.CreateAsync(entityModel);
    }

    [Authorize]
    [HttpDelete("{entityId}")]
    public async Task<ActionResult> Delete(int entityId)
    {
        await _entityManager.DeleteAsync(entityId);
        return Ok();
    }
}