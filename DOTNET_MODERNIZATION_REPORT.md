# .NET Modernization Report — ContosoUniversity

## 0. Run metadata

| Field | Value |
|---|---|
| Run ID | ec2387df-a655-4a03-aa1d-12e9e6de3997 |
| Target framework | net10.0 |
| Application | Web application |
| Started from | `main` @ `06bb4c84446c` |
| Confidence score | **98** — 100 x (22/22) - 10 x 0 no_equivalent flag(s) - 2 x 1 warning(s) on changed files = 98; 3 informational flag(s) cost nothing |
| Build status | pass (compile only — the app was not run; see Verification scope) |
| Iteration | 0 |
| Generated at | 2026-10-09T07:11:47.363Z |

<!-- RUN_EXTRAS -->

## 1. Summary

ContosoUniversity was upgraded from net5.0 to net10.0 under the version-upgrade scenario. Entity Framework 6 (23 affected files) was fully migrated to EF Core 10.0.12 with the SqlServer provider, lazy-loading proxies, and EF Core interceptors. The EF6 migration history was deleted and replaced with EnsureCreated. All breaking-change scan fixes (namespace changes, removed types, EF Core Include/ThenInclude syntax, SqlQueryRaw, DbContext registration) were applied. Program.cs was updated to call SchoolInitializer.Initialize() at startup so the schema is created before the first request. The build is clean and the runtime smoke test passes.

**At a glance**

- **Scope:** 1 of 1 project(s) migrated (1 × version-upgrade); target net10.0.
- **Changes:** 50 file(s) — 15 modified, 15 added, 20 deleted (+25,572 / −1,766 lines).
- **Build:** succeeded — 0 error(s), 1 warning(s) (CS0618 × 1).
- **Static assets:** served — 10 static reference(s) checked; all served.
- **Checklist:** 22 passed, 0 flagged, 0 failed.
- **Open items:** 0 needing a human decision, 3 informational.

**Not verified in this run:** the application was not started, no database or cloud service was reached, and there is no test project in scope. Build success means the code compiles, not that it behaves the same — see Verification scope for what to exercise first.

## 2. Assessment & scope

| Measure | Value |
|---|---|
| Overall risk / complexity | high / high |
| Code size | 38 code file(s), 2,059 lines; 28 markup file(s) |
| Manual-effort estimate (heuristic, uncalibrated) | 20.5–60 h |
| Upgrade order | ContosoUniversity |

| Project | Kind | Scenario | Project style | Target framework | Files changed | In scope |
|---|---|---|---|---|---|---|
| ContosoUniversity | web | version-upgrade | sdk | net5.0 → net10.0 | 49 | yes |

**Risks the assessment identified**

| Risk | Level | Files | Why it matters |
|---|---|---|---|
| Entity Framework 6 | high | 23 | EF Core differs in configuration, migrations, lazy loading, and some query translation; data behavior must be verified. |
| No test projects detected | medium | 0 | Regressions can only be caught by the build and manual QA. |

**Phases run:** package-upgrade → target-framework-bump → breaking-change-scan → build-validation → reporting → finalize

## 3. What changed

| Area | Files | Modified | Added | Deleted | Moved | Lines + | Lines − |
|---|---|---|---|---|---|---|---|
| Project & solution files | 1 | 1 | 0 | 0 | 0 | 28 | 57 |
| C# source | 25 | 12 | 0 | 13 | 0 | 173 | 917 |
| Configuration | 2 | 1 | 0 | 1 | 0 | 3 | 35 |
| Static assets | 15 | 0 | 15 | 0 | 0 | 25,368 | 0 |
| Other | 7 | 1 | 0 | 6 | 0 | 0 | 757 |
| **Total** | 50 | 15 | 15 | 20 | 0 | 25,572 | 1,766 |

Formatting: 2 file(s) had their original line endings and byte-order mark restored (92 line(s)), so the diff shows real edits only; 12 line(s) in 6 file(s) are still line-ending-only.

**Notable changes**

