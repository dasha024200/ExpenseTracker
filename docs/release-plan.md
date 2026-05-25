# Release Plan - v1.0.0

## Дата підготовки
Дата: 25.05.2026

## Release Scope

### Що входить у версію v1.0.0

#### ✅ Core Functionality (Lab 34-35)
- Додавання, видалення, редагування витрат
- Категоризація витрат (8 категорій: Їжа, Транспорт, Розваги, Навчання, Медицина, Комунальні, Досуг, Інше)
- Фільтрація за категорією та датою
- Сортування за датою та сумою
- Пошук за описом витрати
- Статистика по категоріям (загальна сума, середня витрата, кількість витрат)
- JSON persistence (збереження/завантаження даних)

#### ✅ Quality Assurance (Lab 36)
- 20+ unit тестів (Domain + Application layers)
- 7+ інтеграційних тестів (Service + Repository interaction)
- Code coverage: 78%
- Comprehensive error handling (файлові помилки, валідація даних, помилки JSON)
- Quality gates у CI/CD (GitHub Actions)

#### ✅ Documentation (Lab 37)
- Vision document з описом проблеми та цільових користувачів
- Architecture diagrams (class diagram, sequence diagram)
- Testing strategy та test matrix
- Developer guide з описом архітектури і розширення
- User guide з інструкціями використання
- API documentation з XML comments

### Що переноситься в "після курсу" (Post-v1.0.0)

#### Відкладені на майбутнє
- Веб-інтерфейс (WPF/Blazor)
- Бюджетні ліміти та сповіщення (Observer pattern)
- Мультикористувацький режим
- Експорт у CSV/Excel
- Графіки та діаграми витрат
- Синхронізація з хмарою
- Мобільний додаток

## Технічні борги, що допустимі

### Допустимі обмеження в v1.0.0
1. **Console UI** - простий текстовий інтерфейс (без GUI)
2. **Categories immutable** - категорії зберігаються лише в пам'яті, не в файлі
3. **Single-user** - одного користувача в памяті
4. **Sync I/O** - синхронні операції файлів (асинхронність в post-release)
5. **Limited analytics** - базова статистика без складних аналіз

### Планується закрити на Lab 37
- ✅ Code smells та дублювання
- ✅ XML documentation
- ✅ Performance profiling для LINQ запитів
- ✅ Edge cases у валідації

## Теми курсу у проекті

### Обов'язково покрито
- ✅ ООП: класи, інкапсуляція, конструктори, наслідування
- ✅ Абстракції: інтерфейси, абстрактні класи
- ✅ Generics та колекції: List<T>, Dictionary, IEnumerable
- ✅ LINQ: Where, OrderBy, GroupBy, Sum, Select
- ✅ Обробка помилок: try-catch, Result pattern
- ✅ SOLID: SRP, DIP, ISP
- ✅ Патерни: Repository, Service, Result
- ✅ UML: class diagram, sequence diagram
- ✅ Тестування: xUnit, unit тести, integration тести
- ✅ Рефакторинг: код чистий, читабельний, без code smells

### Частково покрито (розширення Lab 37)
- ⏳ Делегати та pipeline-поведінка (для меню)
- ⏳ HashSet для оптимізації пошуку
- ⏳ Custom Result<T> type для більш точної обробки помилок
- ⏳ Extension methods для LINQ

### Плануються як додаткові розширення
- Observer pattern (для сповіщень)
- Decorator pattern (для форматування)
- Adapter pattern (для нових джерел даних)
- Asynchronous I/O

## Критичні тести перед релізом

### Обов'язкові на День 1
- [x] Додавання витрати в пам'ять
- [x] Додавання витрати у файл
- [x] Завантаження витрат з файлу
- [x] Видалення витрати
- [x] Пошук витрати за описом

### Обов'язкові на День 2
- [x] Статистика коректна при множинних категоріях
- [x] Помилка при недійсній сумі
- [x] Помилка при недійсній категорії
- [x] Обробка відсутнього файлу

### Обов'язкові перед випуском
- [x] Весь код компілюється
- [x] CI/CD проходить успішно
- [x] Code coverage >= 60%
- [x] Документація актуальна

## Матриця готовності до релізу

| Компонент | Статус | Покриття | Примітка |
|-----------|--------|---------|---------|
| Domain Layer | ✅ Ready | 95% | Category, Expense, Entity |
| Application Layer | ✅ Ready | 85% | ExpenseService |
| Infrastructure Layer | ✅ Ready | 90% | Repositories, JSON persistence |
| Console UI | ✅ Ready | 60% | Main menu, input handling |
| Documentation | ✅ Ready | 100% | All docs for v1.0.0 |
| Tests | ✅ Ready | 20+ tests | Unit + Integration |
| CI/CD | ✅ Ready | Green | GitHub Actions working |

## Ризики залишені до релізу

### Низький ризик
- Можливість збою при роботі з великими файлами (> 10MB)
- Прізнання Unicode символів в описах (мало користувачів з цим)

### Мінімізовані ризики
- ✅ Файлова система недоступна → обробка помилок
- ✅ JSON пошкоджений → обробка та retry
- ✅ Недійсні дані → валідація в конструкторах

## Плани розвитку після v1.0.0

### v1.1.0 (Q3 2026)
- Web UI (Blazor)
- Budget limits
- Observer notifications

### v2.0.0 (Q4 2026)
- Multi-user support
- Cloud sync
- Advanced analytics

## Дата готовності
**Target Release Date**: 25.05.2026
**Статус**: Готово до релізу
