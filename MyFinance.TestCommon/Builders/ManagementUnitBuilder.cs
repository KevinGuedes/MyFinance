using MyFinance.TestCommon.Common;

namespace MyFinance.TestCommon.Builders;

public class ManagementUnitBuilder : EntityBuilder<ManagementUnit>
{
    private Guid? _id;
    private DateTime? _createdOnUtc;
    private DateTime? _updatedOnUtc;
    private string? _name;
    private string? _description;
    private decimal? _income;
    private decimal? _outcome;
    private bool? _isArchived;
    private string? _reasonToArchive;
    private DateTime? _archivedOnUtc;
    private Guid? _userId;
    private List<Transfer>? _transfers;
    private List<AccountTag>? _accountTags;
    private List<Category>? _categories;

    private ManagementUnitBuilder()
    {
    }

    public static ManagementUnitBuilder With() => new();

    public ManagementUnitBuilder Id(Guid id)
    {
        _id = id;
        return this;
    }

    public ManagementUnitBuilder CreatedOnUtc(DateTime createdOnUtc)
    {
        _createdOnUtc = createdOnUtc;
        return this;
    }

    public ManagementUnitBuilder UpdatedOnUtc(DateTime updatedOnUtc)
    {
        _updatedOnUtc = updatedOnUtc;
        return this;
    }

    public ManagementUnitBuilder Name(string name)
    {
        _name = name;
        return this;
    }

    public ManagementUnitBuilder Description(string description)
    {
        _description = description;
        return this;
    }

    public ManagementUnitBuilder Income(decimal income)
    {
        _income = income;
        return this;
    }

    public ManagementUnitBuilder Outcome(decimal outcome)
    {
        _outcome = outcome;
        return this;
    }

    public ManagementUnitBuilder IsArchived(bool isArchived)
    {
        _isArchived = isArchived;
        return this;
    }

    public ManagementUnitBuilder ReasonToArchive(string reasonToArchive)
    {
        _reasonToArchive = reasonToArchive;
        return this;
    }

    public ManagementUnitBuilder ArchivedOnUtc(DateTime archivedOnUtc)
    {
        _archivedOnUtc = archivedOnUtc;
        return this;
    }

    public ManagementUnitBuilder UserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public ManagementUnitBuilder Transfers(List<Transfer> transfers)
    {
        _transfers = transfers;
        return this;
    }

    public ManagementUnitBuilder AccountTags(List<AccountTag> accountTags)
    {
        _accountTags = accountTags;
        return this;
    }

    public ManagementUnitBuilder Categories(List<Category> categories)
    {
        _categories = categories;
        return this;
    }

    public override ManagementUnit Build()
    {
        var values = new
        {
            Id = _id,
            CreatedOnUtc = _createdOnUtc,
            UpdatedOnUtc = _updatedOnUtc,
            Name = _name,
            Description = _description,
            Income = _income,
            Outcome = _outcome,
            IsArchived = _isArchived,
            ReasonToArchive = _reasonToArchive,
            ArchivedOnUtc = _archivedOnUtc,
            UserId = _userId,
            Transfers = _transfers,
            AccountTags = _accountTags,
            Categories = _categories
        };

        return BuildEntity(values);
    }
}