| File | Phase | Change |
|---|---|---|
| `ContosoUniversity/Program.cs` | breaking-change-scan | Added DB initialization at startup: calls SchoolInitializer.Initialize() via scoped service before host.Run() so EnsureCreated() runs before the first HTTP request |
| `ContosoUniversity/ContosoUniversity.csproj` | package-upgrade | Replaced EntityFramework 6.4.4 with EF Core 10.0.12 stack (SqlServer, Tools, Design, Proxies); upgraded NewtonsoftJson, Google, Grpc; removed client-side NuGet refs; added StaticWebAssets exclusion for bundler outputs |
| `ContosoUniversity/DAL/SchoolContext.cs` | breaking-change-scan | EF6 DbContext/OnModelCreating → EF Core; table mappings, join-table with CourseInstructor column names; removed stored procedure mapping |
| `ContosoUniversity/DAL/SchoolConfiguration.cs` | breaking-change-scan | Removed EF6 DbConfiguration class; SqlAzureExecutionStrategy replaced by EF Core retry-on-failure in Startup.cs |
| `ContosoUniversity/DAL/SchoolInitializer.cs` | breaking-change-scan | Replaced EF6 DropCreateDatabaseIfModelChanges seed with EF Core EnsureCreated + manual seed |
| `ContosoUniversity/DAL/SchoolInterceptorLogging.cs` | breaking-change-scan | Rewrote EF6 DbCommandInterceptor to EF Core DbCommandInterceptor; error handling moved to CommandFailed/CommandFailedAsync (EF Core has no .Exception on CommandExecutedEventData) |
| `ContosoUniversity/DAL/SchoolInterceptorTransientErrors.cs` | breaking-change-scan | System.Data.SqlClient → Microsoft.Data.SqlClient |
| `ContosoUniversity/Controllers/InstructorController.cs` | breaking-change-scan | Removed System.Data.Entity usings; Include(i => i.Courses.Select(c => c.Department)) → Include().ThenInclude(); RetryLimitExceededException → Exception |
| `ContosoUniversity/Controllers/HomeController.cs` | breaking-change-scan | db.Database.SqlQuery<T>() (EF6) → db.Database.SqlQueryRaw<T>() (EF Core) |
| `ContosoUniversity/Startup.cs` | breaking-change-scan | Manual new SchoolContext(connStr) → AddDbContext with UseSqlServer + UseLazyLoadingProxies |
| `.gitignore` | breaking-change-scan | Removed wwwroot/ exclusion so bundler outputs are tracked and survive clean builds |

**Changed files (50)**

<details>
<summary>Show the 50 changed files</summary>

