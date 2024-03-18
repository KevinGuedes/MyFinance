using MyFinance.Application.UseCases.AccountTags.Commands.ArchiveAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;
using MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;
using MyFinance.Contracts.AccountTag;
using MyFinance.Contracts.AccountTag.Requests;
using MyFinance.Contracts.AccountTag.Responses;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Mappers;

public static class AccountTagMapper
{
    public static class DTR
    {
        public static AccountTagResponse Map(AccountTag accountTag)
            => new()
            {
                Id = accountTag.Id,
                Tag = accountTag.Tag,
                Description = accountTag.Description,
                IsArchived = accountTag.IsArchived,
                ReasonToArchive = accountTag.ReasonToArchive,
                ArchiveDate = accountTag.ArchiveDate
            };

        public static IReadOnlyCollection<AccountTagResponse> Map(IEnumerable<AccountTag> accountTags)
            => accountTags.Select(Map).ToList().AsReadOnly();
    }

    public static class RTC
    {
        public static CreateAccountTagCommand Map(CreateAccountTagRequest request)
            => new(request.Tag, request.Description);

        public static UpdateAccountTagCommand Map(UpdateAccountTagRequest request)
             => new(request.Id, request.Tag, request.Description);

        public static ArchiveAccountTagCommand Map(ArchiveAccountTagRequest request)
            => new(request.Id, request.ReasonToArchive);
    }
}
