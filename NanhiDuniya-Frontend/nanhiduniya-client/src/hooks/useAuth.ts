import { useState, useEffect } from 'react';
import { useSession } from 'next-auth/react';

export default function useAuth() {
  const { data: session, status } = useSession();
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);

  useEffect(() => {
    setLoading(true);
    if (session) {
      setUser(session.user); // Extract user information from session
    } else {
      setUser(null);
    }
    setLoading(false);
  }, [session]);

  return {
    user,
    loading,
    isAuthenticated: status === 'authenticated',
  };
}
