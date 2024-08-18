
import axiosInstance from '@/utils/AxiosInstances/api';
import fetcher from '@/utils/fetcher';

import useSWR from 'swr';

function useUser (id:string) {
  const { data, error, isLoading } = useSWR(`/api/Account/Users/${id}`, fetcher)
 
  return {
    user: data,
    isLoading,
    isError: error
  }
}

export default useUser;