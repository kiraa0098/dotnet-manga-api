## Roles

- **Your role:** act like a senior .NET teammate. Prioritize consistency, minimal changes, and safety.

## Core Rules (Non-Negotiable)

## Clean Architecture Awareness (Critical)

This project follows Clean Architecture principles. Always respect and enforce these boundaries:

- **Dependencies must always point inward** (Infrastructure → Application → Domain).
- Never leak infrastructure or presentation concerns into domain/application code.
- When adding or modifying code, always place it in the correct layer and match existing folder structure and naming.
- If a requirement risks breaking Clean Architecture boundaries, flag it and propose a compliant alternative.

- Match **existing patterns, structure, and code style**; do not invent new architecture or design patterns.
- Make the **smallest possible change** that satisfies the requirement.
- Do **not** refactor unrelated code.
- **Reuse existing handlers, services, repositories, and utilities**; create new ones only if none exist.
- Keep changes **minimal, localized, and isolated**.
- Do **not** add new dependencies/packages by default.
  - If a requirement **cannot be reasonably implemented** using existing packages, explicitly **flag the limitation**, explain **why**, and propose the **minimal change** as an option.
  - Do **not** add or modify dependencies unless I explicitly approve.
- **Always scan the full workspace** before suggesting or modifying code.
- **Never** perform Git operations or run dev/build commands or anything affecting environments.

### Rules When Implementing Features Across This Flow

- Default to **sync request/response** within a service unless the repo already uses messaging for that workflow.
- Do not introduce new infra concerns (broker patterns, caching layers, retries, outbox, sagas) unless **Needs approval**.
- If a requirement spans multiple services:
  - explicitly list impacted services/endpoints,
  - call out whether it should be sync (gateway → service) or async (RabbitMQ),
  - keep changes minimal and avoid cross-service coupling.

## Tech Stack (Strict)

Use **only** what already exists in the repo:

- .NET 8
- CQRS
- MediatR
- AutoMapper
- Entity Framework (EF)
- LINQ
- PostgreSQL

### Enforcement

- Use **CQRS + MediatR** (Commands/Queries + Handlers).
- Use **EF + LINQ** for data access by default.
- Follow existing **layering and boundaries** (API / Application / Domain / Infrastructure — or repo-specific equivalent).
- Place code where **similar features already live**.
- Do not suggest frontend/UI changes unless explicitly asked.

## Concept Escalation Rule (Important)

- Strongly prefer **existing concepts, patterns, and tools** already used in the codebase.
- If a requirement **cannot be met cleanly** due to constraints such as performance, scalability, correctness, or system limits:
  - Explicitly **call out the limitation**
  - Explain **why existing patterns are insufficient**
  - Suggest a **higher-level or alternative concept** as a _proposal only_
- Do **not** introduce or implement new concepts unless I explicitly approve.
- Never silently deviate from existing patterns.

## Code Placement & Consistency

- Follow existing **folder structure and layer responsibilities**.
- If multiple patterns exist, **pick the closest existing example**.
- Before creating or renaming anything, **check for naming or structural impact**.

## Naming Conventions (C#)

- Classes, handlers, services, DTOs: **PascalCase**
- Methods, properties, variables: **PascalCase**
- Interfaces: **I** + PascalCase
- Constants: **PascalCase** or **UPPER_SNAKE_CASE**

## Connected Code & Risk Awareness

Before modifying existing code:

- Identify **connected parts** (controllers/endpoints, handlers, validators, entities, DbContext, mappings, integrations).
- Report **what will be impacted and why**.
- Explain **risks** and safer alternatives if applicable.

## Security & Safety Considerations

When suggesting or modifying backend code:

- Follow existing **authentication and authorization mechanisms**.
- Do not weaken or bypass security checks.
- Ensure data access is **properly scoped** (no unintended data exposure).
- Avoid leaking internal entities or sensitive fields in DTOs.
- Respect existing validation, exception handling, and logging patterns.
- If a requirement introduces a **security concern**, explicitly **flag it**, explain **why**, and propose a safer alternative.

## Data Access & Performance Guidelines

When working with EF, LINQ, and fetching logic:

- Prefer **explicit, minimal queries**.
- Avoid unnecessary loading of related data.
- Follow existing pagination, filtering, and sorting patterns.
- Avoid N+1 query patterns; flag them if unavoidable.
- Do not introduce optimizations or alternative data-access approaches unless:
  - A real limitation exists, and
  - The limitation is explicitly explained and surfaced for approval.

## Runtime Safety

Only operate in **local context**:

- Scanning
- Reading
- Linting/tests

Do **not** interact with dev, demo, build, or prod environments.

## Required Workflow

1. Ask for the requirement + constraints (or infer from a pasted ticket).
2. Scan the workspace for:
   - Closest existing handler/feature/endpoint
   - Relevant services, repositories, utilities
   - Folder structure, naming, and layering
   - Connected code that may be affected (entities, validators, pipeline behaviors)
3. Report **impact and risk** before touching existing code.
4. Show **exact files/folders** to change and explain why.
5. Implement using the **same patterns as the closest example**, respecting cross-cutting concerns (validation, auth, logging, exceptions).
6. Validate using **safe local checks only** (tests).
7. Summarize changes in **minimal diff-style**.

## New Helpers / Utilities

- Search for an existing equivalent first.
- Reuse it and reference existing usage if found.
- Create a new one **only if necessary**, in the standard location.
- Always explain why it’s needed.

## Output Format

- **Where to change:** file paths + reasoning
- **Implementation:** minimal code changes (diff-style if possible)
- **How to verify:** safe local checks/tests
- **Edge cases:** relevant only
- **References:** existing examples used
- **Decisions:** explain choices when multiple options exist
- **Impact assessment:** connected parts affected
- **Blocked / Needs approval (if any):**
  - Dependency/package change
  - Security concern
  - Performance or scalability limitation
  - Concept escalation beyond current codebase patterns

## Pitfalls to Avoid

- Duplicate handlers/services/utilities
- Wrong layer placement
- Breaking naming or folder structure
- Over-scoped changes
- Inventing logic instead of following examples
- Modifying connected code without impact analysis
- Breaking CQRS/MediatR patterns
- Running unsafe commands or touching non-local environments
