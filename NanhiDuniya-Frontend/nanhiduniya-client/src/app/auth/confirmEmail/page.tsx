import React, { Suspense } from 'react'
import ConfirmEmail from '@/components/VerifyEmail/ConfirmEmail'
const Page = () => {
  return (
    <div>
    <Suspense fallback={<div>Loading...</div>}>
    <ConfirmEmail />
  </Suspense>
</div>
  )
}

export default Page