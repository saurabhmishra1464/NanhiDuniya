// middleware.ts
import { getToken } from 'next-auth/jwt';
import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'
import { hasTokenExpired } from './utils/HasTokenExpred';

export const protectedRoutes = [
  '/admin/dashboard',
  '/settings',
  '/profile',
  // Add other protected routes here
];

export async function middleware(request: NextRequest) {
  const url = request.nextUrl;
  const token = await getToken({ req: request });
  const isProtectedRoute = protectedRoutes.some(route => url.pathname.startsWith(route));

  if (isProtectedRoute) {
    if (!token || hasTokenExpired(token)) {
      return NextResponse.redirect(new URL('/auth/login', request.url));
    }
  } else {
    if (url.pathname.startsWith('/auth/login') && token) {
      return NextResponse.redirect(new URL('/admin/dashboard', request.url));
    }
  }

  return NextResponse.next();
}

// See "Matching Paths" below to learn more
export const config = {

  matcher: ['/admin/dashboard', '/auth/login', '/settings', '/profile'],
}