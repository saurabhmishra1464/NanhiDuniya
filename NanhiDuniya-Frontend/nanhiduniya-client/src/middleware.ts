import { NextResponse } from 'next/server';
import { getToken } from 'next-auth/jwt';
import type { NextRequest } from 'next/server';

export async function middleware(req: NextRequest) {
  const token = await getToken({ req, secret: process.env.NEXTAUTH_SECRET });

  const { pathname } = req.nextUrl;

  if (pathname === '/') {   
    if (!token) {
      return NextResponse.redirect(new URL('/auth/login', req.url));
    } else {
      return NextResponse.redirect(new URL('/dashboard', req.url));
    }
  }

  if (pathname === '/auth/login' && token) {
    return NextResponse.redirect(new URL('/dashboard', req.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: ['/', '/dashboard', '/auth/login'], // Add paths to match
};
