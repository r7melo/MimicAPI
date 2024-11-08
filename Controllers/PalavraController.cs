using Microsoft.AspNetCore.Mvc;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavraController : Controller
    {
        private readonly IPalavraRepository _repository;

        public PalavraController(IPalavraRepository banco)
        {
            _repository = banco;
        }

        // APP -- /api/palavras (GET)
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas(DateTime? data)
        {
            List<Palavra> palavras = _repository.ObterPalavras(data);

            return Ok(palavras);
        }

        // WEB -- /api/palavras/1 (GET)
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(long id)
        {
            Palavra palavra = _repository.Obter(id);

            if (palavra == null || !palavra.Ativo)
                return NotFound();

            return Ok(palavra);
        }

        // WEB -- /api/palavras (POST)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody]  Palavra palavra)
        {
            long id = _repository.Cadastrar(palavra);

            palavra = _repository.Obter(id);

            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        // WEB -- /api/palavras/1 (PUT)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(long id, [FromBody] Palavra nova_palavra)
        {
            Palavra palavra = _repository.Obter(id);

            if (palavra == null || !palavra.Ativo)
                return NotFound();

            nova_palavra.Id = palavra.Id;

            _repository.Atualizar(nova_palavra);

            return Ok();
        }

        // WEB -- /api/palavras/1 (DELETE)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(long id)
        {
            Palavra p_aux = _repository.Obter(id);

            if (p_aux == null || !p_aux.Ativo)
                return NotFound();

            _repository.Deletar(id);

            return NoContent();
        }
    }
}
