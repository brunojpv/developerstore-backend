using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Api.Controllers
{
    /// <summary>
    /// Controlador responsável por operações de vendas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        /// <param name="request">Dados da venda</param>
        /// <returns>Venda criada</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SaleRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);
            return NoContent();
        }
    }
}
