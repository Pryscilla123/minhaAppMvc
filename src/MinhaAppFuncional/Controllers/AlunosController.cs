using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhaAppFuncional.Data;
using MinhaAppFuncional.Models;

namespace MinhaAppFuncional.Controllers
{
    [Authorize]
    [Route("meus-alunos")]
    public class AlunosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlunosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.Sucesso = "Listagem bem sucedida";

            return View(await _context.Aluno.ToListAsync());
        }

        [Route("{id:int}/detalhes-aluno")]
        public async Task<IActionResult> Details(int id)
        {

            var aluno = await _context.Aluno
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        [Route("novo-aluno")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("novo-aluno")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,Email,EmailConfirmacao,Avaliacao,Ativo")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aluno);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        [Route("editar-aluno/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {

            var aluno = await _context.Aluno.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        [HttpPost("editar-aluno/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataNascimento,Email,Avaliacao,Ativo")] Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            ModelState.Remove("EmailConfirmacao");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["Sucesso"] = "Aluno editado com sucesso.";

                return RedirectToAction(nameof(Index));
            }
            return View(aluno);
        }

        [Route("excluir-aluno/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aluno = await _context.Aluno
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost("excluir-aluno/{id:int}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Aluno.FindAsync(id);

            if (aluno != null)
            {
                _context.Aluno.Remove(aluno);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Aluno.Any(e => e.Id == id);
        }
    }
}
