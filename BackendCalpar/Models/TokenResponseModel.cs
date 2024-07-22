using Newtonsoft.Json;

namespace BackendCalpar.Models
{
    public class TokenResponseModel
    {
        public TokenResponseModel()
        {
            this.NomeUsuario = string.Empty;
            this.Token = string.Empty;
        }

        [JsonProperty("idUsuario")]
        public uint IdUsuario { get; set; }
        [JsonProperty("nomeUsuario")]
        public string NomeUsuario { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
