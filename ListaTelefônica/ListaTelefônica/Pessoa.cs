using System.Collections.Generic;

namespace ListaTelefônica
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual List<Telefone> Telefones { get; set; }
    }
}
