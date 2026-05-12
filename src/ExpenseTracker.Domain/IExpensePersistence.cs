namespace ExpenseTracker.Domain;

/// <summary>
/// Інтерфейс для збереження та завантаження витрат з файлового сховища
/// </summary>
public interface IExpensePersistence
{
    /// <summary>
    /// Зберегти витрати у JSON файл
    /// </summary>
    void Save(string filePath);

    /// <summary>
    /// Завантажити витрати з JSON файлу
    /// </summary>
    void Load(string filePath);
}
