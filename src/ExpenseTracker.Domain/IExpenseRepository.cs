namespace ExpenseTracker.Domain;

/// <summary>
/// Інтерфейс для абстрагування збереження витрат
/// Дозволяє змінювати реалізацію без зміни решти коду
/// </summary>
public interface IExpenseRepository
{
    /// <summary>
    /// Додати нову витрату до сховища
    /// </summary>
    void Add(Expense expense);

    /// <summary>
    /// Отримати всі витрати
    /// </summary>
    IEnumerable<Expense> GetAll();

    /// <summary>
    /// Отримати витрату за ID
    /// </summary>
    Expense? GetById(int id);

    /// <summary>
    /// Отримати витрати за категорією
    /// </summary>
    IEnumerable<Expense> GetByCategory(Category category);

    /// <summary>
    /// Отримати витрати за період
    /// </summary>
    IEnumerable<Expense> GetByDateRange(DateTime startDate, DateTime endDate);
}
