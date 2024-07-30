// next-auth.d.ts

import NextAuth from 'next-auth';
import { JWT as NextAuthJWT } from 'next-auth/jwt';

// Extend the default NextAuth session interface
declare module 'next-auth' {
  interface Session {
    user: {
      id?: string;
      expiresAt?:number;
    };
  }

  interface User{
    id:string;
    token: string;
    refreshToken: string;
    expiresIn: number;
  }
}

declare module 'next-auth/jwt' {
  interface JWT {
    id: string;
    token:string;
    refreshToken:string;
    accessTokenExpires: number;
  }
}
