// jwt.helper.ts

import { JwtClaims } from "../types/jwtclaims";


/**
 * User info extracted from JWT
 */
export interface UserInfo {
  id: number;
  email: string;
  username: string;
  role: string;
}

export class JwtHelper {
  /**
   * Decode JWT token without verification
   * Note: This only decodes the token, doesn't verify signature
   */
  static decode(token: string): JwtClaims | null {
    try {
      // Remove 'Bearer ' prefix if present
      const cleanToken = token.replace(/^Bearer\s+/i, '');

      // Split token into parts
      const parts = cleanToken.split('.');
      if (parts.length !== 3) {
        throw new Error('Invalid token format');
      }

      // Decode payload (second part)
      const payload = parts[1];
      const decodedPayload = this.base64UrlDecode(payload);

      return JSON.parse(decodedPayload) as JwtClaims;
    } catch (error) {
      console.error('Error decoding JWT:', error);
      return null;
    }
  }

  /**
   * Get user info from JWT token
   */
  static getUserInfo(token: string): UserInfo | null {
    const claims = this.decode(token);
    if (!claims) return null;

    return {
      id: claims.nameid ? parseInt(claims.nameid, 10) : 0,
      email: claims.email || '',
      username: claims.unique_name || '',
      role: claims.role || ''
    };
  }

  /**
   * Get specific claim value
   */
  static getClaim(token: string, claimType: keyof JwtClaims): string | number | undefined {
    const claims = this.decode(token);
    return claims?.[claimType];
  }

  /**
   * Check if token is expired
   */
  static isExpired(token: string): boolean {
    const claims = this.decode(token);
    if (!claims || !claims.exp) return true;

    const now = Math.floor(Date.now() / 1000);
    return claims.exp < now;
  }

  /**
   * Get expiration date
   */
  static getExpirationDate(token: string): Date | null {
    const claims = this.decode(token);
    if (!claims || !claims.exp) return null;

    return new Date(claims.exp * 1000);
  }

  /**
   * Check if token is valid (not expired and properly formatted)
   */
  static isValid(token: string): boolean {
    if (!token) return false;
    const claims = this.decode(token);
    return claims !== null && !this.isExpired(token);
  }

  /**
   * Base64 URL decode helper
   */
  private static base64UrlDecode(str: string): string {
    // Replace URL-safe characters
    let base64 = str.replace(/-/g, '+').replace(/_/g, '/');

    // Add padding if needed
    const padding = base64.length % 4;
    if (padding) {
      base64 += '='.repeat(4 - padding);
    }

    // Decode base64
    if (typeof window !== 'undefined') {
      // Browser environment
      return decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
    } else {
      // Node.js environment
      return Buffer.from(base64, 'base64').toString('utf-8');
    }
  }
}

// // Usage examples:

// // 1. Get user info from token
// const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...';
// const userInfo = JwtHelper.getUserInfo(token);
// console.log(userInfo);
// // { id: 123, email: 'user@example.com', username: 'john_doe', role: 'Admin' }

// // 2. Get specific claim
// const userId = JwtHelper.getClaim(token, 'nameid');
// const role = JwtHelper.getClaim(token, 'role');

// // 3. Check if token is expired
// if (JwtHelper.isExpired(token)) {
//   console.log('Token has expired');
// }

// // 4. Check if token is valid
// if (JwtHelper.isValid(token)) {
//   console.log('Token is valid');
// }

// // 5. Get expiration date
// const expiresAt = JwtHelper.getExpirationDate(token);
// console.log(`Token expires at: ${expiresAt}`);

// // 6. Decode entire token
// const claims = JwtHelper.decode(token);
// console.log(claims);