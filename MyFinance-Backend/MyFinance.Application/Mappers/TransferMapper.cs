﻿using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Enums;

namespace MyFinance.Application.Mappers;

public static class TransferMapper
{
    public static class DTR
    {
        public static Paginated<TransferGroupResponse> Map(
            (long TotalCount, IEnumerable<Transfer> Transfers) transfersData,
            int pageNumber,
            int pageSize)
        {
            var tranferGroups = transfersData.Transfers.GroupBy(
                transfer => transfer.SettlementDate.Date,
                transfer => transfer,
                (settlementDate, transfers) =>
                {
                    var income = 0m;
                    var outcome = 0m;

                    foreach (var transfer in transfers)
                    {
                        if (transfer.Type == TransferType.Profit)
                            income += transfer.Value;
                        else
                            outcome += transfer.Value;
                    }

                    return new TransferGroupResponse
                    {
                        Date = DateOnly.FromDateTime(settlementDate),
                        Transfers = Map(transfers),
                        Income = income,
                        Outcome = outcome
                    };
                });

            return new(
                tranferGroups.ToList().AsReadOnly(),
                pageNumber,
                pageSize,
                transfersData.TotalCount);
        }

        public static TransferResponse Map(Transfer transfer)
            => new()
            {
                Id = transfer.Id,
                RelatedTo = transfer.RelatedTo,
                Description = transfer.Description,
                SettlementDate = transfer.SettlementDate,
                Type = transfer.Type,
                Value = transfer.Value
            };

        public static IReadOnlyCollection<TransferResponse> Map(IEnumerable<Transfer> transfers)
            => transfers.Select(Map).ToList().AsReadOnly();
    }

    public static class RTC
    {
        public static RegisterTransferCommand Map(RegisterTransferRequest request)
            => new(request.ManagementUnitId,
                request.AccountTagId,
                request.CategoryId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);

        public static UpdateTransferCommand Map(UpdateTransferRequest request)
            => new(request.Id,
                request.AccountTagId,
                request.CategoryId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);
    }
}