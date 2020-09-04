using System.Collections.Generic;

namespace ListaTelefônica
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual List<Telefone> Telefones { get; set; }

        public Pessoa()
        {

        }

        public Pessoa(string nome, string numero)
        {
            Nome = FormatarNome(nome);
            Telefones = new List<Telefone>
            {
                new Telefone(numero)
            };
        }

        private string FormatarNome(string nome)
        {
            string[] formatandoNome = nome.Split(" ");
            var acumuladorString = new List<string>();
            foreach (string palavra in formatandoNome)
            {
                string palavraFormatada = palavra.Substring(0, 1).ToUpper() + palavra.Substring(1).ToLower();
                acumuladorString.Add(palavraFormatada);
            }
            string nomeFormatado = string.Join(" ", acumuladorString);
            return nomeFormatado;
        }
    }
}
