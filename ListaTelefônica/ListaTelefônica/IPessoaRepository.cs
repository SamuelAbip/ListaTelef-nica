using System.Collections.Generic;

namespace ListaTelefônica
{
    public interface IPessoaRepository
    {
        List<Pessoa> Listar();
        Pessoa Obter(int id);
        Pessoa Obter(string nome);
        void AtualizarTelefone(Pessoa pessoa);
        void Adicionar(Pessoa pessoa);
        void Remover(int id);
    }
}
