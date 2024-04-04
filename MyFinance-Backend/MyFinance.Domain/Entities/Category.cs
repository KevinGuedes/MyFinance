﻿using MyFinance.Domain.Abstractions;
using MyFinance.Domain.Common;

namespace MyFinance.Domain.Entities;

public sealed class Category : Entity, IUserOwnedEntity, IArchivableEntity
{
    private Category()
    {
    }

    public Category(string name, Guid userId)
    {
        UserId = userId;
        Name = name;
        IsArchived = false;
        ReasonToArchive = null;
        ArchivedOnUtc = null;
        Transfers = [];
    }

    public Guid UserId { get; init; }
    public string Name { get; private set; } = null!;
    public bool IsArchived { get; private set; }
    public string? ReasonToArchive { get; private set; }
    public DateTime? ArchivedOnUtc { get; private set; }
    public List<Transfer> Transfers { get; private set; } = [];

    public void Update(string name)
    {
        SetUpdateOnToUtcNow();

        Name = name;
    }

    public void Archive(string? reasonToArchive)
    {
        var utcNow = DateTime.UtcNow;
        SetUpdateOnToUtcNow(utcNow);

        ArchivedOnUtc = DateTime.UtcNow;
        IsArchived = true;
        ReasonToArchive = reasonToArchive;
    }

    public void Unarchive()
    {
        SetUpdateOnToUtcNow();

        IsArchived = false;
        ArchivedOnUtc = null;
        ReasonToArchive = null;
    }
}