import { createApiInstance } from "../utils/apiUtils";

const api = createApiInstance();

export const TagService = {
    GetTags: async () => {
        return await api.get(`/tags`);
    }
}