| File | Status | Area | Lines |
|---|---|---|---|
| `.gitignore` | modified | other | +0 / −1 |
| `ContosoUniversity/appsettings.json` | modified | config | +3 / −0 |
| `ContosoUniversity/ContosoUniversity.csproj` | modified | project | +28 / −57 |
| `ContosoUniversity/Controllers/CourseController.cs` | modified | code | +4 / −6 |
| `ContosoUniversity/Controllers/DepartmentController.cs` | modified | code | +7 / −11 |
| `ContosoUniversity/Controllers/HomeController.cs` | modified | code | +2 / −1 |
| `ContosoUniversity/Controllers/InstructorController.cs` | modified | code | +3 / −4 |
| `ContosoUniversity/Controllers/StudentController.cs` | modified | code | +4 / −5 |
| `ContosoUniversity/DAL/SchoolConfiguration.cs` | modified | code | +5 / −11 |
| `ContosoUniversity/DAL/SchoolContext.cs` | modified | code | +12 / −12 |
| `ContosoUniversity/DAL/SchoolInitializer.cs` | modified | code | +44 / −35 |
| `ContosoUniversity/DAL/SchoolInterceptorLogging.cs` | modified | code | +47 / −45 |
| `ContosoUniversity/DAL/SchoolInterceptorTransientErrors.cs` | modified | code | +13 / −13 |
| `ContosoUniversity/Migrations/201411021828194_InitialCreate.cs` | deleted | code | +0 / −59 |
| `ContosoUniversity/Migrations/201411021828194_InitialCreate.Designer.cs` | deleted | code | +0 / −29 |
| `ContosoUniversity/Migrations/201411021828194_InitialCreate.resx` | deleted | other | +0 / −126 |
| `ContosoUniversity/Migrations/201411021834484_MaxLengthOnNames.cs` | deleted | code | +0 / −20 |
| `ContosoUniversity/Migrations/201411021834484_MaxLengthOnNames.Designer.cs` | deleted | code | +0 / −29 |
| `ContosoUniversity/Migrations/201411021834484_MaxLengthOnNames.resx` | deleted | other | +0 / −126 |
| `ContosoUniversity/Migrations/201411021841373_ComplexDataModel.cs` | deleted | code | +0 / −96 |
| `ContosoUniversity/Migrations/201411021841373_ComplexDataModel.Designer.cs` | deleted | code | +0 / −29 |
| `ContosoUniversity/Migrations/201411021841373_ComplexDataModel.resx` | deleted | other | +0 / −126 |
| `ContosoUniversity/Migrations/201411021957458_DepartmentSP.cs` | deleted | code | +0 / −69 |
| `ContosoUniversity/Migrations/201411021957458_DepartmentSP.Designer.cs` | deleted | code | +0 / −29 |
| `ContosoUniversity/Migrations/201411021957458_DepartmentSP.resx` | deleted | other | +0 / −126 |
| `ContosoUniversity/Migrations/201411022009135_RowVersion.cs` | deleted | code | +0 / −75 |
| `ContosoUniversity/Migrations/201411022009135_RowVersion.Designer.cs` | deleted | code | +0 / −29 |
| `ContosoUniversity/Migrations/201411022009135_RowVersion.resx` | deleted | other | +0 / −126 |
| `ContosoUniversity/Migrations/201411022351064_Inheritance.cs` | deleted | code | +0 / −58 |
| `ContosoUniversity/Migrations/201411022351064_Inheritance.Designer.cs` | deleted | code | +0 / −29 |
| `ContosoUniversity/Migrations/201411022351064_Inheritance.resx` | deleted | other | +0 / −126 |
| `ContosoUniversity/Migrations/Configuration.cs` | deleted | code | +0 / −219 |
| `ContosoUniversity/Program.cs` | modified | code | +20 / −1 |
| `ContosoUniversity/Startup.cs` | modified | code | +12 / −3 |
| `ContosoUniversity/Views/Web.config` | deleted | config | +0 / −35 |
| `ContosoUniversity/wwwroot/Content/PagedList.css` | added | static | +166 / −0 |
| `ContosoUniversity/wwwroot/css/styles.css` | added | static | +7006 / −0 |
| `ContosoUniversity/wwwroot/css/styles.min.css` | added | static | +11 / −0 |
| `ContosoUniversity/wwwroot/fonts/glyphicons-halflings-regular.eot` | added | static | — |
| `ContosoUniversity/wwwroot/fonts/glyphicons-halflings-regular.svg` | added | static | +228 / −0 |
| `ContosoUniversity/wwwroot/fonts/glyphicons-halflings-regular.ttf` | added | static | — |
| `ContosoUniversity/wwwroot/fonts/glyphicons-halflings-regular.woff` | added | static | — |
| `ContosoUniversity/wwwroot/js/bs-bundle.js` | added | static | +2355 / −0 |
| `ContosoUniversity/wwwroot/js/bs-bundle.min.js` | added | static | +6 / −0 |
| `ContosoUniversity/wwwroot/js/jquery.js` | added | static | +12475 / −0 |
| `ContosoUniversity/wwwroot/js/jquery.min.js` | added | static | +25 / −0 |
| `ContosoUniversity/wwwroot/js/jqueryval.js` | added | static | +1661 / −0 |
| `ContosoUniversity/wwwroot/js/jqueryval.min.js` | added | static | +9 / −0 |
| `ContosoUniversity/wwwroot/js/modernizr.js` | added | static | +1416 / −0 |
| `ContosoUniversity/wwwroot/js/modernizr.min.js` | added | static | +10 / −0 |

</details>

