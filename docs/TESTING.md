# TESTING.md - Стратегія тестування ExpenseTracker

## Огляд

Цей документ описує стратегію тестування для проекту ExpenseTracker, включаючи unit tests, integration tests, code coverage та quality gates.

## Архітектура тестування

### Шари тестування

```
┌─────────────────────────────────────┐
│         Integration Tests           │
│  - Service + Repository interaction │
│  - File I/O operations             │
│  - Error handling scenarios        │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│       Unit Tests (Application)      │
│  - ExpenseService business logic    │
│  - LINQ operations                 │
│  - Validation rules                │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│       Unit Tests (Domain)           │
│  - Expense, Category entities       │
│  - Business rules validation       │
│  - Value objects equality          │
└─────────────────────────────────────┘
```

## Unit Tests

### Domain Layer Tests (5 тестів)
- **Expense validation**: негативні суми, нульові суми, коректні дані
- **Category validation**: порожні назви, рівність об'єктів

### Application Layer Tests (8 тестів)
- **ExpenseService operations**: додавання, отримання, статистика
- **LINQ operations**: GroupBy, Sum, Where, OrderBy
- **Persistence operations**: збереження/завантаження JSON

## Integration Tests

### Service + Repository Integration (7 тестів)
- **In-memory integration**: додавання та отримання через сервіс
- **File system integration**: збереження та завантаження з JSON
- **Search and sort integration**: комбіновані операції
- **Statistics integration**: розрахунки по категоріям

### Fault Scenarios (3 тести)
- **File access errors**: недоступні шляхи, неіснуючі файли
- **Data validation errors**: невалідні витрати
- **JSON serialization errors**: пошкоджені файли

## Code Coverage

### Цілі покриття
- **Загальне покриття**: мінімум 60%
- **Domain layer**: 100% (критичні бізнес-правила)
- **Application layer**: 80% (бізнес-логіка)
- **Infrastructure layer**: 70% (репозиторії)

### Виключення з покриття
- Generated code (авто-генеровані файли)
- Test code (самі тести)
- External dependencies (фреймворки)

## Quality Gates

### CI/CD Gates
- ✅ **Build success**: проект компілюється без помилок
- ✅ **All tests pass**: всі unit та integration тести проходять
- ✅ **Code coverage >= 60%**: мінімальний поріг покриття
- ✅ **No critical errors**: відсутність критичних помилок

### Автоматичні перевірки
- Code coverage threshold check
- Test results validation
- Build artifacts generation

## Test Execution

### Локальний запуск
```bash
# Запуск всіх тестів
dotnet test

# Запуск з покриттям
dotnet test --collect:"XPlat Code Coverage"

# Запуск конкретного тесту
dotnet test --filter "TestName"
```

### CI/CD Pipeline
- **Trigger**: Push/PR на основні гілки
- **Steps**:
  1. Restore dependencies
  2. Build project
  3. Run tests with coverage
  4. Generate coverage report
  5. Check coverage threshold
  6. Upload artifacts

## Test Data Management

### Test Fixtures
- **In-memory repositories**: для швидких unit тестів
- **Temporary files**: для integration тестів з файлами
- **Mock data**: фіксовані витрати та категорії

### Cleanup
- Automatic file deletion after tests
- Temporary directory management
- Resource disposal

## Error Handling Testing

### Tested Scenarios
- **File operations**: UnauthorizedAccess, IOException, FileNotFound
- **JSON operations**: JsonException, deserialization failures
- **Business rules**: ArgumentException, validation failures
- **Data integrity**: null checks, empty collections

### Result Pattern
- **Success cases**: Result.Ok()
- **Failure cases**: Result.Fail(message)
- **Exception handling**: try-catch з meaningful messages

## Performance Considerations

### Test Execution Time
- Unit tests: < 100ms each
- Integration tests: < 1s each
- Full suite: < 30s

### Resource Usage
- Memory: minimal heap usage
- Disk: temporary files only
- Network: none (offline testing)

## Maintenance

### Test Organization
- **Naming convention**: [ClassName]_[MethodName]_[Scenario]
- **Grouping**: логічні класи тестів
- **Documentation**: XML comments для кожного тесту

### Refactoring Impact
- Tests as documentation of expected behavior
- Regression prevention
- Refactoring safety net

## Metrics and Reporting

### Coverage Report
- HTML report generation
- Cobertura XML for CI tools
- Branch and line coverage

### Test Results
- JUnit XML output
- GitHub Actions integration
- Detailed failure messages

## Future Enhancements

### Planned Improvements
- **UI Testing**: Console application testing
- **Load Testing**: Performance under load
- **Mutation Testing**: Test quality validation
- **Property-based Testing**: FsCheck integration

---

**Last Updated**: May 13, 2026
**Coverage Target**: 60%
**Tests Count**: 20 (13 unit + 7 integration)