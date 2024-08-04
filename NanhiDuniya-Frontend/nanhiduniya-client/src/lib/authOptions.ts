import NextAuth, { NextAuthOptions, User } from 'next-auth';
import CredentialsProvider from 'next-auth/providers/credentials';
import { axiosInstance } from '@/utils/AxiosInstances/api';
import { signOut } from 'next-auth/react';
import { JWT } from 'next-auth/jwt';

export const authOptions: NextAuthOptions = {
  session: {
    strategy: 'jwt',
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
          console.error('Error during authentication:', error);
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
      // if (Date.now() > new Date(token.expiresAt).getTime()) {
      //   return await refreshAccessToken(token);
      // }
      return token;
    },
    async session({ session, token }) {
      if (token) {
        session.user._id = token._id;
        session.user.token = token.token;
        session.user.refreshToken = token.refreshToken;
//         Server-Side Token Storage: Store access tokens on the server side, ideally in a secure database or encrypted storage.
// Token Issuance: When a user authenticates, issue a session ID or a similar token that can be safely stored on the client side (e.g., in a cookie or local storage).
// Token Retrieval: On each request, the client sends the session ID or token to the server. The server then retrieves the corresponding access token from its secure storage and uses it for authorization.
      }
      return session;
    },
  },
  pages: {
    signIn: '/auth/login',
  },
  events: {
    async signOut({ session, token }) {
      try {
        await axiosInstance.post('/api/Account/revoke-refresh-token');
        await signOut({ callbackUrl: '/auth/login' });
      } catch (error) {
        console.error('Error during logout:', error);
      }
    },
  },
  secret: process.env.NEXTAUTH_SECRET,
  debug: process.env.NODE_ENV === 'development',
};

async function refreshAccessToken(token:JWT) {
  try {
    const response = await axiosInstance.post('/api/Account/RefreshToken', {
      token: token.token,
      refreshToken: token.refreshToken,
    });
    if (!response.data.ok) {
      throw response.data.message;
    }
    return {
      ...token,
      token: response.data.token,
      refreshToken: response.data.refreshToken ?? token.refreshToken,
      expiresIn: response.data.expiresIn,
    };
  } catch (error) {
    console.log(error);
    return { ...token, error: 'RefreshAccessTokenError' };
  }
}

export default NextAuth(authOptions);