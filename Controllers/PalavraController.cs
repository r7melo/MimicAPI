using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
using MimicAPI.Repositories.Contracts;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavraController : Controller
    {
        private readonly IPalavraRepository _repository;
        private readonly IMapper _mapper;

        public PalavraController(IPalavraRepository banco, IMapper mapper)
        {
            _repository = banco;
            _mapper = mapper;
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
        [HttpGet("{id}", Name = "ObterPalavra")]
        public ActionResult Obter(long id)
        {
            Palavra palavra = _repository.Obter(id);

            if (palavra == null || !palavra.Ativo)
                return NotFound();

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links = new List<LinkDTO>()
            {
                new LinkDTO("self", Url.Link("ObterPalavra", new {id = palavraDTO.Id }), "GET"),
                new LinkDTO("update", Url.Link("AtualizarPalavra", new {id = palavraDTO.Id }), "PUT"),
                new LinkDTO("delete", Url.Link("ExcluirPalavra", new {id = palavraDTO.Id }), "DELETE"),
            };

            return Ok(palavraDTO);
        }

        // WEB -- /api/palavras (POST)
        [HttpPost("{id}", Name = "CadastrarPalavra")]
        public ActionResult Cadastrar([FromBody]  Palavra palavra)
        {
            long id = _repository.Cadastrar(palavra);

            palavra = _repository.Obter(id);

            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        // WEB -- /api/palavras/1 (PUT)
        [HttpPut("{id}", Name = "AtualizarPalavra")]
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
        [HttpDelete("{id}", Name = "ExcluirPalavra")]
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
