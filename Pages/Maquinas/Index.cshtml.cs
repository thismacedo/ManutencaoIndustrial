using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ManutencaoIndustrial.Data;
using ManutencaoIndustrial.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Maquinas
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Maquina> Maquinas { get; set; }

        public async Task OnGetAsync()
        {
            Maquinas = await _context.Maquinas.ToListAsync();
        }
    }
}
