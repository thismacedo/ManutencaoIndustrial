using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Chamados
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Chamado Chamado { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Chamado = await _context.Chamados
                .Include(c => c.Maquina)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Chamado == null)
                return NotFound();

            // Se for operador, garante que é o dono do chamado
            if (!User.IsInRole("Admin") && Chamado.UsuarioSolicitante != User.Identity.Name)
                return Forbid();

            return Page();
        }
    }
}
