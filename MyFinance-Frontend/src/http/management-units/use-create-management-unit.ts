import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { api } from '../api'

type CreateManagementUnitResponse = {
  id: string
  name: string
  income: number
  outcome: number
  balance: number
  description?: string
}

type CreateManagementUnitRequest = Pick<
  CreateManagementUnitResponse,
  'name' | 'description'
>

export function useCreateManagementUnit() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<
    CreateManagementUnitResponse,
    Error,
    CreateManagementUnitRequest
  >({
    mutationFn: async (createManagementUnitRequest) => {
      const response = await api.post<CreateManagementUnitResponse>(
        '/managementunit',
        createManagementUnitRequest,
      )

      return response.data
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['management-units'] })
      toast({
        title: 'Management Unit successfully created!',
      })
    },
    onError: () => {
      toast({
        variant: 'destructive',
        title: 'Failed to create Management Unit',
        description: 'Try again later',
      })
    },
  })

  return mutation
}
