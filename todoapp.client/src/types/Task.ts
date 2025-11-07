import { ISubTask } from "./SubTask";
import { ITag } from "./Tag";

export interface ITask {
    id: number;
    name: string;
    description: string;
    dueDate: Date;
    status: boolean;
    listId: number;
    listName: string;
    createDate: Date;
    subTasks: ISubTask[];
    tags: ITag[];
}

export interface TaskRequest {
    taskId?: number;
    name?: string;
    description?: string;
    listId: number;
    dueDate: Date;
    selectedTags: number[];
}

export interface TaskCountsDto {
    total: number;
    today: number;
    upcoming: number;
    overdue: number;
    completed: number;
}