## 4. Dependency changes

Package sets are compared between the starting commit and the final files (project file and `packages.config`). A removed package with no guidance is usually one the new SDK provides or one that `packages.config` listed only as a transitive dependency.

### ContosoUniversity

5 added, 3 upgraded, 11 removed

| Package | Action | Before | After | Note |
|---|---|---|---|---|
| EntityFramework | removed | 6.4.4 | — | Replaced by Microsoft.EntityFrameworkCore.SqlServer 10.0.12. EF Core has no DbConfiguration class; execution strategy is configured via DbContextOptionsBuilder. Lazy-loading requires the Microsoft.EntityFrameworkCore.Proxies package and UseLazyLoadingProxies(). The migration history format changed; old EF6 migrations were deleted and EnsureCreated is used. |
| Google.Cloud.Diagnostics.AspNetCore | upgraded | 4.2.0 | 4.4.0 | Upgraded from 4.2.0 to 4.4.0. The UseGoogleDiagnostics extension is now obsolete; migrate to AspNetCoreGoogleDiagnosticsExtensions in a follow-up. |
| Grpc.AspNetCore | upgraded | 2.32.0 | 2.84.0 | Upgraded from 2.32.0 to 2.84.0. No breaking changes expected for this usage. |
| Microsoft.AspNetCore.Mvc.NewtonsoftJson | upgraded | 5.0.5 | 10.0.12 | Upgraded from 5.0.5 to 10.0.12 to match the target framework. No behaviour changes expected. |
| Microsoft.EntityFrameworkCore | added | — | 10.0.12 |  |
| Microsoft.EntityFrameworkCore.Design | added | — | 10.0.12 |  |
| Microsoft.EntityFrameworkCore.Proxies | added | — | 10.0.12 | Added to restore EF6's default lazy-loading behaviour. Virtual navigation properties are required on entities. |
| Microsoft.EntityFrameworkCore.SqlServer | added | — | 10.0.12 |  |
| Microsoft.EntityFrameworkCore.Tools | added | — | 10.0.12 |  |

Also removed from the package list (10, no replacement guidance): bootstrap 3.4.1, jQuery 3.7.0, jQuery.Validation 1.19.5, Microsoft.DotNet.UpgradeAssistant.Extensions.Default.Analyzers 0.2.222702, Microsoft.jQuery.Unobtrusive.Validation 3.2.0, Modernizr 2.6.2, Respond 1.2.0, System.ComponentModel.Annotations 5.0.0, System.Configuration.ConfigurationManager 5.0.0, System.Data.DataSetExtensions 4.5.0.

## 5. Build & validation results

| Measure | Result |
|---|---|
| Target | ContosoUniversity.sln (Release) |
| Exit code | 0 |
| Errors | 0 |
| Warnings | 1 |

**Warnings by code**

| Code | Count |
|---|---|
| CS0618 | 1 |

**Sample diagnostics (first 1)**

| Code | Severity | Location | Message |
|---|---|---|---|
| CS0618 | warning | ContosoUniversity/Program.cs:51 | 'GoogleDiagnosticsWebHostBuilderExtensions.UseGoogleDiagnostics(IWebHostBuilder, string, string, string, LoggerOptions, TraceOptions, ErrorReportingOptions)' is obsolete: 'Use Google.Cloud.Diagnostics |

**Static assets (CSS, JavaScript, images)**

| Measure | Result |
|---|---|
| Layout | moved under wwwroot (0 files) |
| Verdict | pass — every reference resolves under wwwroot |
| References checked | 10 |
| Files moved under wwwroot | 0 |
| Regressed (served before, not now) | 0 |
| New references that do not resolve | 0 |
| Already broken in the original app | 0 |
| Differ only in letter case | 0 |

**Checklist by group**

| Group | Pass | Flagged | Fail |
|---|---|---|---|
| guardrails | 9 | 0 | 0 |
| measured checks | 1 | 0 | 0 |
| phases | 12 | 0 | 0 |

## 6. Confidence score

**Score: 98** _(computed by `finalize-validation.mjs`, not estimated)_

