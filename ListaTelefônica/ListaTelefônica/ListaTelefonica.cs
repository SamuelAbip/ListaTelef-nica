using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ListaTelefônica
{
    public class ListaTelefonica
    {
        private const string Separador = ":";

        private Dictionary<string, string> Lista { get; } = new Dictionary<string, string>();

        public ListaTelefonica(string caminhoLista)
        {
            if (!File.Exists(caminhoLista))
            {
                File.WriteAllText(caminhoLista, string.Empty);
                return;
            }
            var linhas = File.ReadLines(caminhoLista);
            foreach (var linha in linhas)
            {
                var linhaSeparada = linha.Split(Separador);
                string nomePessoa = linhaSeparada[0].Trim();
                string telefone = linhaSeparada[1].Trim();
                Lista[nomePessoa] = telefone;
            }
        }

        public bool AdicionarPessoa(string nomePessoa, string telefonePessoa)
        {
            string nomeFormatado = FormartarNomePessoa(nomePessoa);
            bool telefoneÉVálido = ValidarTelefone(telefonePessoa);
            if (Lista.ContainsKey(nomeFormatado))
            {
                return false;
            }
            else
            {
                if (telefoneÉVálido)
                {
                    Lista.Add(nomeFormatado, telefonePessoa);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool ValidarTelefone(string telefoneDigitado)
        {
            return telefoneDigitado.All(char.IsDigit) && (telefoneDigitado.Length == 11 || telefoneDigitado.Length == 10);
        }

        private static string FormartarNomePessoa(string nomeDigitado)
        {
            string[] formatandoNome = nomeDigitado.Split(" ");
            var acumuladorString = new List<string>();
            foreach (string palavra in formatandoNome)
            {
                string nomeFormatado = palavra.Substring(0, 1).ToUpper() + palavra.Substring(1).ToLower();
                acumuladorString.Add(nomeFormatado);
            }

            string nomeAdicionado = string.Join(" ", acumuladorString);
            return nomeAdicionado;
        }

        public bool ContémNome(string nomePessoa)
        {
            return Lista.ContainsKey(nomePessoa);
        }

        public bool AlterarNúmero(string nomePessoa, string telefoneNovo)
        {
            var validarTelefoneNovo = ValidarTelefone(telefoneNovo);
            if (validarTelefoneNovo)
            {
                Lista[nomePessoa] = telefoneNovo;
                return true;
            }
            else return false;
        }

        public bool EstáVazia()
        {
            bool coleçãoVazia = !Lista.Any();
            if (coleçãoVazia)
            {
                return true;
            }
            else return false;
        }

        public string FormaImpressa()
        {
            var listagemNomes = new List<string>();
            foreach (var itemNaLista in Lista)
            {
                listagemNomes.Add($"{itemNaLista.Key}{Separador} {itemNaLista.Value}");
            }

            string listaEmString = string.Join("\n", listagemNomes);
            return listaEmString;
        }

        public void Salvar(string caminhoLista)
        {
            File.WriteAllText(caminhoLista, FormaImpressa());
        }

        public bool RemoverPessoa(string nomePessoa)
        {
            if (Lista.ContainsKey(nomePessoa))
            {
                Lista.Remove(nomePessoa);
                return true;
            }
            else return false;
        }
    }
}
