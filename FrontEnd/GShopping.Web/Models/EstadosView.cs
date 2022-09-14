namespace GShopping.Web.Models
{
    public class EstadosView
    {
        public List<EstadoCidadesView> Estados { get; set; }
    }

    public class EstadoCidadesView
    {
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public List<string> Cidades { get; set; }
    }
}
