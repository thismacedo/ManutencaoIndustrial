using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Maquinas
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Maquina Maquina { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Maquina = await _context.Maquinas.FindAsync(id);
            if (Maquina == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Maquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Maquinas.Any(m => m.Id == Maquina.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("Index");
        }
    }
}
