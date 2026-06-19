# EPR.Payment.Service.IntegrationTests

[Outside-in integration tests](https://0x5.uk/2024/03/27/why-do-automated-tests-matter/#:~:text=Outside%2Din%20tests) for `EPR.Payment.Service.Api`. Each test sends real HTTP requests to an in-process instance of the API hosted by `WebApplicationFactory<Program>`, which talks to a Testcontainers-managed SQL Server.


## Running

```bash
dotnet test src/EPR.Payment.Service.IntegrationTests
```

Requires Docker. One MsSql container + one migrated database + one `WebApplicationFactory<Program>` are shared across every test class via `PaymentsServiceCollection` (xUnit collection fixture). Cost: ~20s container/factory boot once, then sub-second per test. Trade-off: classes in the collection run serially — fine because there's no per-class fixture cost left to parallelise away. Tests stay independent by using `Guid.NewGuid()` or per-class label prefixes (e.g. `LA-001…LA-006`); a `ContainSingle(x => x.Name == "...")` predicate is safe as long as the literal is unique across the whole suite.

## Notes

- **No WireMock or test auth.** The Payments API doesn't call any external HTTP services 
- **The data-generator copy.** `Infrastructure/Builders/` holds a copy of `DatabaseDataGenerator` and `RandomModelData` rather than a project reference to `.Data.IntegrationTests`. Deliberate — keeping the two test projects independent means a refactor in one can't accidentally break the other.
