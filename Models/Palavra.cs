using System;

namespace MimicAPI.Models
{
    public class Palavra
    {
        public long Id { get; set; }
        public string Nome { get; set; }

        public int Pontuacao { get; set; }
        public bool Ativo {  get; set; }
        public DateTime Criado { get; set; }
        public DateTime? Atualizacao { get; set; }
    }
}
