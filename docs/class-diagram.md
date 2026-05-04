# ExpenseTracker - Class Diagram

## Діаграма класів у форматі Mermaid

```mermaid
classDiagram
    class Category {
        -int _id
        -string _name
        +int Id
        +string Name
        +Category(int id, string name)
    }

    class Expense {
        -int _id
        -decimal _amount
        -Category _category
        -DateTime _date
        -string _description
        +int Id
        +decimal Amount
        +Category Category
        +DateTime Date
        +string Description
        +Expense(int id, decimal amount, Category category, DateTime date, string description)
        +static Expense Create(decimal amount, Category category, string description)
    }

    class IExpenseRepository {
        <<interface>>
        +Add(Expense expense) void
        +GetAll() IEnumerable~Expense~
        +GetById(int id) Expense
        +GetByCategory(Category category) IEnumerable~Expense~
        +GetByDateRange(DateTime startDate, DateTime endDate) IEnumerable~Expense~
    }

    class InMemoryExpenseRepository {
        -List~Expense~ _expenses
        -int _nextId
        +Add(Expense expense) void
        +GetAll() IEnumerable~Expense~
        +GetById(int id) Expense
        +GetByCategory(Category category) IEnumerable~Expense~
        +GetByDateRange(DateTime startDate, DateTime endDate) IEnumerable~Expense~
    }

    class ExpenseService {
        -IExpenseRepository _repository
        -ICategoryRepository _categoryRepository
        +ExpenseService(IExpenseRepository repository, ICategoryRepository categoryRepository)
        +AddExpense(decimal amount, Category category, string description) void
        +GetAllExpenses() IEnumerable~Expense~
        +GetExpensesByCategory(Category category) IEnumerable~Expense~
        +GetExpensesByMonth(int month, int year) IEnumerable~Expense~
        +GetCategoryStatistics() Dictionary~Category, decimal~
    }

    class ICategoryRepository {
        <<interface>>
        +Add(Category category) void
        +GetAll() IEnumerable~Category~
        +GetById(int id) Category
        +GetByName(string name) Category
    }

    class InMemoryCategoryRepository {
        -List~Category~ _categories
        -int _nextId
        +Add(Category category) void
        +GetAll() IEnumerable~Category~
        +GetById(int id) Category
        +GetByName(string name) Category
    }

    ExpenseService --> IExpenseRepository
    ExpenseService --> ICategoryRepository
    InMemoryExpenseRepository --|> IExpenseRepository
    InMemoryCategoryRepository --|> ICategoryRepository
    Expense --> Category
    IExpenseRepository --> Expense
    ICategoryRepository --> Category
```

## Опис компонентів

### Domain Layer (ExpenseTracker.Domain)
- **Category** - значення для категоризації витрат (їжа, транспорт, розваги тощо)
- **Expense** - сутність для представлення однієї витрати з усіма деталями

### Application Layer (ExpenseTracker.Application)
- **ExpenseService** - бізнес-логіка для управління витратами
- **IExpenseRepository** - контракт для роботи з витратами
- **ICategoryRepository** - контракт для роботи з категоріями

### Infrastructure Layer (ExpenseTracker.Infrastructure)
- **InMemoryExpenseRepository** - реалізація репозиторію в памяті
- **InMemoryCategoryRepository** - реалізація репозиторію категорій в памяті

### Console Layer (ExpenseTracker.Console)
- **Program** - точка входу та консольний інтерфейс користувача
