using System.Collections.Generic;

namespace ListaTelefônica
{
    public interface IPessoaRepository
    {
        List<Pessoa> Listar();
        Pessoa Obter(int id);
        void Adicionar(Pessoa pessoa);
    }
}
