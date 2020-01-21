using System.Threading.Tasks;
using fansoft.cosmosdb.cadcli.core;
using Microsoft.AspNetCore.Mvc;
using fansoft.cosmosdb.cadcli.api.Models;

namespace fansoft.cosmosdb.cadcli.api.Controllers
{
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IRepository _repository;
        public ClientesController(IRepository repository) => _repository = repository;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _repository.GetAllClientesAsync();
            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Models.Cliente model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = model.ToData();
            var response = await _repository.AddItemsToContainerAsync(data);
            return Ok(response);
        }
    }
}