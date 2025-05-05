using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;


namespace ManutencaoIndustrial.Pages.Chamados
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Chamado> Chamados { get; set; }

        public async Task OnGetAsync(string status, string setor, DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _context.Chamados
                .Include(c => c.Maquina)
                .AsQueryable();

            // Operador vê apenas seus chamados
            if (!User.IsInRole("Admin"))
            {
                query = query.Where(c => c.UsuarioSolicitante == User.Identity.Name);
            }
            else
            {
                if (!string.IsNullOrEmpty(status))
                    query = query.Where(c => c.Status == status);

                if (!string.IsNullOrEmpty(setor))
                    query = query.Where(c => c.Maquina.Setor.Contains(setor));

                if (dataInicio.HasValue)
                    query = query.Where(c => c.DataAbertura >= dataInicio.Value);

                if (dataFim.HasValue)
                    query = query.Where(c => c.DataAbertura <= dataFim.Value);
            }

            Chamados = await query
                .OrderByDescending(c => c.DataAbertura)
                .ToListAsync();
        }
    }
}
