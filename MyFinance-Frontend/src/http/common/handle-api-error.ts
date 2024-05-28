import { HttpStatusCode } from 'axios'

import { ApiError } from './api-error'

export function handleApiError(apiError: ApiError) {
  if (apiError.response === undefined) {
    return {
      statusCode: HttpStatusCode.ServiceUnavailable,
      title: 'Services currently unavailable',
      description: 'Please try again later.',
    }
  }

  const isBadRequest = apiError.response?.status === HttpStatusCode.BadRequest
  const errorData = apiError.response?.data

  return {
    statusCode: apiError.response?.status,
    validationErrors: errorData?.errors,
    isBadRequest,
    title: errorData?.title,
    description: errorData?.detail,
  }
}
