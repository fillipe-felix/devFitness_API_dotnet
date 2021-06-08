using System.Linq;
using AutoMapper;
using DevFitness.API.Core.Entities;
using DevFitness.API.Models.InputModels;
using DevFitness.API.Models.ViewModels;
using DevFitness.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevFitness.API.Controllers
{
    // api/users
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DevFitnessDbContext _dbContext;
        private readonly IMapper _mapper;

        public UsersController(DevFitnessDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Retornar detalhes de usuário
        /// </summary>
        /// <param name="id">Identificador de usuário</param>
        /// <returns>Objeto de detalhe de usuários</returns>
        /// <response code="404">Usuário não encontrado</response>
        /// <response code="200">Usuário encontrado com sucesso</response>
        // api/users/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            //var userViewModel = new UserViewModel(user.Id, user.FullName, user.Height, user.Weight, user.BirthDate);
            var userViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(userViewModel);
        }

        /// <summary>
        /// Cadastrar um usario
        /// </summary>
        /// <param name="inputModel">Objeto com dados de cadastro de usuario</param>
        /// <remarks>
        /// Requisição de Exemplo:
        /// {
        /// "fullName": "Fillipe",
        /// "height": 1.70,
        /// "weight": 56,
        /// "birthDate": "1993-07-18 00:00:00"
        /// }
        /// </remarks>
        /// <returns>Objeto recem-criado.</returns>
        /// <response code"201">Objeto criado com sucesso</response>
        /// <response code"400">Dados invalidos</response>
        // api/users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] CreateUserInputModel inputModel)
        {
            //var user = new User(inputModel.FullName, inputModel.Height, inputModel.Weight, inputModel.BirthDate);
            var user = _mapper.Map<User>(inputModel);

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