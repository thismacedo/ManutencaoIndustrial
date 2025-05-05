using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Maquinas
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Maquina Maquina { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Maquinas.Add(Maquina);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
