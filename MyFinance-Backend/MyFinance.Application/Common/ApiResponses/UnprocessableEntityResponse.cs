﻿using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.Common.ApiResponses;

public sealed class UnprocessableEntityResponse(UnprocessableEntityError unprocessableEntityError)
    : BaseApiResponse<UnprocessableEntityError>("Unable to process entity data", unprocessableEntityError)
{
}
