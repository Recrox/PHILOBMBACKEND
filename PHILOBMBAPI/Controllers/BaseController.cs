using Microsoft.AspNetCore.Mvc;
using PHILOBMBusiness.Models.Base;
using PHILOBMBusiness.Services.Interfaces;

namespace PHILOBMBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T, TService> : ControllerBase
        where T : BaseEntity
        where TService : IBaseService<T>
    {
        protected readonly TService _service;

        protected BaseController(TService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return HandleResult(items, "Aucun élément trouvé.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return HandleResult(item, "Élément non trouvé.");
        }

        [HttpPost]
        public async Task<ActionResult<T>> Create(T entity)
        {
            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, T entity)
        {
            if (id != entity.Id)
            {
                return BadRequest("L'ID de l'entité ne correspond pas.");
            }

            await _service.UpdateAsync(entity);
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool deleted = await _service.DeleteAsync(id);

            if (deleted)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                return NotFound(); // 404 Not Found si l'entité n'existe pas
            }
        }


        protected ActionResult<T> HandleResult(T? result, string? notFoundMessage = null)
        {
            if (result == null)
            {
                return NotFound(notFoundMessage ?? "L'élément demandé n'a pas été trouvé.");
            }
            return Ok(result);
        }

        protected ActionResult<IEnumerable<T>> HandleResult(ICollection<T> result, string? notFoundMessage = null)
        {
            if (result == null)
            {
                return NotFound(notFoundMessage ?? "Une erreur s'est produite lors de la récupération des éléments.");
            }
            return Ok(result);
        }
    }
}
