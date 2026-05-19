# SumitKumar — Personal Portfolio (.NET 10 LTS)

A Blazor Web App (Server + WebAssembly, Auto interactivity) plus an optional
.NET MAUI Blazor Hybrid client, built on Clean Architecture principles.

| Surface | Technology | Audience |
| ------- | ---------- | -------- |
| Public portfolio | Blazor Web App (SPA experience) | Visitors |
| Admin console    | AdminLTE v4-inspired Blazor layout | Owner |
| Mobile / desktop | .NET MAUI Blazor Hybrid (shared Razor components) | Owner & visitors |

The public site is inspired by the [iPortfolio Bootstrap 5 theme](https://themewagon.com/themes/iportfolio/)
and the admin area uses an AdminLTE v4-inspired layout. Each surface is
optimised independently — they do not share design tokens.

## Solution layout

```
SumitKumar/
├─ SumitKumar.slnx                # SDK-style solution
├─ src/
│  ├─ SumitKumar.Domain/          # Entities & domain primitives
│  ├─ SumitKumar.Application/     # Services, DTOs, abstractions, options
│  ├─ SumitKumar.Infrastructure/  # EF Core (SQLite), repositories, SMTP email
│  ├─ SumitKumar.Shared/          # Razor Class Library — public Portfolio UI
│  ├─ SumitKumar.Web/             # Blazor Web App (host) + Admin area + Identity + API
│  ├─ SumitKumar.Web.Client/      # Blazor WebAssembly client (interactive)
│  └─ SumitKumar.Maui/            # MAUI Blazor Hybrid app (see project README)
├─ tests/
│  └─ SumitKumar.UnitTests/       # xUnit tests
├─ docs/                          # Architecture & ADR notes
└─ scripts/                       # Dev / CI helper scripts
```

Layer dependencies follow Clean Architecture:

```
Domain ◄── Application ◄── Infrastructure
                ▲                ▲
                └────── Web ─────┘
                        ▲
                  Web.Client / Shared / Maui
```

## Features

### Public site (`/`)
- iPortfolio-inspired single-page layout with fixed sidebar nav
- Hero, About, Stats, Skills, Resume, Services, Portfolio, Contact, Blog
- Contact form posts to `/api/contact` → persisted + emailed
- Blog index and detail at `/blog` and `/blog/{slug}` (consumes `/api/blog`)
- `Admin` link in nav routes to the secure admin area

### Admin (`/admin`, requires login)
- AdminLTE v4-inspired sidebar + topbar with **Profile / Logout** dropdown
- Dashboard with counters
- Blog management (list, create, edit, publish/draft, delete)
- Contact messages inbox
- Profile page (config-driven)

### APIs (`/api`)
- `GET  /api/blog` — published posts
- `GET  /api/blog/{slug}` — post detail
- `POST /api/contact` — submit a contact message (persisted + emailed)

### Cross-cutting
- **Configuration-first**: portfolio content, contact info, SMTP, and
  connection strings live in `appsettings.json` and are bound via the
  Options pattern with DataAnnotation validation at startup.
- **Identity**: ASP.NET Core Identity (cookies, optional passkeys).
- **Persistence**: EF Core 10 + SQLite (swap providers via DI).
- **Accessibility & responsiveness**: Bootstrap 5 + Bootstrap Icons across
  browsers on Windows/macOS/Linux/iOS/Android.

## Getting started

```powershell
cd C:\GitHub\SumitKumar
dotnet build SumitKumar.slnx
dotnet run --project src/SumitKumar.Web
```

The first run creates `Data/app.db` (Identity) and `Data/blog.db` (Blog). Open
`https://localhost:5001/Account/Register` to create your admin user, then
navigate to `/admin`.

### MAUI client

See [src/SumitKumar.Maui/README.md](src/SumitKumar.Maui/README.md) — requires
`dotnet workload install maui`.

## Configuration

All runtime configuration lives in `src/SumitKumar.Web/appsettings.json`:

| Section            | Purpose                                       |
| ------------------ | --------------------------------------------- |
| `ConnectionStrings.DefaultConnection` | Identity SQLite DB         |
| `ConnectionStrings.BlogConnection`    | Blog/contact SQLite DB     |
| `Email`            | SMTP settings + inbox address for contact form |
| `Portfolio`        | All public profile content (bind once, edit safely) |

Secrets (SMTP passwords, etc.) belong in **user-secrets**, environment
variables, or a key vault — never commit them. Local-only overrides may go in
`appsettings.Local.json` (already in `.gitignore`).

## Tests

```powershell
dotnet test
```

## License

Personal portfolio — © Sumit Kumar.
