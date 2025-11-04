import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";
import { Button } from "@heroui/button";
import { ChevronRight, Cross, Menu, Plus, Save, Trash, Trash2 } from "lucide-react";
import { addToast, Checkbox, DatePicker, Input, ScrollShadow, Select, SelectItem } from "@heroui/react";
import Tiptaptext from "../richtextbox/tiptaptext";
import Tiptap from "../richtextbox/Tiptap";
import { GetListAccount } from "../../services/listservice";
import { fetchCreateTask, fetchDeleteTask, fetchTaskById, fetchUpdateSn, fetchUpdateTask } from "../../services/taskservice";
import { getLocalTimeZone, parseDate, today } from "@internationalized/date";
import { SubtaskComponent } from "./subtaskcomponent";
import { ITask, TaskRequest } from "../../types/Task";
import { ITag } from "../../types/Tag";
import { ISubTask } from "../../types/SubTask";
import { GetTags } from "../ui/sizebar";

interface TaskLayoutProps {
    isOpen: boolean;
    setOpen: (open: boolean) => void;
    taskId?: number;
    onTaskSaved?: (task: ITask, action?: 'update' | 'delete') => void;
}

interface ListItem {
    id: number;
    name: string;
}

function TaskLayout({ isOpen, setOpen, taskId = 0, onTaskSaved }: TaskLayoutProps) {
    const [name, setName] = useState<string>('');
    const [task, setTask] = useState<ITask | null>(null);
    const [description, setDescription] = useState<string>('');

    const [listId, setListId] = useState<number>(0);
    const [list1, setList1] = useState<ListItem[]>([]);

    const [dueDate, setDueDate] = useState<string>("2024-04-04");

    const [tags, setTags] = useState<ITag[]>([]);
    const [selectedTags, setSelectedTags] = useState<Set<string>>(new Set([]));

    const [subTasks, setSubtasks] = useState<ISubTask[]>([]);

    const resetTaskStates = () => {
        setName('');
        setTask(null);
        setDescription('');
        setListId(0);
        setDueDate("2024-04-04");
        setSelectedTags(new Set([]));
    };

    useEffect(() => {
        if (!taskId) {
            resetTaskStates();

            Promise.all([
                GetListAccount(),
                GetTags()
            ])
                .then(([listResponse, tagResponse]) => {
                    setList1(listResponse.data);
                    setTags(tagResponse.data);

                    const fetchedTags = tagResponse.data;

                    if (listResponse.data && listResponse.data.length > 0) {
                        setListId(listResponse.data[0].id);
                    }
                    setSelectedTags(new Set());
                })
                .catch(error => {
                    console.error("Error fetching lists and tags:", error);
                });
        }
        else {
            Promise.all([
                fetchTaskById(taskId),
                GetListAccount(),
                GetTags()
            ])
                .then(([taskResponse, listResponse, tagResponse]) => {
                    const fetchedTask = taskResponse.data;
                    setTask(fetchedTask);
                    setList1(listResponse.data);
                    setTags(tagResponse.data);

                    if (fetchedTask?.name) {
                        setName(fetchedTask.name);
                    }
                    if (fetchedTask?.description) {
                        setDescription(fetchedTask.description);
                    }
                    if (fetchedTask?.listId) {
                        setListId(fetchedTask.listId);
                    }
                    if (fetchedTask?.dueDate) {
                        setDueDate(fetchedTask.dueDate.toString());
                    }
                    if (fetchedTask?.tags) {
                        setSelectedTags(new Set(fetchedTask.tags.map((tag: ITag) => String(tag.id))));
                    }
                    if (fetchedTask?.subTasks) {
                        setSubtasks(fetchedTask.subTasks);
                    }
                })
                .catch(error => {
                    console.error("Error fetching data:", error);
                });
        }
    }, [taskId]);


    const handleSaveButon = () => {
        const taskObject: TaskRequest = {
            taskId: taskId,
            name: name,
            description: description,
            listId: listId,
            dueDate: new Date(dueDate.toString()),
            selectedTags: Array.from(selectedTags).map(Number),
        };

        const handleResponse = (response: any) => {
            if (response.data.status) {
                addToast({
                    title: taskId ? "Update success" : "Create success",
                    color: "success",
                    description: response.data.message,
                    timeout: 3000,
                    shouldShowTimeoutProgress: true,
                });

                if (onTaskSaved) {
                    onTaskSaved(response.data.task);
                }
            } else {
                addToast({
                    title: taskId ? "Update failed" : "Create failed",
                    color: "danger",
                    description: response.data.message,
                    timeout: 3000,
                    shouldShowTimeoutProgress: true,
                });
            }
        };

        if (taskId) {
            fetchUpdateTask(taskObject)
                .then(handleResponse)
                .catch(error => {
                    console.error("Update task error:", error);
                });
        } else {
            fetchCreateTask(taskObject)
                .then(handleResponse)
                .catch(error => {
                    console.error("Create task error:", error);
                });
        }
    };

    const handleDelete = () => {
        fetchDeleteTask(taskId)
            .then(response => {
                if (response.data.status) {
                    addToast({
                        title: "Delete success",
                        color: "success",
                        description: response.data.message,
                        timeout: 3000,
                        shouldShowTimeoutProgress: true,
                    });

                    if (onTaskSaved) {
                        onTaskSaved(response.data.task, 'delete');
                    }

                    setOpen(false);
                } else {
                    addToast({
                        title: "Delete failed",
                        color: "danger",
                        description: response.data.message,
                        timeout: 3000,
                        shouldShowTimeoutProgress: true,
                    });
                }
            })
            .catch(error => {
                console.error("Delete task error:", error);
                addToast({
                    title: "Delete failed",
                    color: "danger",
                    description: "An unexpected error occurred",
                    timeout: 3000,
                    shouldShowTimeoutProgress: true,
                });
            });
    };

    const handleListChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setListId(Number(e.target.value));
    }

    return (
        <motion.div
            initial={false}
            animate={{
                x: isOpen ? 0 : 500,
                opacity: isOpen ? 1 : 0,
                display: isOpen ? "block" : "none"
            }}
            transition={{ duration: 0.2, ease: 'easeInOut' }}
            className="absolute right-0 backdrop-blur-md 
            bg-bg3/70 m-2 h-[98vh] flex flex-col rounded-xl p-5 shadow-lg w-[400px]
            "
        >

            <div className="flex justify-between items-center h-15 flex-none">
                <nav className="text-2xl font-bold pr-4">
                    {taskId ? 'Edit Task' : 'Add New Task'}
                </nav>

                <div>
                    <Button isIconOnly
                        color="warning"
                        onPress={() => setOpen(!isOpen)}
                    >
                        <ChevronRight />
                    </Button>
                </div>
            </div>

            <div className="my-5  h-[80vh]">
                <Input
                    color="primary"
                    type="text"
                    size="sm"
                    placeholder="Title"
                    variant="bordered"
                    value={name}
                    onValueChange={setName}
                />
                <div className="h-52">
                    <Tiptap
                        content={description}
                        onContentChange={(htmlContent) => setDescription(htmlContent)}
                    />
                </div>
                <div className="grid grid-cols-4 gap-4 w-full">
                    <div className="font-semibold col-span-1 flex items-center">List</div>
                    <div className="col-span-3">
                        <Select
                            color="primary"
                            className="max-w-xs"
                            variant="bordered"
                            aria-label="Select List"
                            selectedKeys={[`${listId}`]}
                            defaultSelectedKeys={[`${listId}`]}
                            placeholder="Select list"
                            onSelectionChange={(keys) => {
                                const selectedKey = Array.from(keys)[0] as string;
                                setListId(Number(selectedKey));
                            }}
                        >
                            {list1.map(listItem =>
                            (
                                <SelectItem key={listItem.id}>
                                    {listItem.name}
                                </SelectItem>
                            ))}
                        </Select>
                    </div>

                    <div className="font-semibold col-span-1 flex items-center">DueDate</div>
                    <div className="col-span-3">
                        <DatePicker
                            color="primary"
                            aria-label="date"
                            variant="bordered"
                            value={parseDate(dueDate)}
                            onChange={(value) => {
                                var da = `${value?.year}-${String(value?.month).padStart(2, '0')}-${String(value?.day).padStart(2, '0')}`;
                                setDueDate((da));
                            }}
                            showMonthAndYearPickers

                            CalendarTopContent={
                                <Button

                                    onPress={() => setDueDate(today(getLocalTimeZone()).toString())}
                                    className="m-2 px-3 pb-2 pt-3 bg-content1 [&>button]:text-default-500 [&>button]:border-default-200/60"
                                    variant="bordered"
                                    size="sm"
                                >
                                    Today
                                </Button>
                            }
                        />
                    </div>

                    <div className="font-semibold col-span-1 flex items-center">Tags</div>
                    <div className="col-span-3">
                        <Select
                            color="primary"
                            className="max-w-xs"
                            variant="bordered"
                            aria-label="Select tags"
                            selectedKeys={selectedTags}
                            selectionMode="multiple"
                            onSelectionChange={(keys) => {
                                setSelectedTags(new Set(Array.from(keys) as string[]));
                            }}
                        >
                            {tags.map(tagItem => (
                                <SelectItem key={String(tagItem.id)}>
                                    {tagItem.name}
                                </SelectItem>
                            ))}
                        </Select>

                    </div>
                </div>

                <SubtaskComponent subtasksImport={subTasks} taskId={taskId} />
            </div>

            <div className="w-full flex justify-center h-15 flex-none">
                {task && (
                    <Button className="mr-2 w-[40%]"
                        variant="bordered"
                        color="danger"
                        onPress={handleDelete}>

                        Delete Task
                    </Button>
                )}
                <Button className="mr-2 w-[40%]"
                    variant="bordered"
                    color="success"
                    onPress={handleSaveButon}>
                    {task ? 'Save Changes' : 'Create Task'}
                </Button>
            </div>
        </motion.div>
    );
}

export default TaskLayout;