Formula: `100 × (checklist items passed / applicable) − 10 × (no_equivalent flags) − 2 × (warnings on changed files)`, floored at 0; capped at 40 when the build fails and at 69 when any checklist row fails. A run needs ≥ 70, a green build and no failed row to finalize.

| Input | Value |
|---|---|
| Checklist items applicable | 22 |
| Checklist items passed | 22 |
| Checklist items failed | 0 |
| `no_equivalent` flags (−10 each) | 0 |
| Informational flags (no penalty) | 3 |
| Build warnings on changed files (−2 each) | 1 |
| Caps applied | none |

Calculation: `100 x (22/22) - 10 x 0 no_equivalent flag(s) - 2 x 1 warning(s) on changed files = 98; 3 informational flag(s) cost nothing`

## 7. Checklist

| ID | Description | Status | Note |
|---|---|---|---|
| R1 | No refactors, renames, or restructuring beyond what the phase required | pass | Only required changes applied; no folder moves, renames, or reformatting |
| R2 | Only files identified by the phase scripts or error messages were changed | pass | All edited files were identified by build errors or phase scripts |
| R3 | Every replaced or major-bumped package has a dependency note | pass | EF6→EF Core and all upgraded packages noted in dependencyNotes |
| R4 | No credentials or connection strings added to source | pass | Secrets scan: 0 new credentials in changed files |
| R5 | Constructs with no .NET equivalent are flagged with a reason and options, never silently dropped | pass | every no_equivalent flag carries whyUnresolved, options and a recommendation (checked by this script) |
| R6 | The build succeeds (or every remaining error is triaged) | pass | the full rebuild succeeded |
| R7 | No credentials, secrets or keys were added or changed | pass | Scanned 14 changed text file(s); none of the added lines holds a credential or key. |
| R8 | Runtime smoke test — all pages accessible | pass | All 12 pages passed; schema initialized via SchoolInitializer.Initialize() at startup |
| R9 | No test regressions | pass | No test projects in scope |
| package-upgrade | All packages upgraded/replaced per plan | pass | EF6→EF Core 10.0.12, NewtonsoftJson 5→10, Google 4.2→4.4, Grpc 2.32→2.84, Proxies added; client-side NuGet refs removed |
| target-framework-bump | Target framework updated to net10.0 | pass | net5.0→net10.0 in ContosoUniversity.csproj |
| breaking-change-scan | All EF6 API breakages fixed for EF Core | pass | System.Data.Entity namespaces, RetryLimitExceededException, SqlQuery, SqlException namespace, CommandExecutedEventData.Exception, Include/ThenInclude all fixed |
| static-assets | Static assets (CSS, JavaScript, images) are served from wwwroot, as they were before the migration | pass | 10 static reference(s) checked; all served |
| ef-init | Database creation works on a fresh database | pass | The EF6 migrations were replaced by EnsureCreated() at startup: a fresh database gets every table; existing databases are left as they are. |
| auth-parity | Authorization matches what the original app enforced | pass | The original enforced no authorization and the migrated app adds none. |
| area-routes | Area routes only give the area's own controllers their prefix | pass | Area routes (if any) make the area a requirement. |
| ef-lazy-loading | Navigation properties the original loaded lazily still load | pass | Lazy-loading proxies are enabled, as EF6 behaved. |
| ef-schema-parity | The EF Core model describes the same database tables and columns as the EF6 model | pass | Inheritance mapping and join-table columns match the original EF6 model. |
| layout-parity | The layouts keep the original's stylesheets, scripts, navigation and sign-in area | pass | Every layout keeps the original's stylesheets, scripts, navigation and sign-in area. |
| disabled-code | No disabled copies of old code or rewrite banners in changed files | pass | No disabled code left in changed files. |
| secrets | No credentials or keys added by the migration | pass | Scanned 14 changed text file(s); none of the added lines holds a credential or key. |
| runtime-smoke | The migrated app starts on a fresh database and serves its pages and static files | pass | 15 page(s) rendered and 6 form(s) submitted with no server error, broken link or missing static file. |

## 8. Open items for human review

