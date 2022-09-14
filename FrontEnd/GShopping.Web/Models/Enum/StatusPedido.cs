using System.ComponentModel.DataAnnotations;

namespace GShopping.Web.Models.Enum
{
    public enum StatusPedido
    {
        [Display(Name = "Processando")]
        processando,
        [Display(Name = "Aprovado")]
        aprovado,
        [Display(Name = "Em Análise")]
        analise,
        [Display(Name = "Em Produção")]
        producao,
        [Display(Name = "Pronto")]
        pronto,
        [Display(Name = "Cancelado")]
        cancelado,
        [Display(Name = "Entregue")]
        entregue
    }
}
