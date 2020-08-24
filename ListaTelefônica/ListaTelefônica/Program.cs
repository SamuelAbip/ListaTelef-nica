using System;
using System.Collections.Generic;
using System.Linq;

namespace ListaTelefônica
{
    class Program
    {
        private static List<string> Opções { get; } = new List<string>
        {
            "0 - Sair do programa.",
            "1 - Adicionar pessoa.",
            "2 - Alterar número.",
            "3 - Remover pessoa.",
            "4 - Imprimir lista telefônica.",
            "5 - Salvar lista telefônica."
        };

        private const string CaminhoLista = @"C:\ListaTelefônica\lista.txt";
        private const string ConnectionString = "Server=localhost;Database=ListaTelefonica;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            IPessoaRepository repositorio = new PessoaRepository(ConnectionString);
            var samuel = repositorio.Obter(1);
            Console.WriteLine($"{samuel.Nome}: {samuel.Telefones.First().Numero}");

            return;
            string[] teste = new string[] {"a", "b", "c", "d", "e"};
            int i = Array.IndexOf(teste, "a");
            string newteste = teste[i+2];
            var coleção = new ListaTelefonica(CaminhoLista);
            Console.WriteLine("Bem-vindo ao construtor de lista telefônica. Escolha uma das opções:");
            while (true)
            {
                ImprmirOpções();
                var opção = LerOpção();
                RealizarAção(opção, coleção);
                if (opção == ListaOpções.Sair)
                {
                    Console.WriteLine("Saindo do programa.");
                    break;
                }
            }
        }

        private static void ImprmirOpções()
        {
            foreach (var opção in Opções)
            {
                Console.WriteLine(opção);
            }
        }

        private static ListaOpções LerOpção()
        {
            string opçãoEscolhidaString = Console.ReadLine();
            bool opçãoÉVálida = Enum.TryParse(opçãoEscolhidaString, out ListaOpções opçãoEscolhida) &&
                                Enum.IsDefined(typeof(ListaOpções), opçãoEscolhida);
            if (!opçãoÉVálida)
            {
                Console.WriteLine("A opção digitada é inválida. Escolha entre as opções abaixo:");
                return ListaOpções.Inválida;
            }
            else return opçãoEscolhida;
        }

        private static void RealizarAção(ListaOpções opção, ListaTelefonica lista)
        {
            switch (opção)
            {
                case ListaOpções.Sair:
                    break;
                case ListaOpções.AdicionarPessoa:
                    AdicionarPessoas(lista);
                    break;
                case ListaOpções.AlterarNúmero:
                    AlterarNúmero(lista);
                    break;
                case ListaOpções.RemoverPessoa:
                    RemoverPessoa(lista);
                    break;
                case ListaOpções.ImprimirLista:
                    ImprimirLista(lista);
                    break;
                case ListaOpções.Salvar:
                    SalvarLista(lista);
                    break;
                case ListaOpções.Inválida:
                    break;
                default:
                    break;
            }
        }

        private static void AdicionarPessoas(ListaTelefonica lista)
        {
            Console.WriteLine("Você optou por adicionar uma nova pessoa. Digite o nome dela:");
            var armazenadorNome = Console.ReadLine();
            Console.WriteLine("Digite o telefone com DDD, apenas com números:");
            var armazenadorTelefone = Console.ReadLine();
            bool nomeJáExiste = lista.ContémNome(armazenadorNome);
            if (nomeJáExiste)
            {
                Console.WriteLine("Esta pessoa já consta na lista.");
            }
            else
            {
                bool foiInserida = lista.AdicionarPessoa(armazenadorNome, armazenadorTelefone);
                if (!foiInserida)
                {
                    Console.WriteLine("O telefone digitado é inválido.");
                }
                else
                {
                    Console.WriteLine("Pessoa adicionada com sucesso.");
                }
            }
            
            
        }

        private static void AlterarNúmero(ListaTelefonica lista)
        {
            Console.WriteLine("Você optou por alterar o telefone de uma pessoa já registrada. Digite a pessoa desejada:");
            var pessoaAlterada = Console.ReadLine();
            Console.WriteLine("Digite o novo telefone:");
            var telefoneAtualizado = Console.ReadLine();
            bool pessoaExiste = lista.ContémNome(pessoaAlterada);
            if (pessoaExiste)
            {
                bool foiAtualizado = lista.AlterarNúmero(pessoaAlterada, telefoneAtualizado);
                if (foiAtualizado)
                {
                    Console.WriteLine("O telefone foi atualizado com sucesso.");
                }
                else
                {
                    Console.WriteLine("O telefone é inválido.");
                }
            }
            else
            {
                Console.WriteLine("A pessoa digitada não existe na lista.");
            }
        }

        private static void RemoverPessoa(ListaTelefonica lista)
        {
            var nomePessoa = Console.ReadLine();
            bool foiRemovida = lista.RemoverPessoa(nomePessoa);
            if (foiRemovida)
            {
                Console.WriteLine("Pessoa removida com sucesso.");
            }
            else
            {
                Console.WriteLine("O nome digitado não existe na lista.");
            }
        }

        private static void ImprimirLista(ListaTelefonica lista)
        {
            if (lista.EstáVazia())
            {
                Console.WriteLine("Sua lista está vazia no momento.");
            }
            else
            {
                Console.WriteLine("Sua lista atual contém as seguintes pessoas e telefones:");
                Console.WriteLine(lista.FormaImpressa());
            }
        }

        private static void SalvarLista(ListaTelefonica lista)
        {
            lista.Salvar(CaminhoLista);
            Console.WriteLine("Sua lista telêfônica foi salva com sucesso!"); ;
        }
    }
}
