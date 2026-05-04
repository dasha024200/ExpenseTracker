# Ітерація 1 (Lab 34) - Передача на Ітерацію 2

## Дата завершення
Завершено: май 2026

## Що було зроблено

### ✓ Постановка задачі
- Визначена предметна область: **Система управління особистими витратами (ExpenseTracker)**
- Написаний документ **vision.md** з описом проблеми, користувачів, сценаріїв та вимог
- Задані 3 основні сценарії використання:
  1. Додавання витрати
  2. Перегляд витрат за період
  3. Статистика по категоріям

### ✓ Модель предметної області
Реалізовано **7 класів предметної області**:

#### Domain Layer (ExpenseTracker.Domain)
1. **Category** - Value Object для категоризації витрат
   - ID, Name
   - Валідація непорожньої назви
   - Реалізовано Equals/GetHashCode

2. **Expense** - Основна сутність системи
   - ID, Amount, Category, Date, Description
   - Factory метод `Create()` з валідацією
   - WithId() для встановлення ID (repo pattern)
   - Інваріанти: сума > 0, макс 1,000,000

3. **Entity** - Базовий клас для сутностей
   - ID-based equality
   - Основа для поліморфізму

4. **Result** - Результат операції (успіх/помилка)
   - Result та Result<T> записи (records)
   - Match паттерн для обробки результатів

5. **IExpenseRepository** - Контракт репозиторію витрат
   - Add(), GetAll(), GetById(), GetByCategory(), GetByDateRange()

6. **ICategoryRepository** - Контракт репозиторію категорій
   - Add(), GetAll(), GetById(), GetByName()

#### Application Layer (ExpenseTracker.Application)
7. **ExpenseService** - Сервіс для управління витратами
   - AddExpense(), GetAllExpenses(), GetExpensesByCategory()
   - GetExpensesByMonth(), GetExpensesByDateRange()
   - GetCategoryStatistics() - LINQ GroupBy/Sum
   - GetTotalExpenses(), GetAverageExpense()
   - GetMaxExpense(), GetMinExpense()
   - Демонструє DI через конструктор

#### Infrastructure Layer (ExpenseTracker.Infrastructure)
- **InMemoryExpenseRepository** - In-memory реалізація
- **InMemoryCategoryRepository** - 8 вбудованих категорій

#### Console Layer (ExpenseTracker.Console)
- **Program.cs** - Меню-драйвен інтерфейс
- 5 основних функцій: Add, ViewMonthly, ViewByCategory, ViewAll, Statistics

### ✓ Архітектура

```
┌─────────────────────────────────────────┐
│         Console UI Layer                │
│  - Меню, обробка команд                 │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│      Application Service Layer          │
│  - ExpenseService                       │
│  - Бізнес-логіка (LINQ, статистика)    │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│       Domain Model Layer                │
│  - Expense, Category                    │
│  - Бізнес-правила (валідація)           │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│  Infrastructure & Repository Layer      │
│  - In-memory репозиторії                │
│  - Зберігання даних                     │
└─────────────────────────────────────────┘
```

### ✓ Диаграми
- **docs/class-diagram.md** - Mermaid діаграма класів
- **docs/sequence-diagram.md** - Діаграми послідовності для головних сценаріїв

### ✓ Тестування
Реалізовано **10 юніт-тестів** (мінімум був 5):

#### Domain Tests (5 тестів)
1. ❌ Витрата з від'ємною сумою → ArgumentException
2. ❌ Витрата з нульовою сумою → ArgumentException
3. ✓ Успішне створення витрати з коректними даними
4. ❌ Категорія з порожньою назвою → ArgumentException
5. ✓ Рівність двох категорій з однаковим ID

#### Application/Service Tests (5 тестів)
6. ✓ AddExpense повертає успіх
7. ✓ GetAllExpenses показує додані витрати
8. ✓ GetCategoryStatistics розраховує правильно
9. ✓ GetTotalExpenses розраховує правильно
10. ✓ GetAverageExpense розраховує правильно

**Результат**: All tests passed ✓

### ✓ CI/CD
- Налаштований GitHub Actions workflow (**.github/workflows/dotnet.yml**)
- Автоматичний build та test при push на main/master/develop
- Restore → Build → Test pipeline

### ✓ Конфігурація
- **.gitignore** - налаштований для .NET проекту
- **README.md** - повна документація проекту
- **docs/backlog.md** - план розвитку на всі ітерації

## Артефакти першої ітерації

