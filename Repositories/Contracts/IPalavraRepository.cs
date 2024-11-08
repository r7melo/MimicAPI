using MimicAPI.Models;

namespace MimicAPI.Repositories.Contracts
{
    public interface IPalavraRepository
    {
        List<Palavra> ObterPalavras(DateTime? data);
        Palavra Obter(long id);
        long Cadastrar(Palavra palavra);
        void Atualizar(Palavra palavra);
        void Deletar(long id);
    }
}
