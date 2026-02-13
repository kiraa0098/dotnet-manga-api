# Manga API (.NET) — Clean Architecture Sample

A **.NET Web API** for fetching **manga details/metadata + chapters**.

- **Metadata source:** MangaDex API  
- **Chapter source:** Web scraping (via a .NET scraping library)  
- **Main purpose:** A practical **Clean Architecture** implementation (not “just an API wrapper”).

> ⚠️ Note: Chapter scraping depends on the target site’s HTML structure. If the site changes, scraping may break.

---

## Features

- Fetch manga metadata (title, authors, tags, cover, description, etc.) via **MangaDex**
- Fetch chapter list + chapter content via **web scraping**
- Clean Architecture structure (Domain / Application / Infrastructure / API)
- DTO mapping + separation of concerns (no “everything inside controllers”)

---

## Tech Stack

- **.NET** (ASP.NET Core Web API)
- **MangaDex API** for metadata
- **Web scraping** for chapters (using a .NET library — see `Infrastructure` implementation)
- Clean Architecture + Dependency Injection

---

## Architecture

This project follows **Clean Architecture**, roughly:

- `Domain`  
  Core entities, value objects, domain rules.  
  **No dependencies** on other projects.

- `Application`  
  Use cases / business logic (services, handlers), interfaces (ports), DTOs.  
  Depends only on `Domain`.

- `Infrastructure`  
  Implementations of external concerns:
  - MangaDex API client
  - Scraper implementation for chapters
  - persistence/cache (if added later)
  
- `API`  
  Controllers, request/response models, wiring (DI), middleware.

### Why Clean Architecture here?
Because it forces a useful separation:

- API layer doesn’t “know” how MangaDex works
- Scraper can be swapped/reworked without touching core logic
- You can unit test use-cases without HTTP calls or scraping

---

## Getting Started

### Prerequisites
- .NET SDK installed (recommended: latest LTS)
- Internet access (calls MangaDex + scraping target)

### Run locally

```bash
dotnet restore
dotnet run --project src/YourProject.API
