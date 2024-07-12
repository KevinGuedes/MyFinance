import axios, { AxiosError, HttpStatusCode } from 'axios'

import { useUserStore } from '@/features/user/stores/user-store'

function createApi(resourcePath: string) {
  const api = axios.create({
    baseURL: 'https://myfinance-api.shop/' + resourcePath,
    withCredentials: true,
  })

  api.interceptors.response.use(
    (response) => response,
    (error: AxiosError) => {
      if (error.response?.status === HttpStatusCode.Unauthorized) {
        useUserStore.getState().clearUserInfo()
      }

      return Promise.reject(error)
    },
  )

  return api
}

export const userApi = createApi('user')
export const managementUnitApi = createApi('managementunit')
export const transferApi = createApi('transfer')
export const categoryApi = createApi('category')
export const accountTagApi = createApi('accounttag')
