using MyFinance.IntegrationTests.Common;

namespace MyFinance.IntegrationTests.SharedContexts;

[CollectionDefinition(nameof(ApplicationFactoryCollection))]
public class ApplicationFactoryCollection : ICollectionFixture<ApplicationFactory>;
