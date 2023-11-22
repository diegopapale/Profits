using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cadastro_de_Documentos.Models;
using OfficeOpenXml;

namespace Cadastro_de_Documentos.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly Context _context;

        public DocumentsController(Context context)
        {
            _context = context;
        }
               
        public IActionResult Index()
        {
            var documents = _context.Documents.ToList();
            return View(documents);
        }

        public IActionResult ExportToExcel()
        {
            var documents = _context.Documents.ToList();

            // Cria um objeto MemoryStream para armazenar o conteúdo do arquivo Excel
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("DocumentData");

                // Adiciona os cabeçalhos das colunas
                workSheet.Cells[1, 1].Value = "Title";
                workSheet.Cells[1, 2].Value = "Code";
                workSheet.Cells[1, 3].Value = "Revision";
                workSheet.Cells[1, 4].Value = "Date";

                // Preenche os dados nas células
                for (int i = 0; i < documents.Count; i++)
                {
                    workSheet.Cells[i + 2, 1].Value = documents[i].Title;
                    workSheet.Cells[i + 2, 2].Value = documents[i].Code;
                    workSheet.Cells[i + 2, 3].Value = documents[i].Revision;
                    workSheet.Cells[i + 2, 4].Value = documents[i].Date.ToString("yyyy-MM-dd");
                }
                               
                // Converte o conteúdo para um array de bytes
                var excelBytes = package.GetAsByteArray();

                // Retorna o arquivo Excel para download
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DocumentData.xlsx");
            }
        }


        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Code,Revision,Date")] Document document)
        {
            if (ModelState.IsValid)
            {
                // Converta a data para UTC antes de salvar no banco de dados
                document.Date = document.Date.ToUniversalTime();

                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Code,Revision,Date")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Converta a data para UTC antes de salvar no banco de dados
                    document.Date = document.Date.ToUniversalTime();

                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'Context.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return (_context.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
