import { create } from 'zustand'

import { User } from '@/models/entities/user'

interface AuthState {
  user: User | null
  authenticationStatus: 'signed-in' | 'signed-out' | 'indeterminated'
  setUserInfo: (user: User) => void
  clearUserInfo: () => void
}

export const useUserStore = create<AuthState>()((set) => ({
  user: null,
  authenticationStatus: 'indeterminated',
  setUserInfo: (user) => set({ user, authenticationStatus: 'signed-in' }),
  clearUserInfo: () =>
    set({
      user: null,
      authenticationStatus: 'signed-out',
    }),
}))
