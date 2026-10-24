using MPPLab1.Data;
using MPPLab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers;

public class TransactionsController : Controller
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index(int? walletId)
    {
        IQueryable<Transaction> query = _context.Transactions
            .Include(t => t.Wallet)
            .Include(t => t.Category);

        // Filter
        if (walletId.HasValue && walletId > 0)
        {
            query = query.Where(t => t.WalletId == walletId.Value);
        }

        // Sort
        var transactions = await query.OrderByDescending(t => t.Date).ToListAsync();

        // Agregation
        ViewBag.TotalAmount = transactions.Sum(t => t.Amount);

        ViewBag.Wallets = await _context.Wallets.ToListAsync();
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.SelectedWalletId = walletId;

        return View(transactions);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        var wallet = await _context.Wallets.FindAsync(transaction.WalletId);
        if (wallet != null)
        {
            wallet.Balance += transaction.Amount;
            _context.Add(transaction);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}