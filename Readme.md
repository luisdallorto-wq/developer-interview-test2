# Developer Test - Rebate Calculation

## Overview

This solution implements a rebate calculation system using a strategy-based design where each incentive type has its own calculator implementation.

The system is structured to follow clean architecture principles:

* `RebateService` orchestrates the flow
* Individual calculators implement `IRebateCalculator`
* Data stores are abstracted via interfaces
* Unit tests validate both orchestration and business logic

---

## Running the Application (Console Runner)

You can run the application using **Visual Studio**:

1. Open the solution in Visual Studio.
2. Set **`DeveloperTest.Runner`** as the startup project.
3. Run the project.

The application will display a prompt asking you to input:

* Rebate Identifier
* Product Identifier
* Volume

⚠️ **Important:**

The rebate calculation will fail when running the console application.

This is expected behavior because the logic to properly retrieve the rebate and product from a real data source has not been implemented or modified. The default data store implementation does not contain actual records matching user input.

---

## Recommended Way to Test the Application

The correct and reliable way to test the system is by running the unit tests.

All business logic is fully validated through automated tests located in:

```
DeveloperTest.Tests
```

There are two test files:

### 1️⃣ PaymentServiceTests

This test class verifies:

* Failure scenarios (rebate not found, product not found, no calculator available)
* Service orchestration logic
* Proper interaction with mocked dependencies

Mocks are used for:

* `IRebateDataStore`
* `IProductDataStore`
* `IRebateCalculator` (when testing service behavior only)

---

### 2️⃣ RealRebateCalculatorTests

This test class verifies the **actual business logic** of the three rebate calculators:

* `FixedRateRebateCalculator`
* `AmountPerUomCalculator`
* `FixedCashAmountCalculator`

In these tests:

* The real `RebateService` is used
* The real calculator implementations are used
* Only external dependencies (data stores) are mocked

This ensures that:

* Each calculator performs the correct calculation
* The strategy selection logic works properly
* The final rebate amount is correctly computed

---

## Running the Tests

To run the tests in Visual Studio:

1. Build the solution.
2. Open **Test Explorer**.
3. Run all tests inside `DeveloperTest.Tests`.

All tests should pass.

---

## Design Notes

* The system uses the **Strategy Pattern** for rebate calculation.
* The service follows **Dependency Injection principles**.
* Unit tests follow proper isolation practices:

  * Mock external dependencies
  * Test real business logic where appropriate

---

## Summary

* The console runner demonstrates user interaction but is not intended as the primary validation mechanism.
* The **unit tests are the authoritative way to verify correctness** of the application.

For evaluation purposes, please review and execute the tests inside:

```
DeveloperTest.Tests
```
