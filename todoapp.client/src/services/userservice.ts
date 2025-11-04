import { IUser } from "../types/User";
import { createApiInstance } from "../utils/apiUtils";

const api = createApiInstance();

export const UserService = {
    GetUser: async (userId: number) => {
        try {
            const response = await api.get<IUser>(`/users/${userId}`);
            return response.data;
        } catch (error) {
            console.error("Error fetching account info:", error);
            throw error;
        }
    }

}