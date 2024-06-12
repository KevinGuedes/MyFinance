import { createFileRoute, Link } from '@tanstack/react-router'

import { Button } from '@/components/ui/button'
import { useUserInfo } from '@/http/user/use-user-info'

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
})

function Home() {
  const userInfoMutation = useUserInfo()

  return (
    <div>
      <p>Home Page</p>
      <Button onClick={() => userInfoMutation.mutate({ originPath: '/' })}>
        Get User Info
      </Button>
      <Link to="/management-unit-dashboard">Management Unit Dashboard</Link>
    </div>
  )
}
