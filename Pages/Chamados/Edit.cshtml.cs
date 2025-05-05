using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Chamados
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
        public Chamado Chamado { get; set; }

        public SelectList Maquinas { get; set; }
        public SelectList StatusList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Chamado = await _context.Chamados
                .Include(c => c.Maquina)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Chamado == null)
                return NotFound();

            Maquinas = new SelectList(_context.Maquinas, "Id", "Nome");
            StatusList = new SelectList(new[] { "Pendente", "Em Atendimento", "Concluído" });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remove validation for unchanged fields
            ModelState.Remove("Chamado.Maquina");
            ModelState.Remove("Chamado.Tipo");
            ModelState.Remove("Chamado.Prioridade");
            ModelState.Remove("Chamado.Descricao");

            if (!ModelState.IsValid)
            {
                Maquinas = new SelectList(_context.Maquinas, "Id", "Nome");
                StatusList = new SelectList(new[] { "Pendente", "Em Atendimento", "Concluído" });
                return Page();
            }

            var chamadoDb = await _context.Chamados.FindAsync(Chamado.Id);
            if (chamadoDb == null)
                return NotFound();

            // Update only the status
            chamadoDb.Status = Chamado.Status;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Status do chamado atualizado com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar o chamado. Tente novamente.");
                return Page();
            }
        }
    }
}
