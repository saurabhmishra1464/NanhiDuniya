import NextAuth, { NextAuthOptions, User } from 'next-auth';
import CredentialsProvider from 'next-auth/providers/credentials';
import { axiosInstance } from '@/utils/AxiosInstances/api';
import { signOut } from 'next-auth/react';
import { JWT } from 'next-auth/jwt';

let lastRefreshTime = 0;
export const authOptions: NextAuthOptions = {
  session: {
    strategy: 'jwt',
    maxAge: 15 * 24 * 60 * 60,
  },
  providers: [
    CredentialsProvider({
      name: 'Credentials',
      credentials: {
        email: { label: 'Email', type: 'text' },
        password: { label: 'Password', type: 'password' },
      },
      authorize: async (credentials) => {
        if (!credentials?.email || !credentials?.password) {
          throw new Error('Email and password are required');
        }
        try {
          const response = await axiosInstance.post('/api/Account/Login', {
            email: credentials.email,
            password: credentials.password,
          });
          if (response.status === 200 && response.data) {
            return {
              id: response.data.userId,
              token: response.data.token,
              refreshToken: response.data.refreshToken,
              expiresAt: response.data.expiresAt,
              expiresIn: response.data.expiresIn,
            };
          } else {
            throw new Error(response.data.message || 'Authentication failed');
          }
        } catch (error) {
          throw new Error('Authentication failed. Please try again.');
        }
      },
    }),
  ],
  callbacks: {
    async jwt({ token, user }) {
      if (user) {
        token._id = user.id;
        token.expiresAt = user.expiresAt;
        token.token = user.token;
        token.refreshToken = user.refreshToken;
      }

      if (Date.now() > new Date(token.expiresAt).getTime()- 30 * 1000) {
        lastRefreshTime = Date.now();
        const refreshedToken = await refreshAccessToken(token);
        if (refreshedToken) {
          token = refreshedToken;
        }
      }
      return token;
    },
    async session({ session, token }) {
      if (token) {
        session.user._id = token._id;
         session.user.token = token.token;
      }
      return session;
    },
  },
  pages: {
    signIn: '/auth/login',
  },
  events: {
  },
  secret: process.env.NEXTAUTH_SECRET,
  debug: process.env.NODE_ENV === 'development',
};

async function refreshAccessToken(token: any) {
  debugger
  // try {
  const response = await axiosInstance.post('/api/Account/RefreshToken', {
    token: token.token,
    refreshToken: token.refreshToken,
    expiresAt: token.expiresAt,
  });

  return {
    ...token,
    token: response.data.token,
    refreshToken: response.data.refreshToken ?? token.refreshToken,
    expiresAt: response.data.expiresAt,
  };
}

export default NextAuth(authOptions);