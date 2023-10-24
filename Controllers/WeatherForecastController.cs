using Microsoft.AspNetCore.Mvc;

namespace rose_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly DapperRepository _dapperRepository;

        public WeatherForecastController(DapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        [HttpGet("testar-conexao", Name = "TestarConexao")]
        public IActionResult TestarConexao()
        {
            bool isConnected = _dapperRepository.TestConnection();

            if (isConnected)
            {
                return Ok("Conexão com o banco de dados bem-sucedida.");
            }
            else
            {
                return BadRequest("Falha na conexão com o banco de dados.");
            }
        }
    }
}
