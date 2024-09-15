
import { useLogoutUserMutation } from '@/services/auth';
import { logOut } from '@/store/auth-slice';
import { 
    createApi, 
    fetchBaseQuery, 
    BaseQueryFn,
    FetchArgs,
    FetchBaseQueryError,
  } from '@reduxjs/toolkit/query/react'
  import { Mutex } from 'async-mutex'
// Create a mutex
const mutex = new Mutex()

const baseQuery = fetchBaseQuery({ 
  baseUrl: process.env.NEXT_PUBLIC_API_URL,
  credentials: 'include', // This is crucial for sending cookies
})

export const baseQueryWithReauth: BaseQueryFn<
  string | FetchArgs,
  unknown,
  FetchBaseQueryError
> = async (args, api, extraOptions) => {
  debugger
  // wait until the mutex is available without locking it
  await mutex.waitForUnlock()
  let result = await baseQuery(args, api, extraOptions)
  if (result.error && result.error.status === 401) {
    // checking whether the mutex is locked
    if (!mutex.isLocked()) {
      const release = await mutex.acquire()
      try {
        // try to refresh the session
        const refreshResult = await baseQuery('/api/Account/refresh', api, extraOptions)
        if (refreshResult.data) {
          // Retry the initial query
          result = await baseQuery(args, api, extraOptions)
        } else {
          // If refresh fails, handle logout
        //  useLogoutUserMutation();
           api.dispatch(logOut());
           window.location.href = '/auth/login';
        }
      } finally {
        // release must be called once the mutex should be released again.
        release()
      }
    } else {
      // wait until the mutex is available without locking it
      await mutex.waitForUnlock()
      result = await baseQuery(args, api, extraOptions)
    }
  }
  return result
}

// You might want to add a method to check if the user is authenticated
// export const checkAuth = authApi.injectEndpoints({
//     endpoints: (build) => ({
//       getAuthStatus: build.query<{ isAuthenticated: boolean }, void>({
//         query: () => '/api/Account/check-auth',
//       }),
//     }),
//   })
  
//   export const { useGetAuthStatusQuery } = checkAuth