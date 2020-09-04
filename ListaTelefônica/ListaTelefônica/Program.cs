using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;

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
            "5 - Exportar lista para Excel."
        };

        private const string ConnectionString = "Server=localhost;Database=ListaTelefonica;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            var context = new ListaTelefonicaContext(ConnectionString);
            IPessoaRepository repositorio = new PessoaRepositoryEntity(context);
            Console.WriteLine("Bem-vindo ao construtor de lista telefônica. Escolha uma das opções:");
            while (true)
            {
                ImprmirOpções();
                var opção = LerOpção();
                RealizarAção(opção, repositorio);
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
            return opçãoEscolhida;
        }

        private static void RealizarAção(ListaOpções opção, IPessoaRepository repositorio)
        {
            switch (opção)
            {
                case ListaOpções.Sair:
                    break;
                case ListaOpções.AdicionarPessoa:
                    AdicionarPessoas(repositorio);
                    break;
                case ListaOpções.AlterarNúmero:
                    AlterarNúmero(repositorio);
                    break;
                case ListaOpções.RemoverPessoa:
                    RemoverPessoa(repositorio);
                    break;
                case ListaOpções.ImprimirLista:
                    ImprimirLista(repositorio);
                    break;
                case ListaOpções.ExportarLista:
                    ExportarLista(repositorio);
                    break;
                case ListaOpções.Inválida:
                    break;
            }
        }

        private static void AdicionarPessoas(IPessoaRepository repositorio)
        {
            Console.WriteLine("Você optou por adicionar uma nova pessoa. Digite o nome dela:");
            var armazenadorNome = Console.ReadLine();
            Console.WriteLine("Digite o telefone com DDD, apenas com números:");
            var armazenadorTelefone = Console.ReadLine();
            bool nomeJáExiste = repositorio.Obter(armazenadorNome) != null;
            if (nomeJáExiste)
            {
                Console.WriteLine("Esta pessoa já consta na lista.");
            }
            else
            {
                try
                {
                    var telefone = new Telefone();
                    telefone.Numero = armazenadorTelefone;
                    telefone.Tipo = armazenadorTelefone[2] == 9 ? TipoTelefone.Celular : TipoTelefone.Casa;
                    var pessoa = new Pessoa();
                    pessoa.Nome = armazenadorNome;
                    pessoa.Telefones = new List<Telefone>();
                    pessoa.Telefones.Add(telefone);
                    repositorio.Adicionar(pessoa);
                    Console.WriteLine("Pessoa adicionada com sucesso.");
                }
                catch (Exception)
                {
                    Console.WriteLine("O telefone digitado é inválido.");
                }
            }


        }

        private static void AlterarNúmero(IPessoaRepository repositorio)
        {
            Console.WriteLine("Você optou por alterar o telefone de uma pessoa já registrada. Digite a pessoa desejada:");
            var pessoaAlterada = Console.ReadLine();
            Console.WriteLine("Digite o novo telefone:");
            var telefoneAtualizado = Console.ReadLine();
            var pessoa = repositorio.Obter(pessoaAlterada);
            bool pessoaExiste = pessoa != null;
            if (pessoaExiste)
            {
                try
                {
                    pessoa.Telefones[0].Numero = telefoneAtualizado;
                    repositorio.Atualizar(pessoa);
                    Console.WriteLine("O telefone foi atualizado com sucesso.");
                }
                catch (Exception)
                {
                    Console.WriteLine("O telefone é inválido.");
                }
            }
            else
            {
                Console.WriteLine("A pessoa digitada não existe na lista.");
            }
        }

        private static void RemoverPessoa(IPessoaRepository repositorio)
        {
            Console.WriteLine("Digite o nome da pessoa a ser removida:");
            var nomePessoa = Console.ReadLine();
            var pessoa = repositorio.Obter(nomePessoa);
            if (pessoa == null)
            {
                Console.WriteLine("O nome digitado não existe na lista.");
            }
            else
            {
                repositorio.Remover(pessoa);
                Console.WriteLine("Pessoa removida com sucesso.");
            }
        }

        private static void ImprimirLista(IPessoaRepository repositorio)
        {
            var lista = repositorio.Listar();
            if (lista.Count == 0)
            {
                Console.WriteLine("Sua lista está vazia no momento.");
            }
            else
            {
                Console.WriteLine("\nSua lista atual contém as seguintes pessoas e telefones:");
                Console.WriteLine(FormaImpressa(lista) + "\n");
            }
        }

        private static void ExportarLista(IPessoaRepository repositorio)
        {
            Console.WriteLine("Você optou por exportar a lista telefônica para o Excel. Por favor, digite o local onde o arquivo será salvo:");
            var localArquivo = Console.ReadLine();
            var lista = repositorio.Listar();
            var exportador = new ExportadorListaTelefonica();
            exportador.ExportarLista(lista, localArquivo);
            Console.WriteLine("Lista exportada com sucesso! ");
        }

        public static string FormaImpressa(List<Pessoa> lista)
        {
            var listagemNomes = new List<string>();
            foreach (var itemNaLista in lista)
            {
                listagemNomes.Add($"{itemNaLista.Nome}: {itemNaLista.Telefones.First().Numero}");
            }
            string listaEmString = string.Join("\n", listagemNomes);
            return listaEmString;
        }
    }
}
