import { NextRequest, NextResponse } from 'next/server';
import axiosInstance from '@/utils/AxiosInstances/api';
import { handleError } from '@/utils/ErrorHandelling/errorHandler';

export async function POST(request: NextRequest) {
    const reqBody = await request.json();
    const { email, password } = reqBody;

    try {
        const response = await axiosInstance.post('/api/Account/Login', { email, password }, {
            withCredentials: true, // Include cookies in the request
        });

        // Forward the cookies to the client
        const setCookieHeader = Array.isArray(response.headers['set-cookie'])
        ? response.headers['set-cookie'].join('; ')
        : response.headers['set-cookie'];

    return NextResponse.json(response.data, { headers: { 'Set-Cookie': setCookieHeader || '' } });
} catch (error: any) {
  const status = error.response?.status || 500;
  const message = error.response?.data?.message || 'Login failed';

  return NextResponse.json({ message }, { status });
}
}