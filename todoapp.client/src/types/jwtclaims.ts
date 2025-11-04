/**
 * JWT Claims interface matching your C# implementation
 */
export interface JwtClaims {
    nameid?: string;  // ClaimTypes.NameIdentifier -> user.Id
    email?: string;   // ClaimTypes.Email
    unique_name?: string; // ClaimTypes.Name -> user.UserName
    role?: string;    // ClaimTypes.Role
    exp?: number;     // Expiration time
    iat?: number;     // Issued at
    nbf?: number;     // Not before
  }
  