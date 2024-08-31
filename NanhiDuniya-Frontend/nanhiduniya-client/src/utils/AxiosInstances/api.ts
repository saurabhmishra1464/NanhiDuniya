import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  withCredentials: true,
});

let isRefreshing = false;
let refreshTokenPromise: Promise<string> | null = null;

axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      if (!isRefreshing) {
        isRefreshing = true;

        refreshTokenPromise = axiosInstance.post("/api/Account/RefreshToken")
          .then(({ data }) => {
            axiosInstance.defaults.headers.common['Authorization'] = 'Bearer ' + data.accessToken;
            return data.accessToken;
          })
          .catch(() => {
            refreshTokenPromise = null;
            isRefreshing = false;
            return Promise.reject(new Error('Refresh token failed'));
          })
          .finally(() => {
            isRefreshing = false;
            refreshTokenPromise = null;
          });
      }

      try {
        const newToken = await refreshTokenPromise;
        originalRequest.headers['Authorization'] = 'Bearer ' + newToken;
        return axiosInstance(originalRequest);
      } catch (err) {
        return Promise.reject(err);
      }
    }

    return Promise.reject(error);
  }
);

export default axiosInstance;
