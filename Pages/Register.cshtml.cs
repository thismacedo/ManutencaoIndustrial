using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ManutencaoIndustrial.Models;
using ManutencaoIndustrial.Data;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages
{
    [Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario NovoUsuario { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verificar se o email já está em uso
            if (_context.Usuarios.Any(u => u.Email == NovoUsuario.Email))
            {
                ModelState.AddModelError(string.Empty, "Este email já está em uso.");
                return Page();
            }

            _context.Usuarios.Add(NovoUsuario);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToPage("/Index");
        }
    }
}