# PaymentApp

A small ASP.NET Core MVC app that validates payments against a blacklist and stores every attempt (accepted or rejected) in SQL Server. IBANs are encrypted at rest and decrypted only for display.

## Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10 (SQL Server provider)
- SQL Server (via Docker)
- Tailwind CSS (Play CDN)

## Features

- Server-side validation: non-empty name, positive amount, IBAN format + country code + length + **mod 97 checksum**
- Blacklist check against a local DB table
- AES-encrypted IBAN storage (reversible)
- Transactions page: paginated (10/page), sorted by date, with summary cards (total / accepted / rejected) and top rejection reasons
- Blocked senders page
- Lightweight frontend validation in [wwwroot/js/site.js](wwwroot/js/site.js) (submit button disabled until fields are filled)

## Setup

### 1. Start SQL Server in Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Password" \
  -p 1433:1433 --name paymentapp-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Configure secrets (encryption keys + connection string)

Connection string lives in [appsettings.json](appsettings.json). Encryption key/IV are stored via .NET User Secrets so they never hit the repo:

```bash
dotnet user-secrets set "Encryption:Key" "<base64 32-byte key>"
dotnet user-secrets set "Encryption:IV"  "<base64 16-byte IV>"
```

Generate values with:

```bash
openssl rand -base64 32   # key
openssl rand -base64 16   # IV
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run

```bash
dotnet run
```

Open `https://localhost:<port>` — see console output for the exact port.

## Dev-only seeding

On startup in **Development** only, [Program.cs](Program.cs) seeds the blacklist table with 8 test names (John Doe, Jane Smith, Viktor Petrov, ...). Seeding is **idempotent** [DbSeeder.cs](Data/DbSeeder.cs) checks `if (await db.BlacklistedPersons.AnyAsync()) return;` so running the app repeatedly won't duplicate rows. Production never seeds.

To trigger a blacklist rejection, submit a payment with one of the seeded names.

## Routes

| Path              | Purpose                  |
| ----------------- | ------------------------ |
| `/`               | Payment form             |
| `/process` (POST) | Submit a payment         |
| `/transactions`   | Paginated list + summary |
| `/blocked`        | Current blacklist        |

## Project layout

```
Controllers/   Payment, Transactions, Blocked
Services/      PaymentService, ValidationService, BlacklistService, EncryptionService
Models/        Transaction, BlacklistedPerson
Data/          AppDbContext, DbSeeder
Views/         Razor views (Tailwind CDN)
```
