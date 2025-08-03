## Awesome Bank GIC Project

A simple, console-based banking system built in .NET 7.0 that simulates transactions, calculates interest based on custom rules, and generates account statements. This project is designed to be testable and extensible without any database dependency.

---

## Project Structure
AwesomeBankingApp/
│
├── Models/
│ ├── Transaction.cs # Represents a deposit/withdrawal/interest entry
│ └── InterestRule.cs # Represents interest rules with effective dates
│
├── Services/
│ ├── TransactionService.cs # Handles transaction storage and retrieval
│ ├── InterestService.cs # Manages interest rules
│ └── StatementService.cs # Generates monthly account statements
│
├── Program.cs # Entry point: CLI to interact with the system
├── BankingApp.cs # Core menu/flow controller
└── README.md # Project documentation

AwesomeBankingApp.Tests/
│
├── Tests/
│ ├── TransactionServiceTests.cs
│ ├── InterestServiceTests.cs
│ └── StatementServiceTests.cs


## Features

- Add deposit/withdrawal transactions
- Define and manage monthly interest rules
- Auto-generate interest at end-of-month
- Print monthly statements with running balances
- Console-based interaction
- Fully unit-tested (xUnit)

## Tech Stack

- [.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- C#
- xUnit for Unit Testing

## How to Run

## 1. Clone the Repository

git clone https://github.com/your-username/awesome-banking-gic.git
cd awesome-banking-gic


### 2. Open in Visual Studio
Open the AwesomeBankingApp.sln file

Set AwesomeBankingApp as the startup project

### 3. Run the App
Press F5 or Ctrl + F5 to launch the console UI

## Running Tests
You can run tests using Visual Studio Test Explorer or CLI:

dotnet test

## Sample Functionalities
1. Add transaction: Deposit ₹1000 on 2025-05-01
2. Add interest rule: From 2025-05-01, Interest Rate = 5%
3. Generate Statement for May 2025 → includes computed interest for that month

## Notes
1. No database is used — all data is kept in memory.
2. All logic is testable and modular.
3. Interest is calculated daily, but applied at end of month
