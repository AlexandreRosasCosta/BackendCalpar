using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendCalpar.Data;
using BackendCalpar.Models;
using BackendCalpar.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendCalpar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("/health")]
        [SwaggerOperation(Summary = "Verifica a saúde da API.", Description = "Retorna uma mensagem indicando que a API está funcionando.")]
        [SwaggerResponse(200, "API está funcionando!")]
        public IActionResult GetHealth()
        {
            return Ok(new { message = "API está funcionando!" });
        }
        private readonly AppDbContext _context;
        private readonly ApiService _apiService;

        public UserController(AppDbContext context, ApiService apiService)
        {
            this._context = context;
            this._apiService = apiService;
        }
        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        /// <remarks>
        /// Exemplo de resposta:
        ///
        ///     GET /api/user
        ///     [
        ///        {
        ///           "id": 1,
        ///           "nome": "João",
        ///           "disponivel": true
        ///        }
        ///     ]
        ///
        /// </remarks>
        /// <returns>Uma lista de usuários.</returns>
        /// <response code="200">Retorna a lista de usuários</response>
        /// <response code="401">Não autorizado</response>
        [HttpGet]
        [Authorize]
        [SwaggerResponse(200, "Retorna a lista de usuários", typeof(IEnumerable<User>))]
        [SwaggerResponse(401, "Não autorizado")]
        public async Task<ActionResult<User>> GetUsers()
        {
            var user = await _context.Users.ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Obtém um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do usuário.</param>
        /// <returns>O usuário correspondente ao ID fornecido.</returns>
        /// <response code="200">Retorna o usuário</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Obtém um usuário específico pelo ID.", Description = "Retorna o usuário correspondente ao ID fornecido.")]
        [SwaggerResponse(200, "Retorna o usuário", typeof(User))]
        [SwaggerResponse(401, "Não autorizado")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="user">Os dados do usuário a ser criado.</param>
        /// <returns>O usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Cria um novo usuário.", Description = "Cria um novo usuário com os dados fornecidos.")]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(User))]
        [SwaggerResponse(401, "Não autorizado")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">O ID do usuário a ser atualizado.</param>
        /// <param name="user">Os novos dados do usuário.</param>
        /// <response code="204">Usuário atualizado com sucesso</response>
        /// <response code="400">ID do usuário não corresponde ao ID fornecido</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpPut("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Atualiza um usuário existente.", Description = "Atualiza um usuário existente com os novos dados fornecidos.")]
        [SwaggerResponse(204, "Usuário atualizado com sucesso")]
        [SwaggerResponse(400, "ID do usuário não corresponde ao ID fornecido")]
        [SwaggerResponse(401, "Não autorizado")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Exclui um usuário pelo ID.
        /// </summary>
        /// <param name="id">O ID do usuário a ser excluído.</param>
        /// <response code="204">Usuário excluído com sucesso</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Exclui um usuário pelo ID.", Description = "Exclui um usuário existente pelo ID fornecido.")]
        [SwaggerResponse(204, "Usuário excluído com sucesso")]
        [SwaggerResponse(401, "Não autorizado")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Busca e armazena usuários a partir de uma API externa.
        /// </summary>
        /// <response code="200">Dados armazenados com sucesso</response>
        /// <response code="400">Erro ao consumir a API</response>
        /// <response code="401">Não autorizado</response>
        [HttpPost("fetch-users")]
        [Authorize]
        [SwaggerOperation(Summary = "Busca e armazena usuários a partir de uma API externa.", Description = "Busca e armazena usuários na base de dados a partir de uma API externa.")]
        [SwaggerResponse(200, "Dados armazenados com sucesso")]
        [SwaggerResponse(400, "Erro ao consumir a API")]
        [SwaggerResponse(401, "Não autorizado")]
        public async Task<IActionResult> FetchAndStoreusers()
        {
            var result = await _apiService.FetchAndStoreUsersAsync();
            if(result != "Dados armazenados com sucesso")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Gera um token de autenticação.
        /// </summary>
        /// <returns>O token de autenticação gerado.</returns>
        [HttpGet("auth/get-token")]
        [SwaggerOperation(Summary = "Gera um token de autenticação.", Description = "Gera um token de autenticação que pode ser usado para acessar endpoints protegidos.")]
        [SwaggerResponse(200, "Token gerado com sucesso")]
        public IActionResult GetToken() => Ok(TokenServices.GenerateToken());
    }
}
