import { createContext, useContext, useEffect, useState, useCallback, useMemo } from "react";
import { IUser } from "../types/User";
import { UserService } from "../services/userservice";
import { JwtHelper } from "../utils/jwt.helper";
import { JWT_STORAGE_KEY } from "../constrains";

// Types
export interface UserAuthState {
    isAuthenticated: boolean;
    user: IUser | null;
    token: string | null;
    isLoading: boolean;
}

interface AuthContextType extends UserAuthState {
    login: (token: string, user: IUser) => void;
    logout: () => void;
}

// Context
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Provider Component
export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [authState, setAuthState] = useState<UserAuthState>({
        isAuthenticated: false,
        user: null,
        token: null,
        isLoading: true,
    });

    /**
     * Logout user and clear all auth data
     */
    const logout = useCallback(() => {
        localStorage.removeItem(JWT_STORAGE_KEY);
        setAuthState({
            isAuthenticated: false,
            user: null,
            token: null,
            isLoading: false,
        });
    }, []);

    /**
     * Validate and restore user session from stored JWT
     */
    const restoreSession = useCallback(async () => {
        try {
            const token = localStorage.getItem(JWT_STORAGE_KEY);
            
            // No token found
            if (!token) {
                logout();
                return;
            }

            // Check if token is expired or invalid
            if (!JwtHelper.isValid(token)) {
                console.warn("Token is invalid or expired");
                logout();
                return;
            }

            // Extract user ID from token
            const userIdClaim = JwtHelper.getClaim(token, 'nameid');
            const userId = userIdClaim ? parseInt(userIdClaim as string, 10) : null;

            if (!userId || isNaN(userId)) {
                console.error("Invalid user ID in token");
                logout();
                return;
            }

            // Fetch user data from backend
            const user = await UserService.GetUser(userId);

            if (!user) {
                console.error("User not found");
                logout();
                return;
            }

            // Restore authenticated state
            setAuthState({
                isAuthenticated: true,
                user: user,
                token: token,
                isLoading: false,
            });
        } catch (error) {
            console.error("Error restoring session:", error);
            logout();
        }
    }, [logout]);

    /**
     * Initialize auth state on mount
     */
    useEffect(() => {
        restoreSession();
    }, [restoreSession]);

    /**
     * Login user with token and user data
     */
    const login = useCallback((token: string, user: IUser) => {
        localStorage.setItem(JWT_STORAGE_KEY, token);
        setAuthState({
            isAuthenticated: true,
            user: user,
            token: token,
            isLoading: false,
        });
    }, []);

    /**
     * Memoize context value to prevent unnecessary re-renders
     */
    const contextValue = useMemo(
        () => ({
            ...authState,
            login,
            logout,
        }),
        [authState, login, logout]
    );

    return (
        <AuthContext.Provider value={contextValue}>
            {children}
        </AuthContext.Provider>
    );
};

/**
 * Custom hook to access auth context
 * @throws Error if used outside AuthProvider
 */
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    
    if (context === undefined) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    
    return context;
};

export default AuthContext;