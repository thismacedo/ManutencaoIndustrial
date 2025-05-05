using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Maquinas
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
        public Maquina Maquina { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Maquina = await _context.Maquinas.FirstOrDefaultAsync(m => m.Id == id);
            if (Maquina == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var maquina = await _context.Maquinas.FindAsync(Maquina.Id);
            if (maquina == null)
                return NotFound();

            _context.Maquinas.Remove(maquina);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
