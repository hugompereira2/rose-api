using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace rose_api.Models
{
    public class ClimaAeroporto
    {
        [JsonProperty("codigo_icao")]
        [SwaggerSchema("Código ICAO do aeroporto.")]
        public string CodigoIcao { get; set; }

        [JsonProperty("atualizado_em")]
        [SwaggerSchema("Data e hora da última atualização.")]
        public DateTime AtualizadoEm { get; set; }

        [JsonProperty("pressao_atmosferica")]
        [SwaggerSchema("Pressão atmosférica em hPa.")]
        public string PressaoAtmosferica { get; set; }

        [JsonProperty("visibilidade")]
        [SwaggerSchema("Visibilidade em metros.")]
        public string Visibilidade { get; set; }

        [JsonProperty("vento")]
        [SwaggerSchema("Velocidade do vento em km/h.")]
        public int Vento { get; set; }

        [JsonProperty("direcao_vento")]
        [SwaggerSchema("Direção do vento em graus.")]
        public int DirecaoVento { get; set; }

        [JsonProperty("umidade")]
        [SwaggerSchema("Umidade do ar em porcentagem.")]
        public int Umidade { get; set; }

        [JsonProperty("condicao")]
        [SwaggerSchema("Código da condição do clima.")]
        public string Condicao { get; set; }

        [JsonProperty("condicao_desc")]
        [SwaggerSchema("Descrição da condição do clima.")]
        public string CondicaoDesc { get; set; }

        [JsonProperty("temp")]
        [SwaggerSchema("Temperatura em graus Celsius.")]
        public int Temp { get; set; }
    }
}
