import { AxiosError } from 'axios'

import { ProblemResponse } from './problem-response'
import { ValidationProblemResponse } from './validation-problem-response'

export type ApiError<T = unknown> = AxiosError<
  ProblemResponse | ValidationProblemResponse | T
>
