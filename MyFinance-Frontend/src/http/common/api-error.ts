import { AxiosError } from 'axios'

export type ApiErrorResponse = {
  title: string
  type: string
  status: number
  detail: string
  instance: string
  errors?: {
    [key: string]: string[]
  }
}

export type ApiError<T = unknown> = AxiosError<ApiErrorResponse & T>
