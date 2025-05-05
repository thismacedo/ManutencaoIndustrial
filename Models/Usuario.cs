namespace ManutencaoIndustrial.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; } // Simples para este projeto
        public string Tipo { get; set; } // "Operador" ou "Admin"
    }
}
