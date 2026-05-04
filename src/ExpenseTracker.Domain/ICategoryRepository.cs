namespace ExpenseTracker.Domain;

/// <summary>
/// Інтерфейс для абстрагування збереження категорій
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Додати нову категорію
    /// </summary>
    void Add(Category category);

    /// <summary>
    /// Отримати всі категорії
    /// </summary>
    IEnumerable<Category> GetAll();

    /// <summary>
    /// Отримати категорію за ID
    /// </summary>
    Category? GetById(int id);

    /// <summary>
    /// Отримати категорію за назвою
    /// </summary>
    Category? GetByName(string name);
}
