﻿using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface ISummaryGenerator
{
    (string FileName, byte[] FileContent) GenerateMonthlySummary(
        ManagementUnit managementUnit,
        IEnumerable<Transfer> transfers,
        int year,
        int month);
}