using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManutencaoIndustrial.Models
{
    public class Chamado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione uma máquina")]
        [Display(Name = "Máquina")]
        public int MaquinaId { get; set; }

        public virtual Maquina? Maquina { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de manutenção")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione a prioridade")]
        public string Prioridade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Digite a descrição do problema")]
        [MinLength(10, ErrorMessage = "A descrição deve ter pelo menos 10 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        public string Status { get; set; } = "Pendente";
        
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        
        public string UsuarioSolicitante { get; set; } = string.Empty;
    }
}
