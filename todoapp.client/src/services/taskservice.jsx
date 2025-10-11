import axios from "axios";
import { getHeaders, getUser } from "../utils/tokenManage";
import { API_URL } from "../constrains";


//----------------------------------------------
// get task by id
export const fetchTaskById = async (taskId) => {
    return axios.get(API_URL + `/api/tasks/get/${taskId}`, {
        headers: getHeaders()
    });
}

// get data by time
export const fetchDataTask = async (timeTag) => {
    return axios.get(API_URL + `/api/tasks/${timeTag}`, {
        headers: getHeaders()
    });
}
// get data by list
export const fetchDataTaskByList = async (listId) => {
    return axios.get(API_URL + `/api/tasks/list/${listId}`, {
        headers: getHeaders()
    });
}

export const fetchUpdateTask = async (requestbody) => {
    return axios.put(API_URL + `/api/tasks/${requestbody.taskId}`,
        requestbody
        , {
            headers: getHeaders()
        })
}
export const fetchCreateTask = async (requestbody) => {
    return axios.post(API_URL + `/api/tasks`,
        requestbody
        , {
            headers: getHeaders()
        })
}
export const fetchDeleteTask = async (taskId) => {
    return axios.delete(API_URL + `/api/tasks/${taskId}`,
        {
            headers: getHeaders()
        })
}


export const fetchSetStatus = async (taskId, status) => {
    return axios.get(API_URL + `/api/tasks/updatestatus/${taskId}/${status}`, {
        headers: getHeaders()
    });
}
//------------------------------
// subtask handle

// add
export const fetchAddSubTask = async (requestbody) => {
    return axios.post(API_URL + `/api/subtasks`,
        requestbody
        , {
            headers: getHeaders()
        })
}
// update status
export const fetchUpdateSubTask = async (requestbody) => {
    return axios.put(API_URL + `/api/subtasks/${requestbody.id}`,
        requestbody
        , {
            headers: getHeaders()
        })
}
// delete
export const fetchDeleteSubTask = async (subtaskId) => {
    return axios.delete(API_URL + `/api/subtasks/${subtaskId}`,
        {
            headers: getHeaders()
        })
}

//-----------------------------------
// sticky note api call
export const fetchDataSn = async () => {

    const user = JSON.parse(getUser());

    return axios.get(API_URL + `/api/StickyNotes/${user.id}`, {
        headers: getHeaders()
    });
}

export const fetchUpdateSn = async (selectedItem) => {

    return await axios.put(`${API_URL}/api/StickyNotes/${selectedItem.id}`,
        {
            id: selectedItem.id,
            name: selectedItem.name,
            detail: selectedItem.details
        },
        {
            headers: getHeaders()
        });
};

export const fetchCreateSn = async (selectedItem) => {
    const user = JSON.parse(getUser());
    return await axios.post(`${API_URL}/api/StickyNotes/${user.id}`,
        {
            id: selectedItem.id,
            name: selectedItem.name,
            detail: selectedItem.details
        },
        {
            headers: getHeaders()
        }
    );

}
export const fetchDeleteSn = async (taskId) => {
    const user = JSON.parse(getUser());
    return await axios.delete(`${API_URL}/api/StickyNotes/${user.id}/${taskId}`,
        {
            headers: getHeaders()
        }
    );
}
