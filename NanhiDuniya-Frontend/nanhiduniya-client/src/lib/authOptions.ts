import NextAuth, { NextAuthOptions, User } from 'next-auth';
import CredentialsProvider from 'next-auth/providers/credentials';
import { axiosInstance } from '@/utils/AxiosInstances/api';
import { JWT } from 'next-auth/jwt';

export const authOptions: NextAuthOptions = {
  session: {
    strategy: 'jwt',
    // maxAge: 30 * 24 * 60 * 60, // 30 days
     maxAge: 10 * 60, // 10 minutes
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
    async jwt({ token, user, account }) {
      // Initial sign in
      if (account && user) {
        return {
          accessToken: account.access_token,
          accessTokenExpires: Date.now() + account.expires_in * 1000,
          refreshToken: account.refresh_token,
          user
        }
      }

      // Return previous token if the access token has not expired yet
      if (Date.now() < token.accessTokenExpires) {
        return token
      }

      // Access token has expired, try to update it
      return refreshAccessToken(token)
    },
    async session({ session, token }) {
      // session.user = token.user
      // session.accessToken = token.accessToken
      // session.error = token.error

      return session
    }
  },
  pages:{
signIn: '/auth/login'
  },
  events: {
    async signOut({ session, token }) {
      // Perform any cleanup or additional logout actions here
      // For example, you might want to invalidate the token on your backend
      try {
        await axiosInstance.post('/api/Account/Logout', {}, {
          headers: { Authorization: `Bearer ${token.accessToken}` }
        });
      } catch (error) {
        console.error('Error during logout:', error);
      }
    },
  },
  secret: process.env.NEXTAUTH_SECRET,
  useSecureCookies: process.env.NODE_ENV === 'production',
};

async function refreshAccessToken(token:JWT) {
  try {
    const response = await axiosInstance.post('/api/Account/RefreshToken', {
     UserId: token.id,
      Token: token.token,
     refreshToken: token.refreshToken,
    });

    if (!response.data.ok) {
      throw response.data.message;
    }
    
      return {
        ...token,
        accessToken: response.data.token,
        accessTokenExpires: Date.now() + response.data.expiresIn * 1000,
        refreshToken: response.data.refreshToken ?? token.refreshToken,
        expiresIn: response.data.expiresIn,
  }
  }
   catch (error) {
    console.log(error);
    return {
      ...token,
      error: 'RefreshAccessTokenError'
    }
  }
}

export default NextAuth(authOptions);