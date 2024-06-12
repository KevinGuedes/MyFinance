import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useRouter } from '@tanstack/react-router'

import { useToast } from '@/components/ui/toast/use-toast'
import { useUserStore } from '@/stores/user-store'

import { userApi } from '../api'
import { ApiError } from '../common/api-error'
import { handleError } from '../common/handle-error'

export const useSignOut = () => {
  const clearUserInfo = useUserStore((state) => state.clearUserInfo)
  const queryClient = useQueryClient()
  const router = useRouter()
  const { toast } = useToast()

  const mutation = useMutation<unknown, ApiError>({
    mutationFn: async () => {
      await userApi.post('/signout')
    },
    onSuccess: () => {
      clearUserInfo()
      queryClient.clear()
      router.navigate({ to: '/' })
    },
    onError: (error) => {
      const { description, title } = handleError(error)

      toast({
        variant: 'destructive',
        title,
        description,
      })
    },
  })

  return mutation
}
