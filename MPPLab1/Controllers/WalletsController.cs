using MPPLab1.Data;
using MPPLab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers;

public class WalletsController : Controller
{
    private readonly AppDbContext _context;

    public WalletsController(AppDbContext context) => _context = context;

    // READ + LINQ 
    public async Task<IActionResult> Index()
    {
        var walletsInfo = await _context.Wallets
            .Select(w => new
            {
                w.Id,
                w.Name,
                w.Balance,
                TransactionCount = w.Transactions.Count
            })
            .ToListAsync();

        ViewBag.WalletsInfo = walletsInfo;
        return View(await _context.Wallets.ToListAsync());
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(Wallet wallet)
    {
        _context.Add(wallet);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // UPDATE
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string name)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet != null)
        {
            wallet.Name = name;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var wallet = await _context.Wallets.FindAsync(id);
        if (wallet != null)
        {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}