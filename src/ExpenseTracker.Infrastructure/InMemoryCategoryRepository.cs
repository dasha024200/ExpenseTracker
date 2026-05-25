namespace ExpenseTracker.Infrastructure;

using ExpenseTracker.Domain;

/// <summary>
/// In-memory репозиторій для категорій витрат
/// </summary>
public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new();
    private int _nextId = 1;

    public InMemoryCategoryRepository()
    {
        // Ініціалізувати стандартні категорії
        InitializeDefaultCategories();
    }

    public void Add(Category category)
    {
        if (category == null)
            throw new ArgumentNullException(nameof(category));

        if (_categories.Any(c => c.Name == category.Name))
            throw new InvalidOperationException($"Категорія '{category.Name}' вже існує");

        _categories.Add(new Category(_nextId++, category.Name));
    }

    public IEnumerable<Category> GetAll()
    {
        return _categories.AsReadOnly();
    }

    public Category? GetById(int id)
    {
        return _categories.FirstOrDefault(c => c.Id == id);
    }

    public Category? GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return _categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Ініціалізувати стандартні категорії витрат
    /// </summary>
    private void InitializeDefaultCategories()
    {
        var defaultCategories = new[]
        {
            "Їжа",
            "Транспорт",
            "Розваги",
            "Освіта",
            "Здоров'я",
            "Дім",
            "Одяг",
            "Утилити"
        };

        foreach (var categoryName in defaultCategories)
        {
            _categories.Add(new Category(_nextId++, categoryName));
        }
    }

    /// <summary>
    /// Очистити репозиторій (для тестування)
    /// </summary>
    public void Clear()
    {
        _categories.Clear();
        _nextId = 1;
        InitializeDefaultCategories();
    }

    /// <summary>
    /// Отримати кількість категорій
    /// </summary>
    public int Count => _categories.Count;
}
