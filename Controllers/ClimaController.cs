using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rose_api.ExternalServices.Services;
using rose_api.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace rose_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClimaController : ControllerBase
    {
        private readonly DapperRepository _dapperRepository;
        private readonly BrasilApi _brasilApi;
        private string _endPoint;
        private readonly ILogger<ClimaController> _logger;

        public ClimaController(DapperRepository dapperRepository, BrasilApi brasilApi, ILogger<ClimaController> logger)
        {
            _dapperRepository = dapperRepository;
            _brasilApi = brasilApi;
            _logger = logger;
            _endPoint = "";
        }

        private LogRequisition CreateLogRequisition(string cidade, string clima, string clientIp)
        {
            return new LogRequisition
            {
                EndPoint = _endPoint,
                Parameters = cidade,
                Data = clima,
                Ip = clientIp,
                CreatedDate = DateTime.Now
            };
        }

        private LogError CreateLogError(string cidade, string errorMessage, string clientIp)
        {
            return new LogError
            {
                EndPoint = _endPoint,
                Parameters = cidade,
                Error = errorMessage,
                Ip = clientIp,
                CreatedDate = DateTime.Now
            };
        }

        [HttpGet("testar-conexao", Name = "TestarConexao")]
        [SwaggerOperation(Summary = "Testar a conexão com o banco de dados", Description = "Este endpoint testa a conexão com o banco de dados.")]
        [SwaggerResponse(200, "Conexão bem-sucedida")]
        [SwaggerResponse(400, "Falha na conexão")]
        public IActionResult TestarConexao()
        {
            _endPoint = "clima/testar-conexao";

            bool isConnected = _dapperRepository.TestConnection();

            if (isConnected)
            {
                return Ok("Conexão com o banco de dados bem-sucedida.");
            }
            else
            {
                _logger.LogError("Falha na conexão com o banco de dados.");
                return BadRequest("Falha na conexão com o banco de dados.");
            }
        }

        [HttpGet("buscar-por-cidade/{cidade}")]
        [SwaggerOperation(Summary = "Buscar informações climáticas por cidade", Description = "Este endpoint recupera informações climáticas com base no nome da cidade.")]
        [SwaggerResponse(200, "Informações climáticas recuperadas com sucesso", typeof(Cidade))]
        [SwaggerResponse(400, "Solicitação inválida")]
        [SwaggerResponse(404, "Cidade não encontrada ou erro ao buscar o clima")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> BuscarClimaPorCidade(
            [FromRoute]
            [SwaggerParameter("Nome da cidade")]
            string cidade)
        {
            _endPoint = "clima/buscar-por-cidade";

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            string clientIp = remoteIpAddress?.MapToIPv4()?.ToString() ?? "Endereço IP não detectado";

            try
            {
                string cidadeCodificada = cidade;
                if (cidade.Any(ch => WebUtility.UrlEncode(ch.ToString()) != ch.ToString()))
                {
                    cidadeCodificada = WebUtility.UrlEncode(cidade);
                }

                Cidade? brasil_cidade = await _brasilApi.GetCidadePorNome(cidadeCodificada);

                if (brasil_cidade != null)
                {
                    var clima = await _brasilApi.GetClimaPorCodigoCidade(brasil_cidade.Id);

                    if (clima != null)
                    {
                        LogRequisition logRequisition = CreateLogRequisition(cidade, clima, clientIp);
                        _dapperRepository.InsertLogRequisition(logRequisition);
                        return Ok(clima);
                    }
                }

                LogError logError = CreateLogError(cidade, "Cidade não encontrada ou erro ao buscar o clima.", clientIp);
                _dapperRepository.InsertLogError(logError);

                _logger.LogError("Cidade não encontrada ou erro ao buscar o clima.");
                return NotFound("Cidade não encontrada ou erro ao buscar o clima.");
            }
            catch (Exception ex)
            {
                LogError logError = CreateLogError(cidade, ex.Message, clientIp);
                _dapperRepository.InsertLogError(logError);

                _logger.LogError(ex, "Erro ao buscar clima.");
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpGet("buscar-por-codigo-cidade/{codigoCidade}")]
        [SwaggerOperation(Summary = "Buscar informações climáticas por código de cidade", Description = "Este endpoint recupera informações climáticas com base no código da cidade.")]
        [SwaggerResponse(200, "Informações climáticas recuperadas com sucesso", typeof(Cidade))]
        [SwaggerResponse(400, "Solicitação inválida")]
        [SwaggerResponse(404, "Cidade não encontrada ou erro ao buscar o clima")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> BuscarClimaPorCodigoCidade(
            [FromRoute]
            [SwaggerParameter("Código da cidade")]
            int codigoCidade)
        {
            _endPoint = "clima/buscar-por-codigo-cidade";

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            string clientIp = remoteIpAddress?.MapToIPv4()?.ToString() ?? "Endereço IP não detectado";

            try
            {
                var clima = await _brasilApi.GetClimaPorCodigoCidade(codigoCidade);

                if (!string.IsNullOrEmpty(clima))
                {
                    LogRequisition logRequisition = CreateLogRequisition(codigoCidade.ToString(), clima, clientIp);
                    _dapperRepository.InsertLogRequisition(logRequisition);
                    return Ok(clima);
                }

                LogError logError = CreateLogError(codigoCidade.ToString(), "Cidade não encontrada ou erro ao buscar o clima.", clientIp);
                _dapperRepository.InsertLogError(logError);

                _logger.LogError("Cidade não encontrada ou erro ao buscar o clima.");
                return NotFound("Cidade não encontrada ou erro ao buscar o clima.");
            }
            catch (Exception ex)
            {
                LogError logError = CreateLogError(codigoCidade.ToString(), ex.Message, clientIp);
                _dapperRepository.InsertLogError(logError);

                _logger.LogError(ex, "Erro ao buscar clima.");
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }

        [HttpGet("aeroporto/{icaoCode}")]
        [SwaggerOperation(Summary = "Buscar informações climáticas por código ICAO do aeroporto", Description = "Este endpoint recupera informações climáticas com base no código ICAO do aeroporto.")]
        [SwaggerResponse(200, "Informações climáticas recuperadas com sucesso", typeof(ClimaAeroporto))]
        [SwaggerResponse(400, "Solicitação inválida")]
        [SwaggerResponse(404, "Aeroporto não encontrado ou erro ao buscar o clima")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> BuscarClimaPorAeroporto(
            [FromRoute]
            [SwaggerParameter("Código ICAO (4 dígitos) do aeroporto desejado")] 
            string icaoCode)
        {
            _endPoint = "clima/aeroporto";

            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            string clientIp = remoteIpAddress?.MapToIPv4()?.ToString() ?? "Endereço IP não detectado";

            try
            {
                var climaAeroporto = await _brasilApi.GetClimaPorAeroporto(icaoCode);

                if (climaAeroporto != null)
                {
                    LogRequisition logRequisition = CreateLogRequisition(icaoCode, JsonConvert.SerializeObject(climaAeroporto), clientIp);
                    _dapperRepository.InsertLogRequisition(logRequisition);
                    return Ok(climaAeroporto);
                }

                LogError logError = CreateLogError(icaoCode, "Aeroporto não encontrado ou erro ao buscar o clima.", clientIp);
                _dapperRepository.InsertLogError(logError);

                _logger.LogError("Aeroporto não encontrado ou erro ao buscar o clima.");
                return NotFound("Aeroporto não encontrado ou erro ao buscar o clima.");
            }
            catch (Exception ex)
            {
                LogError logError = CreateLogError(icaoCode, ex.Message, clientIp);
                _dapperRepository.InsertLogError(logError);

                _logger.LogError(ex, "Erro ao buscar clima do aeroporto.");
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }
    }
}