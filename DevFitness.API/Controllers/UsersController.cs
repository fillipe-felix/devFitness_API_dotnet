using DevFitness.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevFitness.API.Controllers
{
    // api/users
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // api/users/1
        [HttpGet("{id}")]
        public IActionResult Get( int id)
        {
            return Ok();
        }
        
        // api/users
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserInputModel inputModel)
        {

            // isso daqui retorna o id no location do header da requisição
            return CreatedAtAction(nameof(Get), new {id = 10}, inputModel);
        }
        
        // aí/users/4
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserInputModel inputModel)
        {

            return NoContent();
        }
    }
}