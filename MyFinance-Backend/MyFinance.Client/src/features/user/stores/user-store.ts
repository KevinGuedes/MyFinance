import { create } from 'zustand'

import { User } from '@/features/user/models/user'
import { getInitials } from '@/lib/utils'

interface AuthState {
  user: User | null
  initials: string | null
  authenticationStatus: 'signed-in' | 'signed-out' | 'indeterminated'
  setUserInfo: (user: User) => void
  clearUserInfo: () => void
}

export const useUserStore = create<AuthState>()((set) => ({
  user: null,
  initials: null,
  authenticationStatus: 'indeterminated',
  setUserInfo: (user) =>
    set({
      user,
      initials: getInitials(user.name),
      authenticationStatus: 'signed-in',
    }),
  clearUserInfo: () =>
    set({
      user: null,
      initials: null,
      authenticationStatus: 'signed-out',
    }),
}))
