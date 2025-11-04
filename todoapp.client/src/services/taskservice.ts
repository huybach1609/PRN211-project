import axios from "axios";
import { getHeaders, getUser } from "../utils/tokenManage";
import { API_URL } from "../constrains";
import { IStickyNote } from "../types/StickyNote";
import { ISubTask } from "../types/SubTask";
import { TaskRequest } from "../types/Task";

interface User {
  id: string;
}

interface RequestBody {
  taskId?: string;
  [key: string]: any;
}

interface SubTaskRequest {
  id?: string;
  [key: string]: any;
}

interface StickyNote {
  id?: string;
  name: string;
  details?: string;
}

//----------------------------------------------
// get task by id
export const fetchTaskById = async (taskId: number) => {
  return axios.get(API_URL + `/tasks/get/${taskId}`, {
    headers: getHeaders(),
  });
};

// get data by time
export const fetchDataTask = async (timeTag: string) => {
  return axios.get(API_URL + `/tasks/${timeTag}`, {
    headers: getHeaders(),
  });
};
// get data by list
export const fetchDataTaskByList = async (listId: number) => {
  return axios.get(API_URL + `/tasks/list/${listId}`, {
    headers: getHeaders(),
  });
};

export const fetchUpdateTask = async (requestbody: TaskRequest) => {
  return axios.put(API_URL + `/tasks/${requestbody.taskId}`, requestbody, {
    headers: getHeaders(),
  });
};
export const fetchCreateTask = async (requestbody: TaskRequest) => {
  return axios.post(API_URL + `/tasks`, requestbody, {
    headers: getHeaders(),
  });
};
export const fetchDeleteTask = async (taskId: number) => {
  return axios.delete(API_URL + `/tasks/${taskId}`, {
    headers: getHeaders(),
  });
};

export const fetchSetStatus = async (taskId: number, status: boolean | string) => {
  return axios.get(API_URL + `/tasks/updatestatus/${taskId}/${status}`, {
    headers: getHeaders(),
  });
};
//------------------------------
// subtask handle

// add
export const fetchAddSubTask = async (requestbody: any) => {
  return axios.post(API_URL + `/subtasks`, requestbody, {
    headers: getHeaders(),
  });
};
// update status
export const fetchUpdateSubTask = async (requestbody: ISubTask) => {
  return axios.put(API_URL + `/subtasks/${requestbody.id}`, requestbody, {
    headers: getHeaders(),
  });
};
// delete
export const fetchDeleteSubTask = async (subtaskId: number) => {
  return axios.delete(API_URL + `/subtasks/${subtaskId}`, {
    headers: getHeaders(),
  });
};

//-----------------------------------
// sticky note api call
export const StickyNoteService = {
  fetchDataSn: async () => {
    const user = JSON.parse(getUser() || "{}") as User;

    return axios.get(API_URL + `/stickynotes/${user.id}`, {
      headers: getHeaders(),
    });
  },

  fetchUpdateSn: async (selectedItem: IStickyNote) => {
    return await axios.put(
      `${API_URL}/stickynotes/${selectedItem.id}`,
      {
        id: selectedItem.id,
        name: selectedItem.name,
        details: selectedItem.details,
      },
      {
        headers: getHeaders(),
      }
    );
  },

  fetchCreateSn: async (selectedItem: IStickyNote) => {
    const user = JSON.parse(getUser() || "{}") as User;
    return await axios.post(
      `${API_URL}/stickynotes/${user.id}`,
      {
        id: selectedItem.id,
        name: selectedItem.name,
        detail: selectedItem.details,
      },
      {
        headers: getHeaders(),
      }
    );
  },
  fetchDeleteSn: async (stickyNoteId: number) => {
    const user = JSON.parse(getUser() || "{}") as User;
    return await axios.delete(`${API_URL}/stickynotes/${user.id}/${stickyNoteId}`, {
      headers: getHeaders(),
    });
  }
};