```
docs/
├── vision.md                    ✓ Постановка задачі
├── backlog.md                   ✓ План ітерацій
├── class-diagram.md             ✓ UML діаграма класів
└── sequence-diagram.md          ✓ Діаграми послідовності

src/
├── ExpenseTracker.Domain/
│   ├── Category.cs              ✓ Value Object
│   ├── Expense.cs               ✓ Основна сутність
│   ├── Entity.cs                ✓ Базовий клас
│   ├── Result.cs                ✓ Result паттерн
│   ├── IExpenseRepository.cs     ✓ Контракт
│   └── ICategoryRepository.cs    ✓ Контракт
├── ExpenseTracker.Application/
│   └── ExpenseService.cs         ✓ Бізнес-логіка
├── ExpenseTracker.Infrastructure/
│   ├── InMemoryExpenseRepository.cs
│   └── InMemoryCategoryRepository.cs
└── ExpenseTracker.Console/
    └── Program.cs               ✓ Меню-UI

tests/
└── ExpenseTracker.Tests/
    └── ExpenseTrackerTests.cs   ✓ 10 юніт-тестів

.github/workflows/
└── dotnet.yml                   ✓ GitHub Actions

.gitignore                        ✓
README.md                         ✓
ExpenseTracker.sln               ✓
```

## Як працює вертикальний зріз (сценарій)

### Сценарій: Додавання витрати

```
1. User → Console (вибирає "1. Додати витрату")
2. Console → Користувач (показує список категорій)
3. User → Console (вводить категорію, суму, опис)
4. Console → ExpenseService.AddExpense()
5. ExpenseService → Expense.Create() [валідація]
6. Expense → Domain rules [перевірка суми]
7. ExpenseService → InMemoryExpenseRepository.Add()
8. Repository → In-Memory List [зберігає витрату]
9. Service → Console [Result.Success]
10. Console → User [повідомлення про успіх]
```

## SOLID принципи

✓ **S** - Single Responsibility
  - Expense вміє тільки представляти витрату
  - ExpenseService вміє тільки орхеструвати
  - Repository вміє тільки керувати даними

✓ **O** - Open/Closed
  - Легко додати новий Repository (FileSystemExpenseRepository)
  - Не потрібно змінювати Service

✓ **L** - Liskov Substitution
  - InMemoryExpenseRepository можна замінити на FileSystemExpenseRepository

✓ **I** - Interface Segregation
  - Малі фокусовані інтерфейси (IExpenseRepository, ICategoryRepository)

✓ **D** - Dependency Inversion
  - Service залежить від інтерфейсів, не від реалізації

## Використані теми курсу

✓ Основи ООП: класи, інкапсуляція, конструктори  
✓ Абстракції: інтерфейси (IExpenseRepository, ICategoryRepository)  
✓ Generics і колекції: List<T>, Dictionary<Category, decimal>  
✓ LINQ: GroupBy, Sum, Where, OrderBy, OrderByDescending  
✓ Обробка помилок: try-catch, ArgumentException  
✓ SOLID: DI через конструктор, SRP, DIP  
✓ Патерни: Repository Pattern, Service Pattern  
✓ UML діаграми: класів та послідовності  
✓ Тестування: xUnit, AAA паттерн  
✓ Рефакторинг: Result паттерн, Factory методи  

## Що готові на наступну ітерацію

- ✓ Стійка Domain модель готова до розширення
- ✓ Сервіси готові до додавання нових методів
- ✓ Репозиторії можна замінити на persistence
- ✓ Тести забезпечують захист від регресій
- ✓ Архітектура дозволяє додавати нові сценарії

## Ризики та невизначеності

1. **Потреба у persistence** - На Lab 35 потрібно буде реалізувати збереження у файл. Поточна архітектура готова до цього (simply create FileSystemExpenseRepository).

2. **Масштабованість данних** - In-memory репозиторій добре працює на Lab 34, але може потребувати оптимізацій при більших обсягах даних.

3. **Інтеграційні тести** - Основна фокус тільки на юніт-тестах. На Lab 36 буде потрібне більше інтеграційних тестів.

## Класи готові до розширення

1. **Expense**
   - WithId() готовий для репозиторію
   - Можна додати методи для редагування (Edit())
   - Можна додати методи для зберігання у файл

2. **ExpenseService**
   - Готовий додати методи DeleteExpense(), UpdateExpense()
   - Готовий для додавання асинхронних операцій

3. **Категорії**
   - Можна розширити з новими правилами валідації
   - Можна додати ієрархічні категорії

## Рекомендації для Lab 35

1. **Persistence**: Реалізувати FileSystemExpenseRepository з JSON
2. **LINQ**: Додати більше фільтрацій та сортування
3. **Validation**: Розширити валідацію на сервісному рівні
4. **Error Handling**: Більш деталізована обробка помилок
5. **More Use Cases**: Мінімум 3 завершених use cases

## Статус

🟢 **Ітерація 1 успішно завершена**

- Всі вимоги виконані
- Всі тести пройшли
- Архітектура готова до розширення
- Документація повна

---

**Передача на Lab 35**: Готово до розширення функціональності з persistence, LINQ, та додатковими use cases.
