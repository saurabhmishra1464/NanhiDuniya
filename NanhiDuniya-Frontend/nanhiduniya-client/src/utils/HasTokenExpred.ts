// utils/HasTokenExpired.ts

import jwt from 'jsonwebtoken';
export function hasTokenExpired(token: string | null): boolean {
  if (!token) {
    return true;
  }
  const decodedToken: any = jwt.decode(token);
  const currentTime = Math.floor(Date.now() / 1000);
  const expirationTime = decodedToken.exp as number | undefined;
  if (!expirationTime) {
    // If there's no expiration time, assume the token is not expired
    return false;
  }
  const isExpired = currentTime >= expirationTime;
  // Compare the current time with the expiration time
  return currentTime >= expirationTime;
}