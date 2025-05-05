using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Chamados
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Chamado Chamado { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Chamado = await _context.Chamados
                .Include(c => c.Maquina)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Chamado == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Chamado == null || Chamado.Id == 0)
                return NotFound();

            var chamado = await _context.Chamados.FindAsync(Chamado.Id);

            if (chamado != null)
            {
                _context.Chamados.Remove(chamado);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
