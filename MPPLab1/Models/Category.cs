using System.ComponentModel.DataAnnotations;

namespace MPPLab1.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}