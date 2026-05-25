# ExpenseTracker - Sequence Diagrams

## Сценарій: Додавання витрати

```mermaid
sequenceDiagram
    actor User
    participant Console as Console UI
    participant Service as ExpenseService
    participant Repository as IExpenseRepository
    participant Storage as Storage

    User->>Console: Вводить суму, категорію, опис
    Console->>Service: AddExpense(amount, category, date, description)
    Service->>Service: Expense.Create(amount, category, date, description)
    Service->>Repository: Add(expense)
    Repository->>Storage: Зберігає витрату в список
    Storage-->>Repository: Підтвердження збереження
    Repository-->>Service: Повертає результат
    Service-->>Console: Успіх
    Console-->>User: "Витрату додано успішно"
```

![Додавання витрати](assets/Додавання витрати.png)

## Сценарій: Отримання статистики по категоріям

```mermaid
sequenceDiagram
    actor User
    participant Console as Console UI
    participant Service as ExpenseService
    participant Repository as IExpenseRepository
    participant LINQ as LINQ Queries

    User->>Console: Запитує статистику по категоріям
    Console->>Service: GetCategoryStatistics()
    Service->>Repository: GetAll()
    Repository-->>Service: IEnumerable~Expense~
    Service->>LINQ: GroupBy(e => e.Category).Sum(e => e.Amount)
    LINQ-->>Service: Dictionary~Category, decimal~
    Service-->>Console: Статистика по категоріям
    Console-->>User: Виводить звіт
```

![Отримання статистики](assets/Отримання статистики.png)

## Сценарій: Збереження витрат у JSON

```mermaid
sequenceDiagram
    actor User
    participant Console as Console UI
    participant Service as ExpenseService
    participant Persistence as IExpensePersistence
    participant File as JSON File

    User->>Console: Вибирає "Зберегти витрати у JSON"
    Console->>Service: SaveExpenses(filePath)
    Service->>Persistence: Save(filePath)
    Persistence->>File: Записує JSON
    File-->>Persistence: Підтвердження збереження
    Persistence-->>Service: Повертає успіх
    Service-->>Console: Показує повідомлення про успіх
    Console-->>User: "Витрати збережено"
```

## Архітектурні переходи між шарами

```mermaid
flowchart TD
    A[Console UI Layer<br/>- Читає введення користувача<br/>- Форматує вихід] --> B[Application Service Layer<br/>- ExpenseService<br/>- Бізнес-логіка<br/>- Орхестрація операцій]
    B --> C[Domain Model Layer<br/>- Expense (entity)<br/>- Category (value object)<br/>- Бізнес-правила]
    C --> D[Infrastructure & Repository Layer<br/>- InMemoryExpenseRepository<br/>- FileSystemExpenseRepository<br/>- InMemoryCategoryRepository<br/>- Зберігання даних]

    style A fill:#e1f5fe,stroke:#0097a7,stroke-width:2px
    style B fill:#f3e5f5,stroke:#ff9800,stroke-width:2px
    style C fill:#e8f5e9,stroke:#43a047,stroke-width:2px
    style D fill:#fff3e0,stroke:#f57c00,stroke-width:2px
```

![Архітектурні переходи між шарами](assets/Архітектурні переходи між шарами.png)