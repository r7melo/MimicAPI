using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;
using MimicAPI.Models;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavraController : Controller
    {
        private readonly MimicContext _banco;

        public PalavraController(MimicContext banco)
        {
            _banco = banco;
        }

        // APP -- /api/palavras (GET)
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas(DateTime? data)
        {
            var palavras = _banco.Palavras.AsQueryable();
            palavras = palavras.Where(p => p.Ativo == true);

            if (data.HasValue)
            {
                palavras = palavras.Where(p => p.Criado > data.Value);
            }

            return Ok(palavras);
        }

        // WEB -- /api/palavras/1 (GET)
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(long id)
        {
            Palavra palavra = _banco.Palavras.Find(id);

            if (palavra == null || !palavra.Ativo)
                return NotFound();

            return Ok(palavra);
        }

        // WEB -- /api/palavras (POST)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]  Palavra palavra)
        {
            palavra.Criado = DateTime.Now;

            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();

            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        // WEB -- /api/palavras/1 (PUT)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(long id, [FromBody] Palavra nova_palavra)
        {
            Palavra palavra = _banco.Palavras.Find(id);

            if (palavra == null || !palavra.Ativo)
                return NotFound();

            palavra.Nome = nova_palavra.Nome;
            palavra.Pontuacao = nova_palavra.Pontuacao;
            palavra.Ativo = nova_palavra.Ativo;
            palavra.Atualizacao = DateTime.Now;

            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

            return Ok(palavra);
        }

        // WEB -- /api/palavras/1 (DELETE)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(long id)
        {
            Palavra palavra = _banco.Palavras.Find(id);

            if (palavra == null || !palavra.Ativo)
                return NotFound();

            palavra.Ativo = false;

            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

            return NoContent();
        }
    }
}
