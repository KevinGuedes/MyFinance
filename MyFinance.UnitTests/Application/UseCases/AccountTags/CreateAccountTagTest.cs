using MockQueryable.NSubstitute;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Contracts.AccountTag.Requests;

namespace MyFinance.UnitTests.Application.UseCases.AccountTags;

public sealed class CreateAccountTagTest
{
    private readonly CreateAccountTagHandler _sut;
    private readonly IMyFinanceDbContext _myFinanceDbContext;

    public CreateAccountTagTest()
    {
        _myFinanceDbContext = Substitute.For<IMyFinanceDbContext>();
        _sut = new CreateAccountTagHandler(_myFinanceDbContext);
    }

    [Fact]
    public async Task CreateAccountTag_Should_ReturnFailure_WhenManagementUnitIsNotFound()
    {
        var request = new CreateAccountTagRequest(Guid.NewGuid(), "TAG", "");
        var command = new CreateAccountTagCommand(request);
        var managementUnits = Enumerable.Empty<ManagementUnit>();
        var mockManagementUnitsDbSet = managementUnits.BuildMockDbSet();
        _myFinanceDbContext.ManagementUnits.Returns(mockManagementUnitsDbSet);

        var result = await _sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailed);
    }

    //[Fact]
    //public async Task CreateAccountTag_Should_ReturnSuccess_WhenManagementUnitIsFound()
    //{
    //    var managementUnitId = Guid.NewGuid();
    //    var request = new CreateAccountTagRequest(managementUnitId, "TAG", "");
    //    var command = new CreateAccountTagCommand(request);
    //    var managementUnits = new List<ManagementUnit> { 
    //        new(managementUnitId, "MU Teste", "Description", Guid.NewGuid()) 
    //    };

    //    var mockManagementUnitsDbSet = managementUnits.BuildMockDbSet();
    //    _myFinanceDbContext.ManagementUnits.Returns(mockManagementUnitsDbSet);

    //    var result = await _sut.Handle(command, CancellationToken.None);

    //    Assert.True(result.IsFailed);
    //}
}
