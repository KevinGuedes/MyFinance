import { ProblemResponse } from './problem-response'

export type ValidationProblemResponse = ProblemResponse & {
  errors: {
    [key: string]: string[]
  }
}
