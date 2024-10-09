using MyFinance.Domain.Enums;
using MyFinance.TestCommon.Builders;
using MyFinance.TestCommon.Common;

namespace MyFinance.UnitTests.Domain;

public sealed class ManagementUnitTests
{
    [Fact]
    public void ManagementUnit_ShouldHaveDefaultValues_WhenCreated()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var description = faker.Lorem.Sentence();
        var managementUnit = new ManagementUnit(managementUnitName, description, userId);

        Assert.Equal(managementUnitName, managementUnit.Name);
        Assert.Equal(description, managementUnit.Description);
        Assert.Equal(0, managementUnit.Income);
        Assert.Equal(0, managementUnit.Outcome);
        Assert.Equal(0, managementUnit.Balance);
        Assert.False(managementUnit.IsArchived);
        Assert.Null(managementUnit.ReasonToArchive);
        Assert.Null(managementUnit.ArchivedOnUtc);
        Assert.Equal(userId, managementUnit.UserId);
        Assert.Empty(managementUnit.Transfers);
        Assert.Empty(managementUnit.AccountTags);
        Assert.Empty(managementUnit.Categories);
    }

    [Fact]
    public void ManagementUnit_ShouldBeUpdated_WhenUpdateIsCalled()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var description = faker.Lorem.Sentence();
        var managementUnit = new ManagementUnit(managementUnitName, description, userId);

        var updatedName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var updatedDescription = faker.Lorem.Sentence();
        managementUnit.Update(updatedName, updatedDescription);

        Assert.Equal(updatedName, managementUnit.Name);
        Assert.Equal(updatedDescription, managementUnit.Description);
    }

    [Fact]
    public void ManagementUnit_ShouldBeArchived_WhenArchiveIsCalled()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var description = faker.Lorem.Sentence();
        var managementUnit = new ManagementUnit(managementUnitName, description, userId);

        var reasonToArchive = faker.Lorem.Sentence();
        managementUnit.Archive(reasonToArchive);

        Assert.True(managementUnit.IsArchived);
        Assert.Equal(reasonToArchive, managementUnit.ReasonToArchive);
        Assert.NotNull(managementUnit.ArchivedOnUtc);
    }

    [Fact]
    public void ManagementUnit_ShouldBeUnarchived_WhenUnarchiveIsCalled()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var description = faker.Lorem.Sentence();
        var managementUnit = new ManagementUnit(managementUnitName, description, userId);
        var reasonToArchive = faker.Lorem.Sentence();
        managementUnit.Archive(reasonToArchive);

        managementUnit.Unarchive();

        Assert.False(managementUnit.IsArchived);
        Assert.Null(managementUnit.ReasonToArchive);
        Assert.Null(managementUnit.ArchivedOnUtc);
    }

    [Fact]
    public void ManagementUnit_ShouldHaveBalance_WhenIncomeAndOutcomeAreSet()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var income = 1000m;
        var outcome = 500m;
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var managementUnit = ManagementUnitBuilder.With()
            .Name(managementUnitName)
            .Income(income)
            .Outcome(outcome)
            .UserId(userId).Build();

        Assert.Equal(income, managementUnit.Income);
        Assert.Equal(outcome, managementUnit.Outcome);
        Assert.Equal(income - outcome, managementUnit.Balance);
    }

    [Fact]
    public void ManagementUnit_ShouldHaveUpdatedIncome_WhenProfitIsAdded()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var income = 1000m;
        var outcome = 500m;
        var transferValue = 200m;
        var transferType = TransferType.Profit;
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var managementUnit = ManagementUnitBuilder.With()
            .Name(managementUnitName)
            .Income(income)
            .Outcome(outcome)
            .UserId(userId).Build();

        managementUnit.RegisterTransferValue(transferValue, transferType);

        Assert.Equal(income + transferValue, managementUnit.Income);
    }

    [Fact]
    public void ManagementUnit_ShouldHaveUpdatedOutcome_WhenOutcomeIsAdded()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var income = 1000m;
        var outcome = 500m;
        var transferValue = 200m;
        var transferType = TransferType.Expense;
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var managementUnit = ManagementUnitBuilder.With()
            .Name(managementUnitName)
            .Income(income)
            .Outcome(outcome)
            .UserId(userId).Build();

        managementUnit.RegisterTransferValue(transferValue, transferType);

        Assert.Equal(outcome + transferValue, managementUnit.Outcome);
    }

    [Fact]
    public void ManagementUnit_ShouldHaveUpdatedIncome_WhenProfitIsCanceled()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var income = 1000m;
        var outcome = 500m;
        var transferValue = 200m;
        var transferType = TransferType.Profit;
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var managementUnit = ManagementUnitBuilder.With()
            .Name(managementUnitName)
            .Income(income)
            .Outcome(outcome)
            .UserId(userId).Build();

        managementUnit.CancelTransferValue(transferValue, transferType);

        Assert.Equal(income - transferValue, managementUnit.Income);
    }

    [Fact]
    public void ManagementUnit_ShouldHaveUpdatedOutcome_WhenOutcomeIsCanceled()
    {
        var faker = DataGenerator.CreateFaker();
        var userId = faker.Random.Guid();
        var income = 1000m;
        var outcome = 500m;
        var transferValue = 200m;
        var transferType = TransferType.Expense;
        var managementUnitName = faker.Company.CompanyName() + " " + faker.Commerce.Department();
        var managementUnit = ManagementUnitBuilder.With()
            .Name(managementUnitName)
            .Income(income)
            .Outcome(outcome)
            .UserId(userId).Build();

        managementUnit.CancelTransferValue(transferValue, transferType);

        Assert.Equal(outcome - transferValue, managementUnit.Outcome);
    }
}
