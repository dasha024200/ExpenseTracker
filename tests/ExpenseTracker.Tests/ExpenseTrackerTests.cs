using ExpenseTracker.Domain;
using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;

namespace ExpenseTracker.Tests;

/// <summary>
/// Інтеграційні тести - тестування взаємодії між компонентами
/// </summary>
public class IntegrationTests
{
    /// <summary>
    /// Тест 14: Інтеграція Service + Repository - додавання та отримання витрат
    /// </summary>
    [Fact]
    public void ExpenseService_WithInMemoryRepository_AddAndRetrieveExpenses()
    {
        // Arrange
        var expenseRepo = new InMemoryExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);
        var category = service.GetAllCategories().First();

        // Act
        service.AddExpense(100, category, DateTime.Now, "Тест 1");
        service.AddExpense(200, category, DateTime.Now, "Тест 2");
        var expenses = service.GetAllExpenses().ToList();

        // Assert
        Assert.Equal(2, expenses.Count);
        Assert.Contains(expenses, e => e.Description == "Тест 1");
        Assert.Contains(expenses, e => e.Description == "Тест 2");
    }

    /// <summary>
    /// Тест 15: Інтеграція з FileSystem - збереження та завантаження
    /// </summary>
    [Fact]
    public void ExpenseService_WithFileSystemRepository_SaveAndLoadIntegration()
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"integration_test_{Guid.NewGuid()}.json");
        var expenseRepo = new FileSystemExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);
        var category = service.GetAllCategories().First();

        try
        {
            // Act - додати витрати
            service.AddExpense(150, category, new DateTime(2026, 5, 1), "Інтеграційний тест");
            service.AddExpense(250, category, new DateTime(2026, 5, 2), "Ще один тест");

            // Зберегти
            var saveResult = service.SaveExpenses(tempFile);
            Assert.IsType<Result.Success>(saveResult);

            // Створити новий сервіс і завантажити
            var loadRepo = new FileSystemExpenseRepository();
            var loadService = new ExpenseService(loadRepo, categoryRepo);
            var loadResult = loadService.LoadExpenses(tempFile);
            Assert.IsType<Result.Success>(loadResult);

            // Перевірити завантажені дані
            var loadedExpenses = loadService.GetAllExpenses().ToList();
            Assert.Equal(2, loadedExpenses.Count);
            Assert.Equal(150, loadedExpenses[0].Amount);
            Assert.Equal(250, loadedExpenses[1].Amount);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Тест 16: Fault handling - помилка при збереженні у недоступний файл
    /// </summary>
    [Fact]
    public void ExpenseService_SaveExpenses_InvalidPath_ReturnsFailure()
    {
        // Arrange
        var invalidPath = @"\\nonexistent\server\share\file.json";
        var expenseRepo = new FileSystemExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);

        // Act
        var result = service.SaveExpenses(invalidPath);

        // Assert
        Assert.IsType<Result.Failure>(result);
        var failure = (Result.Failure)result;
        Assert.Contains("вводу-виводу", failure.ErrorMessage);
    }

    /// <summary>
    /// Тест 17: Fault handling - помилка при завантаженні неіснуючого файлу
    /// </summary>
    [Fact]
    public void ExpenseService_LoadExpenses_NonExistentFile_ReturnsFailure()
    {
        // Arrange
        var nonExistentFile = "non_existent_file.json";
        var expenseRepo = new FileSystemExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);

        // Act
        var result = service.LoadExpenses(nonExistentFile);

        // Assert
        Assert.IsType<Result.Failure>(result);
        var failure = (Result.Failure)result;
        Assert.Contains("не знайдено", failure.ErrorMessage);
    }

    /// <summary>
    /// Тест 18: Fault handling - додавання витрати з невалідними даними
    /// </summary>
    [Fact]
    public void ExpenseService_AddExpense_InvalidData_ReturnsFailure()
    {
        // Arrange
        var expenseRepo = new InMemoryExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);
        var category = service.GetAllCategories().First();

        // Act
        var result = service.AddExpense(-100, category, DateTime.Now, "Невалідна витрата");

        // Assert
        Assert.IsType<Result.Failure>(result);
        var failure = (Result.Failure)result;
        Assert.Contains("більша за 0", failure.ErrorMessage);
    }

    /// <summary>
    /// Тест 19: Інтеграція пошуку та сортування
    /// </summary>
    [Fact]
    public void ExpenseService_SearchAndSort_Integration()
    {
        // Arrange
        var expenseRepo = new InMemoryExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);
        var category = service.GetAllCategories().First();

        service.AddExpense(100, category, new DateTime(2026, 1, 1), "Кафе вранці");
        service.AddExpense(200, category, new DateTime(2026, 1, 2), "Обід у ресторані");
        service.AddExpense(50, category, new DateTime(2026, 1, 3), "Кава ввечері");

        // Act - пошук
        var searchResults = service.SearchExpensesByDescription("ка").ToList();

        // Assert - знайдено 2 результати (Кафе вранці, Кава ввечері)
        Assert.Equal(2, searchResults.Count);

        // Act - сортування за сумою
        var sortedByAmount = service.GetExpensesSortedByAmount(true).ToList();

        // Assert - сортування працює
        Assert.Equal(50, sortedByAmount[0].Amount);
        Assert.Equal(200, sortedByAmount[2].Amount);
    }

    /// <summary>
    /// Тест 20: Інтеграція статистики по категоріям
    /// </summary>
    [Fact]
    public void ExpenseService_CategoryStatistics_Integration()
    {
        // Arrange
        var expenseRepo = new InMemoryExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);
        var categories = service.GetAllCategories().ToList();
        var foodCategory = categories.First(c => c.Name == "Їжа");
        var transportCategory = categories.First(c => c.Name == "Транспорт");

        // Act
        service.AddExpense(100, foodCategory, DateTime.Now, "Сніданок");
        service.AddExpense(200, foodCategory, DateTime.Now, "Обід");
        service.AddExpense(300, transportCategory, DateTime.Now, "Таксі");

        var stats = service.GetCategoryStatistics();
        var total = service.GetTotalExpenses();
        var average = service.GetAverageExpense();

        // Assert
        Assert.Equal(300, stats[foodCategory]);
        Assert.Equal(300, stats[transportCategory]);
        Assert.Equal(600, total);
        Assert.Equal(200, average);
    }
}

