using System.Text.Json.Serialization;

namespace ExpenseTracker.Domain;

/// <summary>
/// Витрата - основна сутність системи для відслідковування видатків
/// </summary>
public class Expense
{
    public int Id { get; }
    public decimal Amount { get; }
    public Category Category { get; }
    public DateTime Date { get; }
    public string Description { get; }

    /// <summary>
    /// Конструктор для створення та десеріалізації витрати
    /// </summary>
    [JsonConstructor]
    public Expense(int id, decimal amount, Category category, DateTime date, string description)
    {
        ValidateAmount(amount);
        
        Id = id;
        Amount = amount;
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Date = date;
        Description = description ?? string.Empty;
    }

    /// <summary>
    /// Factory метод для створення витрати з валідацією
    /// </summary>
    public static Expense Create(decimal amount, Category category, DateTime date, string description)
    {
        ValidateAmount(amount);
        
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        return new Expense(0, amount, category, date, description);
    }

    /// <summary>
    /// Метод для встановлення ID (використовується репозиторієм)
    /// </summary>
    public Expense WithId(int id)
    {
        return new Expense(id, Amount, Category, Date, Description);
    }

    /// <summary>
    /// Валідація суми витрати
    /// </summary>
    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сума витрати повинна бути більша за 0");

        if (amount > 1_000_000)
            throw new ArgumentException("Сума витрати занадто велика (максимум 1,000,000)");
    }

    public override string ToString()
    {
        return $"[{Id}] {Date:dd.MM.yyyy} - {Category.Name}: {Amount:F2} грн - {Description}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Expense other)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
