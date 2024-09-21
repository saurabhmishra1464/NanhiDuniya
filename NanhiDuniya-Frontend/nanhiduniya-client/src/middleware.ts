

import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// export const protectedRoutes = [
//   '/admin/dashboard',
//   '/settings',
//   '/profile',
//   // Add other protected routes here
// ];

export const PublicRotes = ['/auth/login','/signup','/forgot-password','/auth/verifyEmail', '/auth/resetPassword','/auth/forgotPassword'];

export function middleware(request: NextRequest) {
  const path = request.nextUrl.pathname;

  // Define public paths that don't require authentication
  // const isProtectedRoute = protectedRoutes.some(route => path.startsWith(route));
  const isPublicPath = PublicRotes.some(route=>path.startsWith(route));
  // Get the access token from cookies

  const refreshToken = request.cookies.get('X-Refresh-Token')?.value;

  // If accessing a public path and user is already authenticated, redirect to dashboard
  if (isPublicPath) {
    if (refreshToken) {
      // User is authenticated on a public route
      return NextResponse.redirect(new URL('/admin/dashboard', request.url));
    }
    return NextResponse.next();
  }

  // If accessing a protected path and user is not authenticated
  if (!isPublicPath && !refreshToken) {
    // Redirect to login page
    return NextResponse.redirect(new URL('/auth/login', request.url));
  }


  // Allow the request to continue
  return NextResponse.next();
}

// Specify which routes this middleware should run on
export const config = {
  matcher: [
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
};