# Game Shop System - Clean Architecture Notes

## Final Project Layout
- `P.GameShopSystem.Domain` → domain constants/entities only.
- `P.GameShopSystem.DB` → EF Core database-first models and `DbContext` only.
- `P.GameShopSystem.API` → application/business logic and API endpoints.
- `P.GameShopSystem.Web` → MVC UI only, calling API via `HttpClient`.

## Merge Conflict Notes
If your PR target branch still has old `GameShop.Web/*` paths, conflicts can appear because the project was renamed to `P.GameShopSystem.Web/*`.

Recommended resolution strategy:
1. Keep renamed files under `P.GameShopSystem.Web/*` from this branch.
2. For each conflicting old path (for example `GameShop.Web/Program.cs`), choose **theirs** only if target branch contains newer logic not present here.
3. Move any needed logic into the matching file under `P.GameShopSystem.Web/*` and remove old `GameShop.Web/*` files.
4. Ensure `GameShopSystem.sln` references only:
   - `P.GameShopSystem.Domain`
   - `P.GameShopSystem.DB`
   - `P.GameShopSystem.API`
   - `P.GameShopSystem.Web`
