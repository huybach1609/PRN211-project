import { ITask } from "./Task";
import { IUser } from "./User";

export interface IList {
    id: number;
    name: string;
    userId: number;
    user: IUser;
    tasks: ITask[];
}

export interface ListResponseDto {
    result: IList;
    numberOfTaskInfo: number;
}
