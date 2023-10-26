using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using rose_api.Controllers;
using rose_api.ExternalServices.Services;
using rose_api.Models;

namespace rose_api.Tests
{
    [TestClass]
    public class ClimaControllerTests
    {
        [TestMethod]
        public async Task TestBuscarClimaPorCidade_Success()
        {
            string cidade = "Ponta Grossa";

            var mockBrasilApi = new Mock<BrasilApi>();
            var mockLogger = new Mock<ILogger<ClimaController>>();
            var mockDapperRepository = new Mock<DapperRepository>();

            mockBrasilApi
            .Setup(api => api.GetCidadePorNome(cidade))
            .ReturnsAsync(GetCidadeTest(cidade));

            var controller = new ClimaController(mockDapperRepository.Object, mockBrasilApi.Object, mockLogger.Object);

            var result = await controller.BuscarClimaPorCidade(cidade);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var cidadeRetornada = okResult.Value as Cidade;

            Assert.IsNotNull(cidadeRetornada);
            Assert.AreEqual(cidade, cidadeRetornada.CidadeNome);
        }

        [TestMethod]
        public async Task TestBuscarClimaPorCodigoCidade_Success()
        {
            int codigoCidade = 4090; // Um exemplo de código de cidade

            var mockBrasilApi = new Mock<BrasilApi>();
            var mockLogger = new Mock<ILogger<ClimaController>>();
            var mockDapperRepository = new Mock<DapperRepository>();

            // Configurar o mock da BrasilApi para simular os dados do clima do aeroporto
            mockBrasilApi
                .Setup(api => api.GetClimaPorCodigoCidade(codigoCidade))
                .ReturnsAsync(GetClimaCidadeTest());

            var controller = new ClimaController(mockDapperRepository.Object, mockBrasilApi.Object, mockLogger.Object);

            // Chamar o método de busca de clima por código de cidade
            var result = await controller.BuscarClimaPorCodigoCidade(codigoCidade);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var climaCidade = okResult.Value as ClimaCidade;

            Assert.IsNotNull(climaCidade);
        }


        [TestMethod]
        public async Task TestBuscarClimaPorAeroporto_Success()
        {
            string icaoCode = "SBAR";

            var mockBrasilApi = new Mock<BrasilApi>();
            var mockLogger = new Mock<ILogger<ClimaController>>();
            var mockDapperRepository = new Mock<DapperRepository>();

            mockBrasilApi
                .Setup(api => api.GetClimaPorAeroporto(icaoCode))
                .ReturnsAsync(GetClimaAeroportoTest(icaoCode));

            var controller = new ClimaController(mockDapperRepository.Object, mockBrasilApi.Object, mockLogger.Object);

            var result = await controller.BuscarClimaPorAeroporto(icaoCode);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var climaAeroportoRetornado = okResult.Value as ClimaAeroporto;

            Assert.IsNotNull(climaAeroportoRetornado);
        }

        [TestMethod]
        public async Task TestBuscarClimaPorCidade_NotFound()
        {
            string cidade = "CidadeInexistente";

            var mockBrasilApi = new Mock<BrasilApi>();
            var mockLogger = new Mock<ILogger<ClimaController>>();
            var mockDapperRepository = new Mock<DapperRepository>();

            mockBrasilApi
                .Setup(api => api.GetCidadePorNome(cidade))
                .ReturnsAsync((Cidade)null);

            var controller = new ClimaController(mockDapperRepository.Object, mockBrasilApi.Object, mockLogger.Object);

            var result = await controller.BuscarClimaPorCidade(cidade);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestBuscarClimaPorCodigoCidade_NotFound()
        {
            int codigoCidade = 4090;

            var mockBrasilApi = new Mock<BrasilApi>();
            var mockLogger = new Mock<ILogger<ClimaController>>();
            var mockDapperRepository = new Mock<DapperRepository>();

            mockBrasilApi
                .Setup(api => api.GetClimaPorCodigoCidade(codigoCidade))
                .ReturnsAsync((ClimaCidade)null);

            var controller = new ClimaController(mockDapperRepository.Object, mockBrasilApi.Object, mockLogger.Object);

            var result = await controller.BuscarClimaPorCodigoCidade(codigoCidade);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestBuscarClimaPorAeroporto_NotFound()
        {
            string icaoCode = "CódigoIcaoInexistente";

            var mockBrasilApi = new Mock<BrasilApi>();
            var mockLogger = new Mock<ILogger<ClimaController>>();
            var mockDapperRepository = new Mock<DapperRepository>();

            mockBrasilApi
                .Setup(api => api.GetClimaPorAeroporto(icaoCode))
                .ReturnsAsync((ClimaAeroporto)null);

            var controller = new ClimaController(mockDapperRepository.Object, mockBrasilApi.Object, mockLogger.Object);

            var result = await controller.BuscarClimaPorAeroporto(icaoCode);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        private static Cidade GetCidadeTest(string cidade)
        {
            return new Cidade
            {
                Id = 1,
                CidadeNome = cidade,
                Estado = "PR",
                AtualizadoEm = DateTime.Parse("2023-10-25"),
                Clima = new List<ClimaCidade>
        {
            new ClimaCidade
            {
                Data = DateTime.Parse("2023-10-26"),
                Condicao = "c",
                CondicaoDesc = "Chuva",
                Min = 17,
                Max = 23,
                IndiceUv = 13
            }
        }
            };
        }
        private static ClimaCidade GetClimaCidadeTest()
        {
            return new ClimaCidade
            {
                Data = DateTime.Parse("2023-10-26"),
                Condicao = "c",
                CondicaoDesc = "Chuva",
                Min = 17,
                Max = 23,
                IndiceUv = 13
            };
        }
        private static ClimaAeroporto GetClimaAeroportoTest(string icaoCode)
        {
            return new ClimaAeroporto
            {
                CodigoIcao = icaoCode,
                AtualizadoEm = DateTime.Parse("2023-10-25T21:00:00.097Z"),
                PressaoAtmosferica = "1010",
                Visibilidade = ">10000",
                Vento = 18,
                DirecaoVento = 60,
                Umidade = 79,
                Condicao = "ps",
                CondicaoDesc = "Predomínio de Sol",
                Temp = 27
            };
        }

    }
}
