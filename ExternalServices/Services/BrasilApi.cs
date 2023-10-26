using rose_api.Models;
using Newtonsoft.Json;

namespace rose_api.ExternalServices.Services
{
    public class BrasilApi
    {
        private readonly HttpClient _httpClient;
        private readonly DapperRepository _dapperRepository;
        private const string BaseUrl = "https://brasilapi.com.br/api/cptec/v1/";
        private readonly ILogger<BrasilApi> _logger;

        public BrasilApi(HttpClient httpClient, DapperRepository dapperRepository, ILogger<BrasilApi> logger)
        {
            _httpClient = httpClient;
            _dapperRepository = dapperRepository;
            _logger = logger;
        }

        public async Task<string> GetClimaPorCodigoCidade(int codigo_cidade)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}clima/previsao/{codigo_cidade}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                var errorMessage = $"Requisição HTTP falhou, código: {response.StatusCode}";
                LogError("GetClimaPorCodigoCidade", errorMessage);
                return "";
            }
            catch (Exception ex)
            {
                LogError("GetClimaPorCodigoCidade", ex.Message);
                return "";
            }
        }

        public async Task<Cidade?> GetCidadePorNome(string nome_cidade)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}cidade/{nome_cidade}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var cidades = JsonConvert.DeserializeObject<List<Cidade>>(json);

                    return cidades?[0];
                }

                var errorMessage = $"Requisição HTTP falhou, código: {response.StatusCode}";
                LogError("GetCidadePorNome", errorMessage);
                return null;
            }
            catch (Exception ex)
            {
                LogError("GetCidadePorNome", ex.Message);
                return null;
            }
        }

        public async Task<ClimaAeroporto?> GetClimaPorAeroporto(string icaoCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}clima/aeroporto/{icaoCode}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var climaAeroporto = JsonConvert.DeserializeObject<ClimaAeroporto>(json);

                    if (climaAeroporto != null)
                    {
                        return climaAeroporto;
                    }
                }

                var errorMessage = $"Requisição HTTP falhou, código: {response.StatusCode}";
                LogError("GetClimaPorAeroporto", errorMessage);
                return null;
            }
            catch (Exception ex)
            {
                LogError("GetClimaPorAeroporto", ex.Message);
                return null;
            }
        }

        private void LogError(string endPoint, string error)
        {
            _logger.LogError($"Error em {endPoint}: {error}");

            var logError = new LogError
            {
                EndPoint = endPoint,
                Error = error,
                CreatedDate = DateTime.Now
            };
            _dapperRepository.InsertLogError(logError);
        }
    }
}
