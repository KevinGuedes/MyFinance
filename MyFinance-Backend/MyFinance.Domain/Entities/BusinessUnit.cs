﻿using MyFinance.Domain.Enums;

namespace MyFinance.Domain.Entities;

public class BusinessUnit : Entity
{
    public string Name { get; private set; }
    public double CurrentBalance { get; private set; }
    public string? Description { get; private set; }
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchiveDate { get; private set; }
    public List<MonthlyBalance> MonthlyBalances { get; private set; }

    public BusinessUnit() { }

    public BusinessUnit(string name, string? description)
    {
        Name = name;
        CurrentBalance = 0;
        Description = description;
        IsArchived = false;
        ReasonToArchive = null;
        ArchiveDate = null;
        MonthlyBalances = new List<MonthlyBalance>();
    }

    public void Update(string name, string? description)
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

    public void UpdateBalanceWithNewTransfer(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit) CurrentBalance += transferValue;
        else CurrentBalance -= transferValue;
    }

    public void UpdateBalanceWithTransferDeletion(double transferValue, TransferType transferType)
    {
        SetUpdateDateToNow();
        if (transferType == TransferType.Profit) CurrentBalance -= transferValue;
        else CurrentBalance += transferValue;
    }
}