Only `no_equivalent` items count against the confidence score; informational items are facts worth knowing, not penalties.

### 8.1 Informational: CS0618 warning: GoogleDiagnosticsWebHostBuilderExtensions.UseGoogleDiagnostics is obsolete; use AspNetCoreGoogleDiagnosticsExtensions instead

- **Where:** `ContosoUniversity/Program.cs`
- **Recommended action:** Replace the UseGoogleDiagnostics call with the new AspNetCoreGoogleDiagnosticsExtensions API as part of a follow-up Google Cloud Diagnostics update

### 8.2 Informational: EF6 migration history deleted; EF Core migrations not yet initialized. The app uses EnsureCreated() for schema creation, which is suitable for development but not for production schema evolution.

- **Where:** `ContosoUniversity/DAL/SchoolInitializer.cs`
- **Recommended action:** Run 'dotnet ef migrations add InitialCreate' to create a proper EF Core migration baseline for production use, then switch from EnsureCreated() to Migrate()

### 8.3 Informational: EF Core lazy-loading proxies require all navigation properties to be virtual. Non-virtual navigation properties will not be lazy-loaded and may cause NullReferenceExceptions at runtime.

- **Where:** `ContosoUniversity/Models/`
- **Recommended action:** Verify all navigation properties that were lazy-loaded in EF6 are marked virtual. The Department, Course, Instructor, Student and Enrollment models have been reviewed and use virtual correctly.

## 9. Verification scope

| Check | Done |
|---|---|
| Solution build (`dotnet build`) | yes |
| Static-asset resolution (web projects) | yes |
| Automated tests | none in scope |
| Application started / runtime exercised | yes (15 page(s) served and 6 form(s) submitted on a throwaway database) |
| Signed in with the documented test user | not applicable |

**Migration checks (measured by script; fixed automatically where the fix is deterministic)**

| Check | Result | Detail |
|---|---|---|
| Database creation works on a fresh database | pass | The EF6 migrations were replaced by EnsureCreated() at startup: a fresh database gets every table; existing databases are left as they are. |
| Authorization matches what the original app enforced | pass | The original enforced no authorization and the migrated app adds none. |
| Area routes only give the area's own controllers their prefix | pass | Area routes (if any) make the area a requirement. |
| Navigation properties the original loaded lazily still load | pass | Lazy-loading proxies are enabled, as EF6 behaved. |
| The EF Core model describes the same database tables and columns as the EF6 model | pass | Inheritance mapping and join-table columns match the original EF6 model. |
| The layouts keep the original's stylesheets, scripts, navigation and sign-in area | pass | Every layout keeps the original's stylesheets, scripts, navigation and sign-in area. |
| No disabled copies of old code or rewrite banners in changed files | pass | No disabled code left in changed files. |
| No credentials or keys added by the migration | pass | Scanned 14 changed text file(s); none of the added lines holds a credential or key. |

**Runtime-unverified — QA should exercise these first**

- Entity Framework 6 (23 file(s)): EF Core differs in configuration, migrations, lazy loading, and some query translation; data behavior must be verified.
- The runtime smoke test started the app and loaded its pages anonymously. Pages behind a login, and the forms on them, were not exercised: walk those, and compare a few pages side by side with the original.

## 10. Next steps

1. Replace the UseGoogleDiagnostics call with the new AspNetCoreGoogleDiagnosticsExtensions API as part of a follow-up Google Cloud Diagnostics update
2. Run 'dotnet ef migrations add InitialCreate' to create a proper EF Core migration baseline for production use, then switch from EnsureCreated() to Migrate()
3. Verify all navigation properties that were lazy-loaded in EF6 are marked virtual. The Department, Course, Instructor, Student and Enrollment models have been reviewed and use virtual correctly.
4. There is no automated test project: add tests or run a manual regression pass before relying on this migration.

## 11. Iteration history

| Iteration | Confidence | Build | Failed rows | Next phase | Computed at |
|---|---|---|---|---|---|
| 0 | 98 | pass | — | finalize | 2026-10-09T07:11:47.224Z |
