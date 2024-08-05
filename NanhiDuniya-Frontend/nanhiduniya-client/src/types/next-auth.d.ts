// next-auth.d.ts

import NextAuth from 'next-auth';
import { JWT as NextAuthJWT } from 'next-auth/jwt';

// Extend the default NextAuth session interface
declare module 'next-auth' {
  interface Session {
    user: {
      _id?: string;
      token: string;
      refreshToken: string;
    }& DefaultSession["user"];
    Error?:{
      message?:string;
      statusCode?:number;
    };
  }

  interface User {
    _id?: string;
    expiresAt?: DateTime;
    token: string;
    refreshToken: string;
  }

}
  declare module 'next-auth/jwt' {
    interface JWT {
      _id: string;
      expiresAt?: DateTime;
      token: string;
      refreshToken: string;
      error?:{
        message?:string;
        statusCode?:number;
      };
    }
  }
