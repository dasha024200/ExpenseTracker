using ExpenseTracker.Application;
using ExpenseTracker.Domain;
using ExpenseTracker.Infrastructure;

class Program
{
    private static ExpenseService _expenseService = null!;
    private static InMemoryCategoryRepository _categoryRepository = null!;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        InitializeServices();
        
        bool running = true;
        while (running)
        {
            DisplayMainMenu();
            string? choice = Console.ReadLine();

            running = ProcessMenuChoice(choice);
        }

        Console.WriteLine("\nДякуємо за використання ExpenseTracker!");
    }

    /// <summary>
    /// Ініціалізувати сервіси з залежностями
    /// </summary>
    private static void InitializeServices()
    {
        _categoryRepository = new InMemoryCategoryRepository();
        var expenseRepository = new InMemoryExpenseRepository();

        _expenseService = new ExpenseService(expenseRepository, _categoryRepository);
    }

    /// <summary>
    /// Вивести головне меню
    /// </summary>
    private static void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║     Система управління витратами       ║");
        Console.WriteLine("║         ExpenseTracker v1.0            ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("Головне меню:");
        Console.WriteLine("1. Додати витрату");
        Console.WriteLine("2. Переглянути витрати за місяц");
        Console.WriteLine("3. Переглянути статистику по категоріям");
        Console.WriteLine("4. Переглянути всі витрати");
        Console.WriteLine("5. Переглянути статистику");
        Console.WriteLine("0. Вихід");
        Console.Write("\nВиберіть опцію: ");
    }

    /// <summary>
    /// Обробити вибір користувача
    /// </summary>
    private static bool ProcessMenuChoice(string? choice)
    {
        return choice switch
        {
            "1" => AddExpense(),
            "2" => ViewMonthlyExpenses(),
            "3" => ViewCategoryStatistics(),
            "4" => ViewAllExpenses(),
            "5" => ViewStatistics(),
            "0" => false,
            _ => InvalidChoice()
        };
    }

    /// <summary>
    /// Сценарій: Додавання витрати
    /// </summary>
    private static bool AddExpense()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("       ДОДАВАННЯ ВИТРАТИ");
        Console.WriteLine("═══════════════════════════════════════\n");

        try
        {
            // Показати доступні категорії
            var categories = _expenseService.GetAllCategories().ToList();
            Console.WriteLine("Доступні категорії:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }

            // Отримати категорію
            Console.Write("\nВиберіть номер категорії: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("❌ Помилка: невірне число для категорії");
                PauseForUser();
                return true;
            }

            var selectedCategory = _expenseService.GetAllCategories().FirstOrDefault(c => c.Id == categoryId);
            if (selectedCategory == null)
            {
                Console.WriteLine("❌ Помилка: категорія не знайдена");
                PauseForUser();
                return true;
            }

            // Отримати суму
            Console.Write("Введіть суму витрати (грн): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("❌ Помилка: невірне число для суми");
                PauseForUser();
                return true;
            }

            // Отримати опис
            Console.Write("Введіть опис витрати: ");
            string? description = Console.ReadLine() ?? "";

            // Додати витрату
            var result = _expenseService.AddExpense(amount, selectedCategory, DateTime.Now, description);

            result.Match(
                onSuccess: _ =>
                {
                    Console.WriteLine("\n✓ Витрату успішно додано!");
                },
                onFailure: failure =>
                {
                    Console.WriteLine($"\n❌ Помилка: {failure.ErrorMessage}");
                }
            );

            PauseForUser();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Непередбачена помилка: {ex.Message}");
            PauseForUser();
            return true;
        }
    }

    /// <summary>
    /// Сценарій: Перегляд витрат за місяц
    /// </summary>
    private static bool ViewMonthlyExpenses()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("    ВИТРАТИ ЗА МІСЯЦ");
        Console.WriteLine("═══════════════════════════════════════\n");

        Console.Write("Введіть місяц (1-12): ");
        if (!int.TryParse(Console.ReadLine(), out int month) || month < 1 || month > 12)
        {
            Console.WriteLine("❌ Помилка: невірний місяц");
            PauseForUser();
            return true;
        }

        Console.Write("Введіть рік: ");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("❌ Помилка: невірний рік");
            PauseForUser();
            return true;
        }

        var expenses = _expenseService.GetExpensesByMonth(month, year).ToList();

        if (expenses.Count == 0)
        {
            Console.WriteLine("\nЖодних витрат за цей період не знайдено.");
        }
        else
        {
            Console.WriteLine($"\nВитрати за {month:D2}.{year}:\n");
            decimal total = 0;
            foreach (var expense in expenses)
            {
                Console.WriteLine(expense);
                total += expense.Amount;
            }
            Console.WriteLine($"\nЗагально: {total:F2} грн");
        }

        PauseForUser();
        return true;
    }

    /// <summary>
    /// Сценарій: Перегляд статистики по категоріям
    /// </summary>
    private static bool ViewCategoryStatistics()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("   СТАТИСТИКА ПО КАТЕГОРІЯМ");
        Console.WriteLine("═══════════════════════════════════════\n");

        var statistics = _expenseService.GetCategoryStatistics();

        if (statistics.Count == 0)
        {
            Console.WriteLine("Немає витрат для аналізу.");
        }
        else
        {
            var sortedStats = statistics.OrderByDescending(x => x.Value);
            foreach (var (category, total) in sortedStats)
            {
                Console.WriteLine($"{category.Name,-15}: {total,10:F2} грн");
            }
            var totalLabel = "Загально";
            Console.WriteLine($"\n{totalLabel,-15}: {statistics.Values.Sum(),10:F2} грн");
        }

        PauseForUser();
        return true;
    }

    /// <summary>
    /// Переглянути всі витрати
    /// </summary>
    private static bool ViewAllExpenses()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("      УСІ ВИТРАТИ");
        Console.WriteLine("═══════════════════════════════════════\n");

        var expenses = _expenseService.GetAllExpenses().ToList();

        if (expenses.Count == 0)
        {
            Console.WriteLine("Немає записаних витрат.");
        }
        else
        {
            var sortedExpenses = expenses.OrderByDescending(e => e.Date);
            foreach (var expense in sortedExpenses)
            {
                Console.WriteLine(expense);
            }
            Console.WriteLine($"\nЗагально витрат: {expenses.Count}");
            Console.WriteLine($"Загальна сума: {expenses.Sum(e => e.Amount):F2} грн");
        }

        PauseForUser();
        return true;
    }

    /// <summary>
    /// Переглянути загальну статистику
    /// </summary>
    private static bool ViewStatistics()
    {
        Console.Clear();
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("      ЗАГАЛЬНА СТАТИСТИКА");
        Console.WriteLine("═══════════════════════════════════════\n");

        var expenses = _expenseService.GetAllExpenses().ToList();

        if (expenses.Count == 0)
        {
            Console.WriteLine("Немає витрат для аналізу.");
        }
        else
        {
            Console.WriteLine($"Всього витрат: {expenses.Count}");
            Console.WriteLine($"Загальна сума: {_expenseService.GetTotalExpenses():F2} грн");
            Console.WriteLine($"Середня витрата: {_expenseService.GetAverageExpense():F2} грн");
            
            var maxExpense = _expenseService.GetMaxExpense();
            if (maxExpense != null)
                Console.WriteLine($"Найбільша витрата: {maxExpense.Amount:F2} грн ({maxExpense.Category.Name})");
            
            var minExpense = _expenseService.GetMinExpense();
            if (minExpense != null)
                Console.WriteLine($"Найменша витрата: {minExpense.Amount:F2} грн ({minExpense.Category.Name})");
        }

        PauseForUser();
        return true;
    }

    /// <summary>
    /// Обробити невалідний вибір
    /// </summary>
    private static bool InvalidChoice()
    {
        Console.WriteLine("❌ Невірна опція. Будь ласка, спробуйте знову.");
        PauseForUser();
        return true;
    }

    /// <summary>
    /// Зупинити для читання користувачем
    /// </summary>
    private static void PauseForUser()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
        Console.ReadKey();
    }
}
