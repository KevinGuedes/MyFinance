﻿using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class BusinessUnit : UserOwnedEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public double Income { get; private set; }
    public double Outcome { get; private set; }
    public double Balance { get => Income - Outcome; }
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchiveDate { get; private set; }
    public List<MonthlyBalance> MonthlyBalances { get; private set; } = [];

    private BusinessUnit() { }

    public BusinessUnit(string name, string description, Guid userId) : base(userId)
    {
        Name = name;
        Income = 0;
        Outcome = 0;
        Description = description;
        IsArchived = false;
        ReasonToArchive = null;
        ArchiveDate = null;
        MonthlyBalances = [];
    }

    public void Update(string name, string description)
    {
        SetUpdateDateToNow();
        Name = name;
        Description = description;
    }

    public void Archive(string reasonToArchive)
    {
        SetUpdateDateToNow();
        ArchiveDate = DateTime.UtcNow;
        IsArchived = true;
        ReasonToArchive = reasonToArchive;
    }

    public void Unarchive()
    {
        SetUpdateDateToNow();
        IsArchived = false;
        ArchiveDate = null;
        ReasonToArchive = null;
    }

    public void RegisterValue(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit)
            Income += transferValue;
        else
            Outcome += transferValue;
    }

    public void CancelValue(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit)
            Income -= transferValue;
        else
            Outcome -= transferValue;
    }
}
