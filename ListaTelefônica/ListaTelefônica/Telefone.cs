using System;
using System.Linq;

namespace ListaTelefônica
{
    public class Telefone
    {
        public int Id { get; set; }

        public TipoTelefone Tipo { get; set; }
        
        public string Numero { get; set; }

        public Telefone()
        {

        }

        public Telefone(string numero)
        {
            var tipo = numero[2] == '9' ? TipoTelefone.Celular : TipoTelefone.Casa;
            if (!TelefoneEhValido(tipo, numero))
            {
                throw new Exception("Número de telefone inválido!");
            }
            Tipo = tipo;
            Numero = numero;
        }

        private bool TelefoneEhValido(TipoTelefone tipo, string numero)
        {
            if (numero.All(x => char.IsDigit(x)))
            {
                if (tipo == TipoTelefone.Casa)
                {
                    return numero.Length == 10;
                }
                if (tipo == TipoTelefone.Celular)
                {
                    return numero.Length == 11;
                }
            }
            return false;
        }
    }
}