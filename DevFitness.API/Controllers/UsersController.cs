using System.Linq;
using DevFitness.API.Core.Entities;
using DevFitness.API.Models.InputModels;
using DevFitness.API.Models.ViewModels;
using DevFitness.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevFitness.API.Controllers
{
    // api/users
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DevFitnessDbContext _dbContext;

        public UsersController(DevFitnessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // api/users/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = new UserViewModel(user.Id, user.FullName, user.Height, user.Weight, user.BirthDate);

            return Ok(userViewModel);
        }

        // api/users
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserInputModel inputModel)
        {
            var user = new User(inputModel.FullName, inputModel.Height, inputModel.Weight, inputModel.BirthDate);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            
            // isso daqui retorna o id no location do header da requisição
            return CreatedAtAction(nameof(Get), new {id = user.Id}, inputModel);
        }

        // aí/users/4
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserInputModel inputModel)
        {

            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            
            user.Update(inputModel.Height, inputModel.Weight);

            _dbContext.SaveChanges();
            
            return NoContent();
        }
    }
}