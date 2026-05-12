# ExpenseTracker - Backlog

## Огляд проєкту по ітераціях

Цей документ визначає план розробки проєкту ExpenseTracker на всі чотири лабораторні роботи та самостійну роботу.
## Ітерація 1 (Lab 34) - Постановка задачі, модель і архітектурний каркас
**Статус**: Завершено

### Завдання:
- [x] Визначити предметну область та написати vision.md
- [x] Створити діаграми класів і послідовності
- [x] Ініціалізувати solution з коректною структурою
- [x] Реалізувати Domain layer з 5-8 класами
- [x] Реалізувати контракти (інтерфейси репозиторіїв)
- [x] Реалізувати перший вертикальний зріз
- [x] Додати базові юніт-тести (мінімум 5)
- [x] Налаштувати GitHub Actions CI

### Очікувані артефакти:
- [x] docs/vision.md
- [x] docs/backlog.md
- [x] docs/class-diagram.md
- [x] docs/sequence-diagram.md
- [x] Робоча консольна програма з одним повним сценарієм
- [x] Мінімум 5 юніт-тестів
- [x] Налаштований CI/CD (GitHub Actions)
- [x] .gitignore та README
- [x] docs/iteration-1.md (передача на наступну ітерацію)

### Scope:
- **Domain**: Category, Expense, можливо User (для майбутнього)
- **Scenario**: Додавання витрати та перегляд витрат за період
- **Storage**: In-memory (тимчасово)
- **Validation**: Базова перевірка введення

## Ітерація 2 (Lab 35) - Розширення use cases, Persistence, LINQ
**Статус**: Завершено

### Завдання:
- [x] Реалізувати persistence (збереження у файл JSON)
- [x] Додати LINQ-запити для фільтрації та сортування
- [x] Реалізувати мінімум 3 use cases
- [x] Додати обробку помилок при роботі з файлами
- [x] Розширити набір тестів
- [x] Оновити діаграми та backlog

### Очікувані функції:
- Фільтрація витрат за категоріями
- Сортування витрат за датою та сумою
- Експорт витрат у файл
- Імпорт витрат з файлу
- Пошук витрат за описом

### Scope:
- **Persistence**: JSON файли для збереження даних
- **Queries**: LINQ для складних фільтрацій
- **Patterns**: Repository Pattern більш детально, можливо Factory
- **Storage**: FileSystemExpenseRepository

## Ітерація 3 (Lab 36) - Quality Gate, Unit/Integration Tests, Fault Handling

**Статус**: Не розпочата

### Завдання:
- [ ] Написати integration tests
- [ ] Реалізувати comprehensive error handling
- [ ] Встановити code coverage (мінімум 60%)
- [ ] Налаштувати Quality Gate у CI
- [ ] Документувати тестову стратегію

### Очікувані артефакти:
- [ ] TESTING.md (тестова стратегія)
- [ ] test-strategy.md (детальна стратегія)
- [ ] test-matrix.md (матриця тестів)
- [ ] CI з тестами та quality gate
- [ ] Code coverage report

### Scope:
- **Unit Tests**: Domain и Application layer
- **Integration Tests**: Repository и Service interaction
- **Fault Scenarios**: Тестування помилкових сценаріїв
- **Coverage**: Мінімум 60% code coverage

## Ітерація 4 (Lab 37) - Release Hardening, Документація, Demo

**Статус**: Не розпочата

### Завдання:
- [ ] Написати USER_GUIDE.md (інструкція користувача)
- [ ] Написати DEVELOPER_GUIDE.md (інструкція розробника)
- [ ] Створити CHANGELOG.md
- [ ] Написати FINAL_REPORT.md
- [ ] Підготувати DEMO.md (сценарій демонстрації)
- [ ] Написати syllabus-coverage.md (покриття тем курсу)

### Очікувані артефакти:
- [ ] USER_GUIDE.md
- [ ] DEVELOPER_GUIDE.md
- [ ] CHANGELOG.md
- [ ] FINAL_REPORT.md
- [ ] DEMO.md
- [ ] syllabus-coverage.md
- [ ] Release version в репозиторії

### Scope:
- **Documentation**: Повна документація для користувачів та розробників
- **Demo**: Запис або крок-за-кроком сценарій демонстрації
- **Release**: Чиста версія коду готова до релізу

## Самостійна робота 29 - Закриття прогалин та підготовка захисту

**Статус**: Не розпочата

### Завдання:
- [ ] Виконати self-audit проєкту
- [ ] Визначити невикористані теми курсу
- [ ] Планувати розширення для закриття прогалин
- [ ] Реалізувати цільові розширення
- [ ] Підготувати defense-checklist

### Очікувані артефакти:
- [ ] self-audit.md
- [ ] extension-plan.md
- [ ] extension-report.md
- [ ] defense-checklist.md

### Можливі розширення:
- Делегати та pipeline-поведінка для меню
- HashSet, Queue для оптимізації операцій
- Асинхронний I/O для роботи з файлами
- Observer pattern для сповіщень
- Decorator pattern для форматування витрат

### Обов'язкові на основному шляху:
- ✓ Основи ООП: класи, інкапсуляція, конструктори
- ✓ Абстракції: інтерфейси
- ✓ Generics і колекції (List<T>, Dictionary<K,V>)
- ✓ LINQ (GroupBy, Where, Sum)
- ✓ Обробка помилок (try-catch)
- ✓ SOLID принципи (Dependency Injection, Single Responsibility)
- ✓ Мінімум два патерни (Repository, Service)
- ✓ UML діаграми
- ✓ Тестування (Unit Tests)
- ✓ Рефакторинг

### Бажані через розширення:
- Делегати і pipeline-поведінка
- HashSet, Queue, Stack, кеші
- Асинхронний I/O
- Retry-політики
- Observer, Decorator, Adapter, Facade, Proxy
- Custom extensions для LINQ

## Критерії успіху кожної ітерації

### Lab 34
- [ ] Одна робоча функція (додавання витрати) через усі шари
- [ ] Мінімум 5-8 доменних класів
- [ ] Мінімум 5 юніт-тестів
- [ ] Всі обов'язкові артефакти

### Lab 35
- [ ] Persistence реалізований
- [ ] Мінімум 3 use cases
- [ ] LINQ-запити працюють
- [ ] Розширений набір тестів

### Lab 36
- [ ] Code coverage >= 60%
- [ ] Integration tests наявні
- [ ] Fault handling реалізований
- [ ] Quality gate налаштований

### Lab 37
- [ ] Повна документація
- [ ] Демонстрація готова
- [ ] Проєкт готовий до захисту

## Залежності між ітераціями

```
Lab 34 (Vision, Domain, Basic Service)
   ↓
Lab 35 (Persistence, LINQ, More Use Cases)
   ↓
Lab 36 (Tests, Quality Gate, Fault Handling)
   ↓
Lab 37 (Documentation, Release)
   ↓
Lab 29 (Extensions, Final Polish)

