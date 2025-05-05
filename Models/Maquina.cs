using System.ComponentModel.DataAnnotations;

namespace ManutencaoIndustrial.Models
{
    public class Maquina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da máquina é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O setor é obrigatório")]
        public string Setor { get; set; }

        [Required(ErrorMessage = "O código interno é obrigatório")]
        [Display(Name = "Código Interno")]
        public string CodigoInterno { get; set; }
    }
}
