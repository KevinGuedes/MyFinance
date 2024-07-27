using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Contracts.Common;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;
using System.Globalization;

namespace MyFinance.Application.Mappers;

public static class TransferMapper
{
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