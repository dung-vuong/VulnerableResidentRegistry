# Vulnerable Resident Registry

A government-style emergency response tool built with ASP.NET Core. Residents
with health, mobility, or isolation risk factors register once, so that when
an emergency is declared (heatwave, flood, power outage, winter storm),
responders get an automatically prioritized check-in list instead of starting
from zero.

## The problem this addresses

During heatwaves, floods, and other emergencies, cities and counties
repeatedly struggle to identify and check on elderly or disabled residents
who live alone — a gap cited in multiple post-disaster reviews. This project
lets residents (or a family member on their behalf) opt in ahead of time, so
that when an emergency hits a zone, staff already know who needs a wellness
check first and how urgently.

## Tech stack

- **ASP.NET Core 10** (MVC + Razor Views)
- **Entity Framework Core** with SQL Server (LocalDB for local development)
- **ASP.NET Core Identity** — cookie-based auth with three roles:
  `Admin`, `EmergencyCoordinator`, `CaseWorker`
- **Bootstrap 5** for layout and styling
- **xUnit** for unit testing

## Architecture

The solution follows a layered (clean) architecture, chosen to keep business
logic testable and independent of the database and UI:

```
VRR.Domain          Entities and enums. No dependencies on anything else.
VRR.Application     Interfaces, DTOs, and validation. Depends only on Domain.
VRR.Infrastructure  EF Core, DbContext, and service implementations
                    (risk scoring, emergency declaration, audit logging).
                    Depends on Domain and Application.
VRR.Web             MVC controllers, Razor views, Identity. Depends on
                    Application and Infrastructure.
VRR.Tests           xUnit tests for the Application/Infrastructure services.
```

Dependencies point inward — `Domain` knows nothing about EF Core or the web
layer, which makes the core business rules (like risk scoring) easy to unit
test in isolation.

## Key features

**Weighted risk scoring**
A resident's risk score is calculated from age, mobility status, living
alone, unmet medical needs, and environmental resilience (air conditioning,
backup power). The score is stored on the resident record, not just
computed on the fly, so it can be sorted and queried efficiently and stays
consistent even if inputs change later.

**Staff verification workflow**
New registrations start as `PendingVerification` and are not eligible for
emergency response until a staff member reviews and verifies them. This
keeps unverified or fraudulent submissions out of real emergency workflows.

**Emergency declaration with automatic prioritization**
A coordinator declares an emergency by type, severity, and affected ZIP
codes. The system finds every verified, consented resident in those zones
and generates a check-in list automatically, sorted by risk score.

**Live check-in tracking**
Field staff update each check-in as Completed, Attempted, or Unable to
Reach, with notes. A progress bar on the emergency's detail page reflects
completion in real time.

**Full audit logging**
Every view of resident data and every change to a check-in is logged with
who did it, what changed, when, and from what IP address. The log records
old-to-new value changes without duplicating personal data — it's built for
compliance review, not just debugging.

**Role-based access control**
- **Admin** — full access, including the audit log
- **EmergencyCoordinator** — can declare emergencies and manage check-ins
- **CaseWorker** — can verify residents and manage check-ins, but cannot
  declare emergencies

## Running locally

**Prerequisites:** .NET 10 SDK, SQL Server LocalDB (ships with Visual
Studio), Visual Studio 2026 (or the `dotnet` CLI).

1. Clone the repository and open the solution.
2. Set your own seed admin credentials with .NET User Secrets rather than
   editing source code:
   ```
   cd VRR.Web
   dotnet user-secrets set "SeedAdmin:Email" "admin@example.local"
   dotnet user-secrets set "SeedAdmin:Password" "ChangeMe123!"
   ```
   If these secrets aren't set, the app still runs, but no admin account is
   seeded.
3. Apply the database migrations:
   ```
   dotnet ef database update --project VRR.Infrastructure --startup-project VRR.Web
   ```
4. Run the app:
   ```
   dotnet run --project VRR.Web
   ```
5. Register a resident at `/Residents/Register`, then log in with your
   seeded admin account to verify them at `/Residents/Pending` and declare
   a test emergency at `/EmergencyEvents/Declare`.

## Testing

```
dotnet test
```

Includes unit tests for the risk scoring engine, validating that
combinations of age, mobility, and environmental risk factors produce the
expected score.

## What I'd build next

- **Map-based dashboard** (Leaflet.js) showing residents color-coded by risk
  within an active emergency's affected zone
- **Geocoding on registration** so residents have real latitude/longitude
  instead of ZIP-code-only matching
- **Email confirmation and password reset**, currently stubbed out for local
  development
- **CSV/PDF export** of a completed emergency's check-in report for
  after-action review

## Why this project

I built this to learn ASP.NET Core while working toward government IT roles,
so I focused on things that matter specifically in that context: role-based
access control, an audit trail for sensitive data, a verification gate
before data is used operationally, and accessibility-minded markup — not
just CRUD.
