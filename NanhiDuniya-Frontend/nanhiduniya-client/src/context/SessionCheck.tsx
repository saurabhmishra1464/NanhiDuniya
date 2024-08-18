
import LoadingSpinner from '@/components/LoadingSpinner';
import { useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import React, { useEffect, useState } from 'react';

export default function SessionCheck({ children }: { children: React.ReactNode }) {
  const { data: session, status } = useSession();
  const router = useRouter();

  useEffect(() => {
    if (status === 'unauthenticated' || !session) {
      router.push('/auth/login');
    } 
  }, [status, session, router]);

  if (status === 'loading') {
    return <LoadingSpinner/>
  }

  return <>{children}</>;
}
