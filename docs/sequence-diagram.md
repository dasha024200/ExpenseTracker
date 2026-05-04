# ExpenseTracker - Sequence Diagram

## Діаграма послідовності для сценарію "Додавання витрати"

```mermaid
sequenceDiagram
    actor User
    participant Console as Console UI
    participant Service as ExpenseService
    participant Repo as IExpenseRepository
    participant Domain as Expense (Domain)
    participant Memory as In-Memory Storage

    User->>Console: Вводить суму, категорію, опис
    Console->>Service: AddExpense(amount, category, description)
    Service->>Domain: Expense.Create(amount, category, description)
    Domain->>Domain: Валідація (перевірка суми > 0)
    Domain-->>Service: Повертає Expense
    Service->>Repo: Add(expense)
    Repo->>Memory: Зберігає витрату в список
    Repo-->>Service: Підтвердження
    Service-->>Console: Успіх
    Console-->>User: "Витрату додано успішно"
```

## Діаграма послідовності для сценарію "Перегляд витрат"

```mermaid
sequenceDiagram
    actor User
    participant Console as Console UI
    participant Service as ExpenseService
    participant Repo as IExpenseRepository
    participant Memory as In-Memory Storage

    User->>Console: Запитує витрати за період
    Console->>Service: GetExpensesByMonth(month, year)
    Service->>Repo: GetByDateRange(startDate, endDate)
    Repo->>Memory: Отримує витрати
    Memory-->>Repo: Повертає список витрат
    Repo-->>Service: Повертає IEnumerable~Expense~
    Service-->>Console: Витрати
    Console-->>User: Виводить відформатований список
```

## Діаграма послідовності для сценарію "Отримання статистики"

```mermaid
sequenceDiagram
    actor User
    participant Console as Console UI
    participant Service as ExpenseService
    participant Repo as IExpenseRepository
    participant LINQ as LINQ Queries

    User->>Console: Запитує статистику по категоріям
    Console->>Service: GetCategoryStatistics()
    Service->>Repo: GetAll()
    Repo-->>Service: IEnumerable~Expense~
    Service->>LINQ: GroupBy(e => e.Category).Sum(e => e.Amount)
    LINQ-->>Service: Dictionary~Category, decimal~
    Service-->>Console: Статистика
    Console-->>User: Виводить звіт
```

## Архітектурні переходи між шарами

```
┌─────────────────────────────────────────────┐
│           Console UI Layer                  │
│ - Читає введення користувача                │
│ - Форматує вихід                            │
└──────────────┬──────────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────────┐
│      Application Service Layer              │
│ - ExpenseService                            │
│ - Бізнес-логіка                             │
│ - Орхестрація операцій                      │
└──────────────┬──────────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────────┐
│       Domain Model Layer                    │
│ - Expense (entity)                          │
│ - Category (value object)                   │
│ - Бізнес-правила                            │
└──────────────┬──────────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────────┐
│    Infrastructure & Repository Layer        │
│ - InMemoryExpenseRepository                 │
│ - InMemoryCategoryRepository                │
│ - Зберігання даних                          │
└─────────────────────────────────────────────┘
```
