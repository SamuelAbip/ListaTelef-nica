namespace ListaTelefônica
{
    public class Telefone
    {
        public int Id { get; set; }

        public TipoTelefone Tipo { get; set; }
        
        public string Numero { get; set; }
    }
}