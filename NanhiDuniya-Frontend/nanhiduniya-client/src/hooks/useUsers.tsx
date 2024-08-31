
import axiosInstance from '@/utils/AxiosInstances/api';
import fetcher from '@/utils/fetcher';

import useSWR from 'swr';

function useUser () {
  const { data, error, isLoading } = useSWR(`/api/Account/me`, fetcher,{ revalidateOnFocus: false, revalidateOnReconnect: false,refreshInterval: 0 })
 
  return {
    user: data,
    isLoading,
    isError: error,
  }
}

export default useUser;