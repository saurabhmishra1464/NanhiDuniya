import VerifyEmailPage from '@/components/VerifyEmail/VerifyEmailPage'
import React, { Suspense } from 'react'

const Page = () => {
  return (
    <div>
        <Suspense fallback={<div>Loading...</div>}>
        <VerifyEmailPage />
      </Suspense>
    </div>
  )
}

export default Page;
  