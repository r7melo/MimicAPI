using MimicAPI.Database;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;

namespace MimicAPI.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;

        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }
        public List<Palavra> ObterPalavras(DateTime? data)
        {
            var palavras = _banco.Palavras.AsQueryable();
            palavras = palavras.Where(p => p.Ativo == true);

            if (data.HasValue)
            {
                palavras = palavras.Where(p => p.Criado > data.Value);
            }

            return palavras.ToList();
        }

        public Palavra Obter(long id)
        {
            Palavra palavra = _banco.Palavras.Find(id);

            return palavra;
        }

        public long Cadastrar(Palavra palavra)
        {
            palavra.Criado = DateTime.Now;

            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();

            return palavra.Id;
        }

        public void Atualizar(Palavra nova_palavra)
        {
            Palavra palavra = _banco.Palavras.Find(nova_palavra.Id);

            palavra.Nome = nova_palavra.Nome;
            palavra.Pontuacao = nova_palavra.Pontuacao;
            palavra.Ativo = nova_palavra.Ativo;
            palavra.Atualizacao = DateTime.Now;

            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }

        public void Deletar(long id)
        {
            Palavra palavra = _banco.Palavras.Find(id);

            palavra.Ativo = false;

            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

        }

    }
}
