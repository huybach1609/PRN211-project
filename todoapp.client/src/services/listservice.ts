
import { ITask } from "../types/Task";
import { createApiInstance } from "../utils/apiUtils";

export interface ListItem {
  id: string;
  name: string;
  tasks: ITask[];

}
const api = createApiInstance();

export const ListService = {
  GetLists: async () => {
    return await api.get(`/lists?includeCounts=true`);
  },


  GetListById: async (listId: number) => {
    return await api.get(`/lists/${listId}`);
  },

  GetNumOfTaskInfo: async (timestamp: string, listId: number | null) => {
    return await api.get(`/count-info/${timestamp}/${listId ?? ''}`);
  },

  UpdateList: async (selectedItem: ListItem) => {
    return await api.put(`/lists`, selectedItem);
  },

  CreateList: async (selectedItem: ListItem) => {
    return await api.post(`/lists`, selectedItem);
  },
};

