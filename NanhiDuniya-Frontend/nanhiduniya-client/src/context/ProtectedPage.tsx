'use client';
import LoadingSpinner from '@/components/LoadingSpinner';
import { useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import { useEffect } from 'react';

export default function ProtectedPage({ children }: { children: React.ReactNode }) {
  const { data: session, status } = useSession();
  const router = useRouter();

  useEffect(() => {
    // Ensure that redirects happen only after session status is resolved
    console.log("Session status:", status, "Session data:", session);
    if (status === 'unauthenticated') {
      router.push('/auth/login');
    }
  }, [status]);

  // Prevent rendering of the component until the session is resolved
  if (status === 'loading') {
    return <LoadingSpinner />;
  }

  // Once the session is authenticated, render the protected content
  if (status === 'authenticated') {
    return <>{children}</>;
  }

  // Handle unauthenticated state if needed (should redirect automatically)
  return null;
}
