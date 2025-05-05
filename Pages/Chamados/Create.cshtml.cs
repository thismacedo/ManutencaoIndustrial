using System;
using System.Threading.Tasks;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ManutencaoIndustrial.Pages.Chamados
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Chamado Chamado { get; set; } = default!;

        public SelectList Maquinas { get; set; } = default!;

        public void OnGet()
        {
            Chamado = new Chamado
            {
                Status = "Pendente",
                DataAbertura = DateTime.Now,
                UsuarioSolicitante = User.Identity?.Name ?? "Sistema"
            };

            var maquinas = _context.Maquinas.OrderBy(m => m.Nome).ToList();
            Maquinas = new SelectList(maquinas, "Id", "Nome");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remove Maquina validation since we're using MaquinaId
            ModelState.Remove("Chamado.Maquina");

            // Set default values
            Chamado.Status = "Pendente";
            Chamado.DataAbertura = DateTime.Now;
            Chamado.UsuarioSolicitante = User.Identity?.Name ?? "Sistema";

            if (!ModelState.IsValid)
            {
                var maquinas = _context.Maquinas.OrderBy(m => m.Nome).ToList();
                Maquinas = new SelectList(maquinas, "Id", "Nome");
                return Page();
            }

            try
            {
                await _context.Chamados.AddAsync(Chamado);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Chamado registrado com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar: " + ex.Message);
                var maquinas = _context.Maquinas.OrderBy(m => m.Nome).ToList();
                Maquinas = new SelectList(maquinas, "Id", "Nome");
                return Page();
            }
        }
    }
}
