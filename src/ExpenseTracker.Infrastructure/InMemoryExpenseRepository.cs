namespace ExpenseTracker.Infrastructure;

using ExpenseTracker.Domain;

/// <summary>
/// In-memory репозиторій для витрат
/// Використовується як альтернативне сховище для локального тестування
/// Має просту реалізацію без persistence
/// </summary>
public class InMemoryExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> _expenses = new();
    private int _nextId = 1;

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

    /// <summary>
    /// Очистити репозиторій (для тестування)
    /// </summary>
    public void Clear()
    {
        _expenses.Clear();
        _nextId = 1;
    }

    /// <summary>
    /// Отримати кількість витрат у репозиторії
    /// </summary>
    public int Count => _expenses.Count;
}
