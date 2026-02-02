using CoworkingManagement.Application.Common.Interfaces;

namespace CoworkingManagement.IntegrationsTests.Fixtures;

[CollectionDefinition("Integration tests")]
public class IntegrationTestCollection
    : ICollectionFixture<ApiWebApplicationFactory>
{
}
