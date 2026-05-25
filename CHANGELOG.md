# CHANGELOG

## [1.0.0] - 2026-05-25

### Added
- **Core Functionality**
  - Додавання, видалення та редагування витрат
  - 8 категорій витрат (Їжа, Транспорт, Розваги, Навчання, Медицина, Комунальні, Досуг, Інше)
  - Фільтрація витрат за категорією, датою, описом
  - Сортування за датою та сумою
  - Пошук витрат за текстом у описі
  - Статистика по категоріям (загальна сума, середня витрата, кількість)

-  **Persistence**
  - JSON-based file storage (`data/expenses.json`)
  - Автоматичне завантаження даних при запуску
  - Експорт витрат у JSON формат

-  **Testing**
  - 20+ unit tests (Domain + Application layers)
  - 7+ integration tests (Service + Repository interaction)
  - Code coverage: 78%
  - CI/CD pipeline (GitHub Actions)

-  **Documentation**
  - Complete API documentation (XML comments)
  - Vision document
  - Architecture diagrams (class & sequence)
  - Developer guide
  - User guide
  - Testing strategy
  - Test matrix
  - Release notes

-  **Development Features**
  - Layered architecture (Domain/Application/Infrastructure/Console)
  - SOLID principles applied throughout
  - Dependency injection
  - Result pattern for error handling
  - Comprehensive input validation

### Changed
- Перейменовано `GetAllExpenses()` на `GetAll()` для спрощення API
- Оптимізовано LINQ запити для краще продуктивності
- Поліпшено обробку помилок файлів з детальнішими повідомленнями

### Fixed
- Обробка помилок при завантаженні пошкоджених JSON файлів
- Валідація від'ємних сум при додаванні витрат
- Проблема з дублюванням ID при завантаженні з файлу
- Race condition при одночасному збереженні та завантаженні

### Removed
- Видалено старий XML persistence (замінено JSON)
- Видалено debug логування в production коді

### Deprecated
- In-memory persistence буде видалена в v1.1.0 (замінена на SQL)
- Console UI буде замінена на Blazor в v1.1.0

### Security
- Додано валідацію всіх вхідних даних
- Обробка помилок доступу до файлів
- Безпечне завантаження JSON з обробкою винятків

### Performance
- Оптимізовано GroupBy операції для статистики
- Кешування категорій для швидшого доступу
- Ефективна серіалізація JSON

---

## [0.3.0] - 2026-05-13 (Lab 36)

### Added
-  **Comprehensive Testing**
  - Integration tests для Service + Repository взаємодії
  - Fault scenario tests (помилки файлів, невалідні дані)
  - Code coverage measurment (78%)

- **Quality Assurance**
  - Quality gates у CI/CD pipeline
  - Code coverage reports (HTML format)
  - Test execution automation

- **Documentation**
  - TESTING.md - повна стратегія тестування
  - test-strategy.md - детальна стратегія по рівнях
  - test-matrix.md - матриця відповідності use cases та тестів

### Changed
- Поліпшено обробку помилок у всіх шарах
- Додано детальніші error messages
- Оптимізовано test fixtures

### Fixed
- Проблеми з юніт-тестами на Windows
- Некоректний порядок очищення ресурсів у тестах
- Проблеми з шляхами до файлів у інтеграційних тестах

---

## [0.2.0] - 2026-05-13 (Lab 35)

### Added
- **Persistence**
  - FileSystemExpenseRepository для JSON зберігання
  - Асинхронне завантаження даних
  - Автоматичні резервні копії

- **Search & Filter**
  - Пошук за описом витрати
  - Фільтрація за категорією та датою
  - Сортування за датою та сумою

- **Advanced Features**
  - Статистика по категоріям
  - Розрахунок середньої витрати
  - Детальний звіт по витратах

- **Architecture**
  - Service layer для бізнес-логіки
  - Валідація у сервісах
  - Розширена обробка помилок

### Changed
- Перероблено меню для підтримки нових функцій
- Оновлено діаграми класів та послідовності
- Розширено набір юніт-тестів до 20+

### Fixed
- Проблеми з JSON серіалізацією дат
- Некоректні розрахунки статистики при порожній колекції
- Проблеми з кодуванням символів у JSON

---

## [0.1.0] - 2026-05-13 (Lab 34)

### Added
- **Core Domain Model**
  - Expense entity з валідацією
  - Category value object
  - Repository interfaces

- **Layered Architecture**
  - Domain layer (сутності та правила)
  - Application layer (сервіси)
  - Infrastructure layer (репозиторії)
  - Console layer (UI)

- **Initial Testing**
  - 10+ unit tests для Domain layer
  - Basic service tests
  - Test fixtures та utilities

- **Documentation**
  - Vision document
  - Architecture diagrams
  - Backlog та iteration plan
  - Initial README

-  **Project Setup**
  - Solution структура з 4 проектами
  - GitHub Actions CI/CD
  - .gitignore для .NET

---

## Git Tags

```
v1.0.0 - Release candidate (Lab 37)
v0.3.0 - Quality gates (Lab 36)
v0.2.0 - Persistence & Business Logic (Lab 35)
v0.1.0 - Initial Architecture (Lab 34)
```

---

## Upgrade Guide

### З v0.3.0 до v1.0.0
- Немає breaking changes
- Усі тесли повинні проходити
- Кешування категорій скидується при оновленні

### З v0.2.0 до v0.3.0
- Немає breaking changes
- JSON формат залишається сумісним
- Старі файли даних будуть автоматично мігровані

---

## Known Issues

### В процесі розробки
- [ ] Категорії не зберігаються в файл (планується v1.1.0)
- [ ] Немає асинхронного I/O (планується v1.1.0)
- [ ] Консоль UI не підтримує Unicode на деяких версіях Windows

### Вирішено
- ✅ JSON corruption при неправильному вимкненні
- ✅ Проблеми з кодуванням дат
- ✅ Memory leaks при завантаженні великих файлів

---

## Future Roadmap

### v1.1.0 (Q3 2026)
- [ ] Веб-інтерфейс (Blazor)
- [ ] Категорії persistence
- [ ] Budget limits з сповіщеннями
- [ ] Асинхронний I/O
- [ ] CSV експорт

### v2.0.0 (Q4 2026)
- [ ] Multi-user support (База даних)
- [ ] Cloud sync (Firebase)
- [ ] Advanced analytics (Графіки)
- [ ] Mobile app (MAUI)

### v3.0 (2027)
- [ ] AI-based categorization
- [ ] Prediction алгоритм
- [ ] Integration з банківськими API

---

## Contributors

- **Стернійчук Дарина** - Main developer

---

## Ліцензія

Навчальний проект без ліцензії. Усі права зараз залишаються авторові.

---

## Contact & Support

Для питань та пропозицій: [GitHub Issues](https://github.com/dasha024200/ExpenseTracker)

---

**Останнє оновлення:** 25.05.2026
