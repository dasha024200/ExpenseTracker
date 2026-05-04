using ExpenseTracker.Domain;
using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;

namespace ExpenseTracker.Tests;

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
}