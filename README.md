# Vulnerable Resident Registry

A government-style emergency response tool: residents with health, mobility, 
or isolation risk factors register once, and during a declared emergency 
(heatwave, flood, power outage), staff get an automatically prioritized 
check-in list sorted by risk — addressing a real, documented gap in disaster 
response (e.g. wellness-check failures during heatwave events).

## Problem this solves
During emergencies, cities struggle to identify and prioritize checking on 
elderly or disabled residents living alone. This registry lets residents 
opt in ahead of time, so responders aren't starting from zero when an 
emergency hits.

## Tech stack
- ASP.NET Core 10 MVC, C#
- Entity Framework Core + SQL Server
- ASP.NET Core Identity (role-based: Admin, EmergencyCoordinator, CaseWorker)
- Bootstrap

## Key features
- **Risk scoring engine** — weighted score from age, mobility, isolation, 
  medical needs, and environmental resilience (AC/backup power)
- **Verification workflow** — new registrations require staff sign-off 
  before being eligible for emergency response
- **Emergency declaration** — a coordinator declares an emergency by ZIP 
  code; the system auto-generates a prioritized check-in list
- **Live check-in tracking** — staff mark residents Completed / Unable to 
  Reach, with a real-time completion percentage
- **Audit logging** — every view/change of resident data is logged with 
  who, what, when, and from what IP — built for compliance review, not 
  just debugging

## Architecture
Clean/layered architecture: Domain → Application → Infrastructure → Web, 
chosen to keep business logic (risk scoring, emergency workflows) testable 
and independent of the database or UI.

## Running locally
1. Clone the repo
2. Update the connection string in `appsettings.json` if needed (defaults 
   to LocalDB)
3. `dotnet ef database update --project VRR.Infrastructure --startup-project VRR.Web`
4. Set your own admin credentials via `dotnet user-secrets` (see 
   `Program.cs` seeding logic) or the app will skip admin seeding
5. `dotnet run --project VRR.Web`

## What I'd build next
- Map-based dashboard (Leaflet.js) for visualizing affected zones
- Geocoding on registration for real lat/long instead of ZIP-only matching
- Email confirmation + real password reset flow
