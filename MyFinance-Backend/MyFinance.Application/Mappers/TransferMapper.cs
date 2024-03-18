using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Application.UseCases.Transfers.Commands.UpdateTransfer;
using MyFinance.Contracts.Transfer.Requests;
using MyFinance.Contracts.Transfer.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;
public static class TransferMapper
{
    public static class DTR 
    {
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
    }

    public static class RTC 
    {
        public static RegisterTransferCommand Map(RegisterTransferRequest request)
            => new(
              request.BusinessUnitId,
              request.AccountTagId,
              request.Value,
              request.RelatedTo,
              request.Description,
              request.SettlementDate,
              request.Type);

        public static UpdateTransferCommand Map(UpdateTransferRequest request)
            => new(
                request.Id,
                request.AccountTagId,
                request.Value,
                request.RelatedTo,
                request.Description,
                request.SettlementDate,
                request.Type);
    }
}
