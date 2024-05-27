import { HttpStatusCode } from 'axios'

import { ApiError } from './api-error'
import { ProblemResponse } from './problem-response'
import { ValidationProblemResponse } from './validation-problem-response'

export function handleError(error: ApiError) {
  const isBadRequest = error.response?.status === HttpStatusCode.BadRequest

  if (isBadRequest) {
    const errorData = error.response?.data as ValidationProblemResponse

    return {
      statusCode: error.response?.status,
      validationErrors: errorData.errors,
      isBadRequest: true,
      errorData,
    }
  }

  const errorData = error.response?.data as ProblemResponse

  return {
    statusCode: error.response?.status,
    isBadRequest: false,
    errorData,
  }
}
