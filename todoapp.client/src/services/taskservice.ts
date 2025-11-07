import { IStickyNote } from "../types/StickyNote";
import { ISubTask } from "../types/SubTask";
import { TaskRequest, TaskCountsDto } from "../types/Task";

import { createApiInstance } from "../utils/apiUtils";
const api = createApiInstance();
//----------------------------------------------
export const TaskService = {

  fetchTaskCount: async (): Promise<{ data: TaskCountsDto }> => {
    return api.get(`/tasks/task-count`);
  },

  // get task by id
  fetchTaskById: async (taskId: number) => {
    return api.get(`/tasks/get/${taskId}`);
  },

  // get data by time
  fetchDataTask: async (timeTag: string) => {
    return api.get(`/tasks/${timeTag}`);
  },
  // get data by list
  fetchDataTaskByList: async (listId: number) => {
    return api.get(`/tasks/list/${listId}`);
  },

  fetchUpdateTask: async (requestbody: TaskRequest) => {
    return api.put(`/tasks/${requestbody.taskId}`, requestbody);
  },
  fetchCreateTask: async (requestbody: TaskRequest) => {
    return api.post(`/tasks`, requestbody);
  },
  fetchDeleteTask: async (taskId: number) => {
    return api.delete(`/tasks/${taskId}`);
  },

  fetchSetStatus: async (taskId: number, status: boolean | string) => {
    return api.get(`/tasks/updatestatus/${taskId}/${status}`);
  },

  // Get task counts by list
  fetchTaskCountByList: async (listId: number = 0): Promise<{ data: TaskCountsDto }> => {
    return api.get(`/tasks/task-count?listId=${listId}`);
  }
};

//------------------------------
// subtask handle

// add
export const fetchAddSubTask = async (requestbody: any) => {
  return api.post(`/subtasks`, requestbody);
};
// update status
export const fetchUpdateSubTask = async (requestbody: ISubTask) => {
  return api.put(`/subtasks/${requestbody.id}`, requestbody);
};
// delete
export const fetchDeleteSubTask = async (subtaskId: number) => {
  return api.delete(`/subtasks/${subtaskId}`);
};

//-----------------------------------
// sticky note api call
export const StickyNoteService = {
  fetchDataSn: async () => {
    // GET: api/stickynotes - returns all sticky notes for current user
    return api.get(`/stickynotes`);
  },

  fetchUpdateSn: async (selectedItem: IStickyNote) => {
    // PUT: api/stickynotes?id={id} - updates sticky note
    return await api.put(
      `/stickynotes/${selectedItem.id}`,
      {
        id: selectedItem.id,
        name: selectedItem.name,
        details: selectedItem.details,
      }
    );
  },

  fetchCreateSn: async (selectedItem: IStickyNote) => {
    // POST: api/stickynotes - creates new sticky note
    // Note: id should be null/0 for creation, name is required
    return await api.post(
      `/stickynotes`,
      {
        id: selectedItem.id ?? 0,
        name: selectedItem.name,
        details: selectedItem.details,
      }
    );
  },
  fetchDeleteSn: async (stickyNoteId: number) => {
    // DELETE: api/stickynotes/{id} - deletes sticky note
    return await api.delete(`/stickynotes/${stickyNoteId}`);
  }
};
