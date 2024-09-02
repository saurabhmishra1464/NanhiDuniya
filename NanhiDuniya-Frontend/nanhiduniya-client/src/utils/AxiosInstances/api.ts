import { useAuth } from '@/context/AuthProvider';
import axios from 'axios';

export const axiosPublic = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true
});

export const axiosPrivate = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true
});

axiosPrivate.interceptors.response.use(
  response => response,
  async (error) => {
      const prevRequest = error?.config;
      if (error?.response?.status === 401 && !prevRequest?.sent) {
          prevRequest.sent = true;
          await refresh();
          return axiosPrivate(prevRequest);
      }
      return Promise.reject(error);
  }
);

const refresh = async () => {
  await axiosPublic.get('/api/Account/refresh');
}