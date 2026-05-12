namespace ExpenseTracker.Infrastructure;

using System.Text.Json;
using ExpenseTracker.Domain;

/// <summary>
/// Репозиторій витрат з підтримкою збереження у JSON файл
/// </summary>
public class FileSystemExpenseRepository : IExpenseRepository, IExpensePersistence
{
    private readonly List<Expense> _expenses = new();
    private int _nextId = 1;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public void Add(Expense expense)
    {
        if (expense == null)
            throw new ArgumentNullException(nameof(expense));

        var expenseWithId = expense.WithId(_nextId++);
        _expenses.Add(expenseWithId);
    }

    public IEnumerable<Expense> GetAll()
    {
        return _expenses.AsReadOnly();
    }

    public Expense? GetById(int id)
    {
        return _expenses.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Expense> GetByCategory(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        return _expenses.Where(e => e.Category.Equals(category)).ToList();
    }

    public IEnumerable<Expense> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return _expenses
            .Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
            .OrderBy(e => e.Date)
            .ToList();
    }

    public void Save(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Шлях до файлу не може бути порожнім", nameof(filePath));

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(_expenses, _jsonOptions);
        File.WriteAllText(filePath, json);
    }

    public void Load(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Шлях до файлу не може бути порожнім", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл з витратами не знайдено", filePath);

        var json = File.ReadAllText(filePath);
        var expenses = JsonSerializer.Deserialize<List<Expense>>(json, _jsonOptions);

        if (expenses == null)
            throw new InvalidOperationException("Не вдалося десеріалізувати витрати з файлу");

        _expenses.Clear();
        _expenses.AddRange(expenses.OrderBy(e => e.Id));
        _nextId = _expenses.Any() ? _expenses.Max(e => e.Id) + 1 : 1;
    }
}
