# SumitKumar.Maui

.NET MAUI Blazor Hybrid app that re-uses the public portfolio Razor components
from `SumitKumar.Shared` to deliver a rich on-device experience on Android,
iOS, macOS, and Windows.

## Prerequisites

```powershell
dotnet workload install maui
```

For iOS/macOS you additionally need Xcode (macOS) or a paired Mac.

## Add to the solution

This project is intentionally not added to `SumitKumar.slnx` by default so the
solution can build on machines without the MAUI workload. Once the workload is
installed, run:

```powershell
cd C:\GitHub\SumitKumar
dotnet sln SumitKumar.slnx add src/SumitKumar.Maui/SumitKumar.Maui.csproj
dotnet build src/SumitKumar.Maui/SumitKumar.Maui.csproj
```

## Configuration

Edit `appsettings.json` (embedded as resource) and update `Api:BaseAddress` to
the URL of the deployed `SumitKumar.Web` host that exposes `/api/blog` and
`/api/contact`. The `Portfolio` section is also bound so the same Razor
components render identically on mobile.

## Mobile capabilities

Because this is MAUI, the same Razor pages can use native device features
through `Microsoft.Maui.Essentials` (geolocation, sharing, contacts, camera,
biometrics, etc.) by injecting them into your Blazor components.
