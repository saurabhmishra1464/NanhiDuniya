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
  const token = request.cookies.get('accessToken')?.value;

  const isProtectedRoute = protectedRoutes.some(route => url.pathname.startsWith(route));

  if (isProtectedRoute) {
    if (!token) {
      console.log('No token found, redirecting to login');
      return NextResponse.redirect(new URL('/auth/login', request.url));
    }
    
    if (hasTokenExpired(token)) {
      console.log('Token expired, redirecting to login');
      return NextResponse.redirect(new URL('/auth/login', request.url));
    }

    console.log('Token valid, allowing access to protected route');
  } else if (url.pathname.startsWith('/auth/login')) {
    if (token && !hasTokenExpired(token)) {
      console.log('Valid token found, redirecting to dashboard');
      return NextResponse.redirect(new URL('/admin/dashboard', request.url));
    }
    console.log('No valid token, staying on login page');
  }

  return NextResponse.next();
}


// See "Matching Paths" below to learn more
export const config = {

  matcher: ['/admin/dashboard', '/auth/login', '/settings', '/profile'],
}



// import { NextResponse } from 'next/server';
// import type { NextRequest } from 'next/server';

// export async function middleware(req: NextRequest) {
//   const token = req.cookies.get('access-token');

//   if (!token) {
//     const refreshResponse = await fetch('/api/auth/refresh', {
//       method: 'POST',
//       credentials: 'include',
//     });

//     if (refreshResponse.ok) {
//       return NextResponse.next();
//     } else {
//       return NextResponse.redirect(new URL('/auth/login', req.url));
//     }
//   }

//   return NextResponse.next();
// }

// export const config = {
//   matcher: '/protected/:path*',
// };
