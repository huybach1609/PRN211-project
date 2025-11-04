import axios from "axios";
import { API_URL } from "../constrains";
import { getHeaders, getUser } from "../utils/tokenManage";
import { ITask } from "../types/Task";

export interface ListItem {
  id: string;
  name: string;
  tasks: ITask[];
  
}

interface User {
  id: string;
}

export const GetListAccount = async () => {
  const user = JSON.parse(getUser() || "{}") as User;
  return await axios.get(API_URL + `/odata/Lists/user/${user.id}`, {
    headers: getHeaders(),
  });
};

export const GetListById = async (listId: string) => {
  return axios.get(API_URL + `/odata/lists/${listId}`, {
    headers: getHeaders(),
  });
};

export const GetNumOfTaskInfo = async (timestamp: string, listId: number | null) => {
  return axios.get(API_URL + `/count-info/${timestamp}/${listId ?? ''}`, {
    headers: getHeaders(),
  });
};

export const UpdateListAxios = async (selectedItem: ListItem) => {
  return await axios.put(
    API_URL + `/odata/Lists`,
    {
      id: selectedItem.id,
      name: selectedItem.name,
    },
    {
      headers: getHeaders(),
    }
  );
};

export const CreateListAxios = async (selectedItem: ListItem) => {
  const user = JSON.parse(getUser() || "{}") as User;
  return await axios.post(
    API_URL + `/odata/Lists`,
    {
      name: selectedItem.name,
      accountId: user.id,
    },
    {
      headers: getHeaders(),
    }
  );
};