/// <summary>
/// Тести для Domain layer - перевірка бізнес-правил
/// </summary>
public class ExpenseDomainTests
{
    /// <summary>
    /// Тест 1: Перевірити, що витрата з від'ємною сумою викидає помилку
    /// </summary>
    [Fact]
    public void ExpenseCreate_WithNegativeAmount_ThrowsArgumentException()
    {
        // Arrange
        var category = new Category(1, "Їжа");
        decimal negativeAmount = -100;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            Expense.Create(negativeAmount, category, DateTime.Now, "Test"));
    }

    /// <summary>
    /// Тест 2: Перевірити, що витрата з нульовою сумою викидає помилку
    /// </summary>
    [Fact]
    public void ExpenseCreate_WithZeroAmount_ThrowsArgumentException()
    {
        // Arrange
        var category = new Category(1, "Їжа");
        decimal zeroAmount = 0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            Expense.Create(zeroAmount, category, DateTime.Now, "Test"));
    }

    /// <summary>
    /// Тест 3: Перевірити успішне створення витрати з коректними даними
    /// </summary>
    [Fact]
    public void ExpenseCreate_WithValidData_CreatesExpense()
    {
        // Arrange
        var category = new Category(1, "Їжа");
        decimal amount = 100;
        string description = "Обід";
        var date = DateTime.Now;

        // Act
        var expense = Expense.Create(amount, category, date, description);

        // Assert
        Assert.NotNull(expense);
        Assert.Equal(amount, expense.Amount);
        Assert.Equal(category, expense.Category);
        Assert.Equal(description, expense.Description);
    }

    /// <summary>
    /// Тест 4: Перевірити, що категорія з порожньою назвою викидає помилку
    /// </summary>
    [Fact]
    public void Category_WithEmptyName_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Category(1, ""));
    }

    /// <summary>
    /// Тест 5: Перевірити рівність двох категорій з однаковим ID
    /// </summary>
    [Fact]
    public void Category_WithSameId_AreEqual()
    {
        // Arrange
        var category1 = new Category(1, "Їжа");
        var category2 = new Category(1, "Їжа");

        // Act & Assert
        Assert.Equal(category1, category2);
    }
}

/// <summary>
/// Тести для Application layer - перевірка сервісної логіки
/// </summary>
public class ExpenseServiceTests
{
    private ExpenseService CreateService()
    {
        var expenseRepository = new InMemoryExpenseRepository();
        var categoryRepository = new InMemoryCategoryRepository();
        return new ExpenseService(expenseRepository, categoryRepository);
    }

    /// <summary>
    /// Тест 6: Перевірити, що витрата успішно додається до сервісу
    /// </summary>
    [Fact]
    public void AddExpense_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var service = CreateService();
        var category = service.GetAllCategories().First();
        decimal amount = 250;

        // Act
        var result = service.AddExpense(amount, category, DateTime.Now, "Обід");

