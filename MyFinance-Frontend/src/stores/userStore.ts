import { atom } from 'jotai'
import { atomWithImmer } from 'jotai-immer'

export type SignUpRequest = {
  name: string
  email: string
  plainTextPassword: string
  lainTextPasswordConfirmation: string
}

export type SignUpResponse = {
  shouldUpdatePassword: boolean
}

export const userDataAtom = atomWithImmer<SignUpResponse | null>(null)

export const setUserData = atom(null, (_, set, userData: SignUpResponse) => {
  set(userDataAtom, (draft) => {
    if (draft != null)
      draft.shouldUpdatePassword = userData.shouldUpdatePassword
  })
})
