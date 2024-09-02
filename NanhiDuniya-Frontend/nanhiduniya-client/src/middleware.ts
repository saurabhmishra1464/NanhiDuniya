
// middleware.ts
// import { getToken } from 'next-auth/jwt';
// import { NextResponse } from 'next/server'
// import type { NextRequest } from 'next/server'
// import { hasTokenExpired } from './utils/HasTokenExpred';
// export const protectedRoutes = [
//   '/admin/dashboard',
//   '/settings',
//   '/profile',
//   // Add other protected routes here
// ];

// export async function middleware(request: NextRequest) {
//   const url = request.nextUrl;
//   const token = request.cookies.get('accessToken')?.value;
//   console.log(token);

//   const isProtectedRoute = protectedRoutes.some(route => url.pathname.startsWith(route));

//   if (isProtectedRoute) {
//     if (!token) {
//       console.log('No token found, redirecting to login');
//       return NextResponse.redirect(new URL('/auth/login', request.url));
//     }
    
//     if (hasTokenExpired(token)) {
//       console.log('Token expired, redirecting to login');
//       return NextResponse.redirect(new URL('/auth/login', request.url));
//     }

//     console.log('Token valid, allowing access to protected route');
//   } else if (url.pathname.startsWith('/auth/login')) {
//     if (token && !hasTokenExpired(token)) {
//       console.log('Valid token found, redirecting to dashboard');
//       return NextResponse.redirect(new URL('/admin/dashboard', request.url));
//     }
//     console.log('No valid token, staying on login page');
//   }

//   return NextResponse.next();
// }


// // See "Matching Paths" below to learn more
// export const config = {

//   matcher: ['/admin/dashboard', '/auth/login', '/settings', '/profile'],
// }



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






import { getToken } from 'next-auth/jwt';
import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'
import { hasTokenExpired } from './utils/HasTokenExpred';
import axios from 'axios';
const https = require('https');

export const protectedRoutes = [
  '/admin/dashboard',
  '/settings',
  '/profile',
  // Add other protected routes here
];

export async function middleware(request: NextRequest) {
  console.log("middelware ran");
  console.log(request.nextUrl.pathname);
  const url = request.nextUrl;
  // const token = request.cookies.get('accessToken')?.value;
  const refrehtoken = request.cookies.get('X-Refresh-Token')?.value;
  const isProtectedRoute = protectedRoutes.some(route => url.pathname.startsWith(route));
// console.log(token);
  if (isProtectedRoute) {
    if (!refrehtoken) {
//       console.log("inside tokenexpiredddddddddddddddddddddddddddd");
//       console.log(token);
//         // const refreshUrl = new URL('/api/refresh-token', request.url).toString();
//         // console.log(request.cookies.get('refreshToken')?.value);
//         // console.log(request.cookies.get('accessToken')?.value);
//         // const refreshResponse = await axios.post(refreshUrl, {
//         //   // credentials: 'include',
//         //   body: JSON.stringify({
//         //     Token: request.cookies.get('accessToken')?.value,
//         //     RefreshToken: request.cookies.get('refreshToken')?.value,
//         //   }),
//         //   headers: { 'Content-Type': 'application/json' },
//         // });
//     //   const fetchResponse = await axiosInstance.post('/api/Account/RefreshToken', {
//     //     Token: request.cookies.get('accessToken')?.value,
//     //     RefreshToken: request.cookies.get('refreshToken')?.value,
//     // });
// try{

//     const response = await getToken1(request);
//   }catch(error:any){
//     console.error('Error making request:', error);
//   // if (error.response) {
//   //   console.error('Response error:', error.response.data);
//   // } else if (error.request) {
//   //   console.error('Request error:', error.request);
//   // } else {
//   //   console.error('General error:', error.message);
//   // }
//   }
// // console.log("accestoken hai ye", token);
// // console.log("refreshtoken hai ye", refrehtoken);
//     // const fetchResponse = await fetch(refreshUrl, {
//     //   method: 'POST',
//     //   headers: { 'Content-Type': 'application/json' },
//     //   body: JSON.stringify({
//     //     Token: request.cookies.get('accessToken')?.value,
//     //     RefreshToken: request.cookies.get('refreshToken')?.value,
//     //   }),
//     // })
    
//         // console.log(fetchResponse);
      console.log('Token expired, redirecting to login');
     return NextResponse.redirect(new URL('/auth/login', request.url));
      
    }
    //Token valid, allowing access to protected route
  } else if (url.pathname.startsWith('/auth/login')) {
    if (refrehtoken) {
      //Valid token found, redirecting to dashboard
      return NextResponse.redirect(new URL('/admin/dashboard', request.url));
    }
    //No valid token, staying on login page
  }

  return NextResponse.next();
}


// See "Matching Paths" below to learn more
export const config = {

  matcher: ['/admin/dashboard', '/auth/login', '/settings', '/profile'],
}










// // // middleware.ts
// // import { getToken } from 'next-auth/jwt';
// // import { NextResponse } from 'next/server'
// // import type { NextRequest } from 'next/server'
// // import { hasTokenExpired } from './utils/HasTokenExpred';

// // export const protectedRoutes = [
// //   '/admin/dashboard',
// //   '/settings',
// //   '/profile',
// //   // Add other protected routes here
// // ];

// // export async function middleware(request: NextRequest) {
// //   const url = request.nextUrl;
// //   const token = await getToken({ req: request });
// //   console.log(token);
// //   const isProtectedRoute = protectedRoutes.some(route => url.pathname.startsWith(route));

// //   if (isProtectedRoute) {
// //     if (!token || hasTokenExpired(token)) {
// //       return NextResponse.redirect(new URL('/auth/login', request.url));
// //     }
// //   } else {
// //     if (url.pathname.startsWith('/auth/login') && token) {
// //       return NextResponse.redirect(new URL('/admin/dashboard', request.url));
// //     }
// //   }

// //   return NextResponse.next();
// // }

// // // See "Matching Paths" below to learn more
// // export const config = {

// //   matcher: ['/admin/dashboard', '/auth/login', '/settings', '/profile'],
// // }