        // Assert
        Assert.IsType<Result.Success>(result);
        Assert.Single(service.GetAllExpenses());
    }

    /// <summary>
    /// Тест 7: Перевірити, що додаються витрати відображаються в GetAllExpenses
    /// </summary>
    [Fact]
    public void GetAllExpenses_AfterAdding_ReturnsAddedExpenses()
    {
        // Arrange
        var service = CreateService();
        var category = service.GetAllCategories().First();
        
        // Act
        service.AddExpense(100, category, DateTime.Now, "Витрата 1");
        service.AddExpense(200, category, DateTime.Now, "Витрата 2");
        service.AddExpense(150, category, DateTime.Now, "Витрата 3");

        var expenses = service.GetAllExpenses().ToList();

        // Assert
        Assert.Equal(3, expenses.Count);
    }

    /// <summary>
    /// Тест 8: Перевірити розрахунок статистики по категоріям
    /// </summary>
    [Fact]
    public void GetCategoryStatistics_WithMultipleExpenses_CalculatesCorrectly()
    {
        // Arrange
        var service = CreateService();
        var categories = service.GetAllCategories().ToList();
        var foodCategory = categories.First(c => c.Name == "Їжа");
        var transportCategory = categories.First(c => c.Name == "Транспорт");

        // Act
        service.AddExpense(100, foodCategory, DateTime.Now, "Завтрак");
        service.AddExpense(150, foodCategory, DateTime.Now, "Обід");
        service.AddExpense(200, transportCategory, DateTime.Now, "Такси");

        var statistics = service.GetCategoryStatistics();

        // Assert
        Assert.Equal(250, statistics[foodCategory]);
        Assert.Equal(200, statistics[transportCategory]);
    }

    /// <summary>
    /// Тест 9: Перевірити розрахунок загальної суми витрат
    /// </summary>
    [Fact]
    public void GetTotalExpenses_WithMultipleExpenses_CalculatesCorrectly()
    {
        // Arrange
        var service = CreateService();
        var category = service.GetAllCategories().First();

        // Act
        service.AddExpense(100, category, DateTime.Now, "Витрата 1");
        service.AddExpense(200, category, DateTime.Now, "Витрата 2");
        service.AddExpense(150, category, DateTime.Now, "Витрата 3");

        var total = service.GetTotalExpenses();

        // Assert
        Assert.Equal(450, total);
    }

    /// <summary>
    /// Тест 10: Перевірити розрахунок середної витрати
    /// </summary>
    [Fact]
    public void GetAverageExpense_WithMultipleExpenses_CalculatesCorrectly()
    {
        // Arrange
        var service = CreateService();
        var category = service.GetAllCategories().First();

        // Act
        service.AddExpense(100, category, DateTime.Now, "Витрата 1");
        service.AddExpense(200, category, DateTime.Now, "Витрата 2");
        service.AddExpense(300, category, DateTime.Now, "Витрата 3");

        var average = service.GetAverageExpense();

        // Assert
        Assert.Equal(200, average);
    }

    /// <summary>
    /// Тест 11: Перевірити пошук витрат за описом
    /// </summary>
    [Fact]
    public void SearchExpensesByDescription_ReturnsMatchingExpenses()
    {
        // Arrange
        var service = CreateService();
        var category = service.GetAllCategories().First();

        service.AddExpense(100, category, DateTime.Now, "Обід у кафе");
        service.AddExpense(200, category, DateTime.Now, "Книга для навчання");

        // Act
        var results = service.SearchExpensesByDescription("кафе").ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains("кафе", results[0].Description, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Тест 12: Перевірити сортування витрат за сумою
    /// </summary>
    [Fact]
    public void GetExpensesSortedByAmount_ReturnsSortedExpenses()
    {
        // Arrange
        var service = CreateService();
        var category = service.GetAllCategories().First();

        service.AddExpense(300, category, DateTime.Now, "Витрата 1");
        service.AddExpense(100, category, DateTime.Now, "Витрата 2");
        service.AddExpense(200, category, DateTime.Now, "Витрата 3");

        // Act
        var ascending = service.GetExpensesSortedByAmount(true).ToList();
        var descending = service.GetExpensesSortedByAmount(false).ToList();

        // Assert
        Assert.Equal(100, ascending[0].Amount);
        Assert.Equal(300, descending[0].Amount);
    }

    /// <summary>
    /// Тест 13: Перевірити збереження та завантаження витрат у JSON
    /// </summary>
    [Fact]
    public void SaveAndLoadExpenses_PersistsDataToJson()
    {
        // Arrange
        var expenseRepo = new FileSystemExpenseRepository();
        var categoryRepo = new InMemoryCategoryRepository();
        var service = new ExpenseService(expenseRepo, categoryRepo);
        var category = service.GetAllCategories().First();
        var tempFile = Path.Combine(Path.GetTempPath(), $"expense_tracker_{Guid.NewGuid()}.json");

        try
        {
            service.AddExpense(100, category, new DateTime(2026, 1, 1), "Тестова витрата");
            var saveResult = service.SaveExpenses(tempFile);
            Assert.IsType<Result.Success>(saveResult);

            var loadRepo = new FileSystemExpenseRepository();
            var loadService = new ExpenseService(loadRepo, categoryRepo);
            var loadResult = loadService.LoadExpenses(tempFile);
            Assert.IsType<Result.Success>(loadResult);

            var loadedExpenses = loadService.GetAllExpenses().ToList();
            Assert.Single(loadedExpenses);
            Assert.Equal(100, loadedExpenses[0].Amount);
            Assert.Equal("Тестова витрата", loadedExpenses[0].Description);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
}