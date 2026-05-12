# Test Strategy - Детальна стратегія тестування

## 1. Вступ

Цей документ деталізує стратегію тестування для ExpenseTracker, фокусуючись на Lab 36 вимогах якості.

## 2. Тестові рівні

### 2.1 Unit Testing
**Мета**: Перевірити ізольовану функціональність окремих компонентів.

**Об'єкти тестування**:
- Domain entities (Expense, Category)
- Application services (ExpenseService)
- Infrastructure repositories

**Інструменти**: xUnit, AAA pattern

### 2.2 Integration Testing
**Мета**: Перевірити взаємодію між компонентами системи.

**Сценарії**:
- Service ↔ Repository interaction
- File I/O operations
- Error propagation

**Інструменти**: xUnit, temporary files

## 3. Code Coverage Strategy

### 3.1 Coverage Metrics
- **Line Coverage**: > 60%
- **Branch Coverage**: > 50%
- **Method Coverage**: > 70%

### 3.2 Coverage Goals by Layer
```
Domain Layer:       100% (business rules)
Application Layer:   80% (service logic)
Infrastructure:      70% (repositories)
Console UI:          50% (main flow)
```

### 3.3 Exclusions
- Test classes themselves
- Generated code
- External libraries
- Exception paths (where appropriate)

## 4. Test Case Design

### 4.1 Equivalence Classes
- **Valid Expenses**: amount > 0, ≤ 1,000,000
- **Invalid Expenses**: amount ≤ 0, > 1,000,000
- **Valid Categories**: non-empty names
- **Invalid Categories**: empty/null names

### 4.2 Boundary Values
- Amount: 0.01, 1, 1000000, 1000000.01
- Dates: MinValue, MaxValue, Today
- Strings: Empty, Whitespace, Unicode

### 4.3 Error Conditions
- File not found
- Access denied
- Invalid JSON
- Network errors (future)
- Database errors (future)

## 5. Test Data Strategy

### 5.1 Test Data Sources
- **Static Data**: Predefined categories
- **Dynamic Data**: Generated expenses
- **File Data**: JSON fixtures

### 5.2 Data Management
- **Isolation**: Each test independent
- **Cleanup**: Automatic resource disposal
- **Fixtures**: Shared setup for related tests

## 6. Automation Strategy

### 6.1 CI/CD Integration
- **Triggers**: Push to main branches
- **Tools**: GitHub Actions, Coverlet
- **Reports**: HTML coverage, JUnit results

### 6.2 Quality Gates
- Build passes
- All tests pass
- Coverage ≥ 60%
- No critical vulnerabilities

## 7. Risk-Based Testing

### 7.1 High Risk Areas
- **Financial Calculations**: Sum, Average, Statistics
- **Data Persistence**: File I/O, JSON serialization
- **Business Rules**: Validation, Constraints

### 7.2 Risk Mitigation
- Comprehensive unit tests for calculations
- Integration tests for persistence
- Fault injection for error handling

## 8. Performance Testing

### 8.1 Performance Criteria
- Unit test: < 100ms
- Integration test: < 1s
- Full suite: < 30s

### 8.2 Load Testing (Future)
- Large datasets (1000+ expenses)
- Concurrent operations
- Memory usage monitoring

## 9. Maintenance Strategy

### 9.1 Test Code Quality
- Clear naming conventions
- Comprehensive documentation
- DRY principle application

### 9.2 Refactoring Support
- Tests as specification
- Regression detection
- Safe refactoring enablement

## 10. Success Criteria

### 10.1 Test Quality Metrics
- Test pass rate: 100%
- Coverage: ≥ 60%
- Defect detection rate: High
- False positive rate: Low

### 10.2 Process Metrics
- Test execution time: < 30s
- CI/CD pipeline time: < 5min
- Manual intervention: Minimal

## 11. Tools and Frameworks

### 11.1 Testing Framework
- **xUnit**: Test execution
- **Coverlet**: Code coverage
- **ReportGenerator**: Coverage reports

### 11.2 CI/CD Tools
- **GitHub Actions**: Pipeline automation
- **.NET CLI**: Build and test commands

### 11.3 Analysis Tools
- **SonarQube** (future): Code quality
- **Mutation Testing** (future): Test quality

## 12. Test Execution Plan

### 12.1 Daily Execution
- Pre-commit hooks: Fast unit tests
- CI Pipeline: Full test suite + coverage

### 12.2 Release Execution
- Full regression test suite
- Performance validation
- Coverage verification

## 13. Defect Management

### 13.1 Bug Classification
- **Critical**: Data loss, incorrect calculations
- **Major**: Functionality breaks
- **Minor**: UI issues, edge cases

### 13.2 Root Cause Analysis
- Test case enhancement
- Code fix verification
- Regression prevention

## 14. Continuous Improvement

### 14.1 Metrics Collection
- Coverage trends
- Test execution times
- Defect rates
- Maintenance effort

### 14.2 Process Optimization
- Test automation improvements
- Tool upgrades
- Best practice adoption

---

**Document Version**: 1.0
**Last Updated**: May 13, 2026
**Next Review**: End of Lab 36