
'use client';
import { useRouter } from 'next/router';

const ErrorPage = () => {
  const router = useRouter();
  const { error } = router.query;

  return (
    <div>
      <h1>Error</h1>
      <p>{error ? decodeURIComponent(error as string) : 'An unknown error occurred'}</p>
    </div>
  );
};

export default ErrorPage;
