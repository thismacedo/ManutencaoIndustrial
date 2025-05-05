using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using ManutencaoIndustrial.Data;
using Microsoft.AspNetCore.Authorization;

namespace ManutencaoIndustrial.Pages.Chamados
{
    [Authorize(Roles = "Admin")]
    public class ExportarModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ExportarModel> _logger;

        public ExportarModel(AppDbContext context, ILogger<ExportarModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string status = null, string setor = null, 
            DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            try
            {
                var query = _context.Chamados
                    .Include(c => c.Maquina)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(status))
                    query = query.Where(c => c.Status == status);

                if (!string.IsNullOrEmpty(setor))
                    query = query.Where(c => c.Maquina.Setor.Contains(setor));

                if (dataInicio.HasValue)
                    query = query.Where(c => c.DataAbertura.Date >= dataInicio.Value.Date);

                if (dataFim.HasValue)
                    query = query.Where(c => c.DataAbertura.Date <= dataFim.Value.Date);

                var chamados = await query.OrderByDescending(c => c.DataAbertura).ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Chamados");

                    // Cabeçalhos
                    worksheet.Cell("A1").Value = "ID";
                    worksheet.Cell("B1").Value = "Máquina";
                    worksheet.Cell("C1").Value = "Setor";
                    worksheet.Cell("D1").Value = "Tipo";
                    worksheet.Cell("E1").Value = "Prioridade";
                    worksheet.Cell("F1").Value = "Status";
                    worksheet.Cell("G1").Value = "Data Abertura";
                    worksheet.Cell("H1").Value = "Solicitante";
                    worksheet.Cell("I1").Value = "Descrição";

                    // Estilo dos cabeçalhos
                    var headerRow = worksheet.Row(1);
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                    // Dados
                    var row = 2;
                    foreach (var chamado in chamados)
                    {
                        worksheet.Cell($"A{row}").Value = chamado.Id;
                        worksheet.Cell($"B{row}").Value = chamado.Maquina?.Nome ?? "";
                        worksheet.Cell($"C{row}").Value = chamado.Maquina?.Setor ?? "";
                        worksheet.Cell($"D{row}").Value = chamado.Tipo;
                        worksheet.Cell($"E{row}").Value = chamado.Prioridade;
                        worksheet.Cell($"F{row}").Value = chamado.Status;
                        worksheet.Cell($"G{row}").Value = chamado.DataAbertura.ToString("dd/MM/yyyy HH:mm");
                        worksheet.Cell($"H{row}").Value = chamado.UsuarioSolicitante;
                        worksheet.Cell($"I{row}").Value = chamado.Descricao;
                        row++;
                    }

                    // Formatação da tabela
                    var table = worksheet.Range("A1:I" + (row - 1));
                    table.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    table.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Columns().AdjustToContents();

                    // Gerar arquivo
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;

                        var fileName = $"Chamados_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                        return File(
                            stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao exportar chamados");
                TempData["Erro"] = "Erro ao exportar chamados: " + ex.Message;
                return RedirectToPage("./Index");
            }
        }
    }
}
