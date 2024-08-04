import { getToken } from 'next-auth/jwt';
import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'
import { hasTokenExpired } from './utils/HasTokenExpred';


export async function middleware(request: NextRequest) {
  const url = request.nextUrl;
  const token = await getToken({ req: request });
  console.log("inside Midelwarea",token);
//  localStorage.setItem('accessToken', token?.token as string);

  if (token) {
    if (url.pathname.startsWith('/auth/login')) {
      return NextResponse.redirect(new URL('/admin/dashboard', request.url));
    }
  } else {
    if (url.pathname.startsWith('/admin/dashboard')) {
      return NextResponse.redirect(new URL('/auth/login', request.url));
    }
  }

  const isTokenExpired = hasTokenExpired(token);
  if(!token || isTokenExpired) {
    if(url.pathname.startsWith('/auth/login')) {
      return NextResponse.next();
  }
  return NextResponse.redirect(new URL('/auth/login', request.url));
}

  return NextResponse.next();
}


// See "Matching Paths" below to learn more
export const config = {

  matcher: ['/', '/admin/dashboard', '/auth/login'],
}