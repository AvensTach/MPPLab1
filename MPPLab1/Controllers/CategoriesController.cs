using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MPPLab1.Data;
using MPPLab1.Models;

namespace FinanceApp.Controllers;

public class CategoriesController : Controller
{
    private readonly AppDbContext _context;
    public CategoriesController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Categories.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        _context.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}