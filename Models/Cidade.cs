using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace rose_api.Models
{
    public class Cidade
    {
        [JsonProperty("Id")]
        [SwaggerSchema("id da cidade.")]
        public int Id { get; set; }
        [SwaggerSchema("Nome da cidade.")]
        public string CidadeNome { get; set; }

        [JsonProperty("estado")]
        [SwaggerSchema("Estado da cidade.")]
        public string Estado { get; set; }

        [JsonProperty("atualizado_em")]
        [SwaggerSchema("Data e hora da última atualização.")]
        public DateTime AtualizadoEm { get; set; }

        [JsonProperty("clima")]
        [SwaggerSchema("Informações climáticas da cidade.")]
        public List<ClimaCidade> Clima { get; set; }
    }

    public class ClimaCidade
    {
        [JsonProperty("data")]
        [SwaggerSchema("Data da informação climática.")]
        public DateTime Data { get; set; }

        [JsonProperty("condicao")]
        [SwaggerSchema("Código da condição do clima.")]
        public string Condicao { get; set; }

        [JsonProperty("condicao_desc")]
        [SwaggerSchema("Descrição da condição do clima.")]
        public string CondicaoDesc { get; set; }

        [JsonProperty("min")]
        [SwaggerSchema("Temperatura mínima em graus Celsius.")]
        public int Min { get; set; }

        [JsonProperty("max")]
        [SwaggerSchema("Temperatura máxima em graus Celsius.")]
        public int Max { get; set; }

        [JsonProperty("indice_uv")]
        [SwaggerSchema("Índice UV.")]
        public int IndiceUv { get; set; }
    }

}
