using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using BackendCalpar.Data;
using BackendCalpar.Models;
using System.Collections.Generic;

namespace BackendCalpar.Services
{
    public class ApiService(HttpClient httpClient, AppDbContext context, IConfiguration configuration)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> FetchAndStoreUsersAsync()
        {
            var response = await _httpClient.GetStringAsync(_configuration["ExternalApi:Url"]);
            var jsonResponse = JObject.Parse(response);

            if (jsonResponse is null || 
                !jsonResponse.TryGetValue("Msg", out var msgToken) ||
                msgToken.Type != JTokenType.String ||
                msgToken.ToString() != "Sucesso ao Encontrar usuário.")
            {
                return "Erro ao consumir a API";
            }

            if (!jsonResponse.TryGetValue("Dados", out var dadosToken) || dadosToken.Type != JTokenType.Array )
            {
                return "Erro ao converter os dados da API";
            }

            var users = dadosToken.ToObject<List<User>>();
            if (users == null)
            {
                return "Erro ao converter os dados da API";
            }

            foreach (var user in users)
            {
                if (!_context.Users.Any(u => u.Nome == user.Nome))
                {
                    _context.Users.Add(user);
                }
            }
            await _context.SaveChangesAsync();

            return "Dados armazenados com sucesso";
        }
    }
}
