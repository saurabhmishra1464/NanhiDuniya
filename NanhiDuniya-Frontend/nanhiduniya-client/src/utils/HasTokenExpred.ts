// utils/HasTokenExpired.ts

import { JWT } from "next-auth/jwt";

export function hasTokenExpired(token: JWT | null): boolean {
  if (!token) {
    
    return true;
  }

  const currentTime = Math.floor(Date.now() / 1000);
  const expirationTime = token.exp as number | undefined;
  if (!expirationTime) {
    // If there's no expiration time, assume the token is not expired
    return false;
  }

  // Compare the current time with the expiration time
  return currentTime >= expirationTime;
}