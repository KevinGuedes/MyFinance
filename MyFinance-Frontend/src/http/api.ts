import axios from 'axios'

function createApi(resourcePath: string) {
  return axios.create({
    baseURL: 'https://localhost:7286/' + resourcePath,
    withCredentials: true,
  })
}

export const userApi = createApi('user')
export const managementUnitApi = createApi('managementunit')
export const transferApi = createApi('transfer')
export const categoryApi = createApi('category')
export const accountTagApi = createApi('accounttag')
