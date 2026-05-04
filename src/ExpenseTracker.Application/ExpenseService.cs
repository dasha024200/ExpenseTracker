namespace ExpenseTracker.Application;

using ExpenseTracker.Domain;

/// <summary>
/// Сервіс для управління витратами
/// Містить бізнес-логіку для операцій з витратами
/// Демонструє Dependency Injection через конструктор (SOLID принцип)
/// </summary>
public class ExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ExpenseService(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository)
    {
        _expenseRepository = expenseRepository ?? throw new ArgumentNullException(nameof(expenseRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <summary>
    /// Додати нову витрату до системи
    /// </summary>
    public Result AddExpense(decimal amount, Category category, DateTime date, string description)
    {
        try
        {
            var expense = Expense.Create(amount, category, date, description);
            _expenseRepository.Add(expense);
            return Result.Ok();
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Помилка при додаванні витрати: {ex.Message}");
        }
    }

    /// <summary>
    /// Отримати всі витрати
    /// </summary>
    public IEnumerable<Expense> GetAllExpenses()
    {
        return _expenseRepository.GetAll();
    }

    /// <summary>
    /// Отримати витрати за категорією
    /// </summary>
    public IEnumerable<Expense> GetExpensesByCategory(Category category)
    {
        return _expenseRepository.GetByCategory(category);
    }

    /// <summary>
    /// Отримати витрати за місяц та рік
    /// </summary>
    public IEnumerable<Expense> GetExpensesByMonth(int month, int year)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        return _expenseRepository.GetByDateRange(startDate, endDate);
    }

    /// <summary>
    /// Отримати витрати за період
    /// </summary>
    public IEnumerable<Expense> GetExpensesByDateRange(DateTime startDate, DateTime endDate)
    {
        return _expenseRepository.GetByDateRange(startDate, endDate);
    }

    /// <summary>
    /// Отримати статистику витрат по категоріям
    /// Демонструє використання LINQ (GroupBy, Sum)
    /// </summary>
    public Dictionary<Category, decimal> GetCategoryStatistics()
    {
        return _expenseRepository
            .GetAll()
            .GroupBy(e => e.Category)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
    }

    /// <summary>
    /// Отримати загальну суму всіх витрат
    /// </summary>
    public decimal GetTotalExpenses()
    {
        return _expenseRepository.GetAll().Sum(e => e.Amount);
    }

    /// <summary>
    /// Отримати середню витрату
    /// </summary>
    public decimal GetAverageExpense()
    {
        var expenses = _expenseRepository.GetAll().ToList();
        return expenses.Count == 0 ? 0 : expenses.Average(e => e.Amount);
    }

    /// <summary>
    /// Отримати витрату з найбільшою сумою
    /// </summary>
    public Expense? GetMaxExpense()
    {
        return _expenseRepository.GetAll().OrderByDescending(e => e.Amount).FirstOrDefault();
    }

    /// <summary>
    /// Отримати витрату з найменшою сумою
    /// </summary>
    public Expense? GetMinExpense()
    {
        return _expenseRepository.GetAll().OrderBy(e => e.Amount).FirstOrDefault();
    }

    /// <summary>
    /// Отримати всі категорії
    /// </summary>
    public IEnumerable<Category> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }

    /// <summary>
    /// Додати нову категорію
    /// </summary>
    public Result AddCategory(string categoryName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return Result.Fail("Назва категорії не може бути порожньою");

            var existing = _categoryRepository.GetByName(categoryName);
            if (existing != null)
                return Result.Fail("Категорія з такою назвою вже існує");

            var category = new Category(_categoryRepository.GetAll().Count() + 1, categoryName);
            _categoryRepository.Add(category);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Помилка при додаванні категорії: {ex.Message}");
        }
    }
}
