using AluraRpa.Application.Services;
using AluraRpa.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;

namespace AluraRpa.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly CursoService _cursoService;

        public CursosController(CursoService cursoService)
        {
            _cursoService = cursoService;
        }
        [HttpPost]
        public async Task<IActionResult> ScrapeAlura([FromBody] BuscaDto buscaDto)
        {
            if (buscaDto == null || string.IsNullOrEmpty(buscaDto.TermoBusca))
            {
                return BadRequest("O termo de busca é obrigatório.");
            }

            var options = new ChromeOptions();
            options.AddArgument("--incognito"); // Modo anônimo
            options.AddArgument("--disable-extensions"); // Desabilitar extensões
            options.AddArgument("--disable-default-apps"); // Desabilitar apps padrão
            options.AddArgument("--user-data-dir=/tmp/user-data"); // Usar um diretório de dados temporário
            options.AddArgument("--profile-directory=Default"); // Não carregar perfis de usuário específicos

            try
            {
                using (var driver = new ChromeDriver(options))
                {
                    driver.Navigate().GoToUrl("https://www.alura.com.br/");

                    // Localiza o campo de busca e realiza a pesquisa
                    var searchBox = driver.FindElement(By.Id("header-barraBusca-form-campoBusca"));
                    searchBox.SendKeys(buscaDto.TermoBusca);
                    searchBox.Submit();

                    // Aguarda até que os resultados sejam carregados
                    System.Threading.Thread.Sleep(2000);

                    bool hasNextPage = true;

                    while (hasNextPage)
                    {
                        // Localiza todos os elementos da lista de resultados
                        var cursos = driver.FindElements(By.CssSelector("ul.paginacao-pagina > li.busca-resultado"));
                        foreach (var curso in cursos)
                        {
                            var titulo = curso.FindElement(By.CssSelector("h4.busca-resultado-nome")).Text;
                            var descricao = curso.FindElement(By.CssSelector("p.busca-resultado-descricao")).Text;
                            var link = curso.FindElement(By.CssSelector("a.busca-resultado-link")).GetAttribute("href");

                            // Armazena os dados extraídos
                            var cursoEntity = new Curso
                            {
                                Titulo = titulo,
                                Descricao = descricao,
                                Link = link
                            };

                            await _cursoService.AdicionarCursoAsync(cursoEntity);
                        }

                        // Verifica se o botão "Próximo" está presente e habilitado
                        try
                        {
                            var nextButton = driver.FindElement(By.LinkText("Próximo"));
                            if (nextButton.GetAttribute("class").Contains("desabilitado"))
                            {
                                hasNextPage = false; // Sai do loop se o botão "Próximo" estiver desabilitado
                            }
                            else
                            {
                                nextButton.Click(); // Clica no botão "Próximo" para ir para a próxima página
                                System.Threading.Thread.Sleep(2000); // Aguarda a próxima página carregar
                            }
                        }
                        catch (NoSuchElementException)
                        {
                            hasNextPage = false; // Sai do loop se o botão "Próximo" não for encontrado
                        }
                    }
                }


                //Falta implementar o entity para salvar no banco.. não tive muito tempo de fazer.. lotado de coisas aqui galera da Aec

                return Ok("Cursos adicionados com sucesso.");
            }
            catch (NoSuchElementException ex)
            {
                return StatusCode(500, $"Erro ao localizar um elemento: {ex.Message}");
            }
            catch (WebDriverException ex)
            {
                return StatusCode(500, $"Erro ao executar o WebDriver: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetCursos()
        {
            var cursos = await _cursoService.ObterCursosAsync();
            return Ok(cursos);
        }
    }
}
