import React, { useEffect, useState } from "react";
import { MotionConfig, animateValue, color, motion } from "framer-motion";
import { Navbar } from "../ui/navbar";
import { GetTags, SideBar } from "../ui/sizebar";
import { Button } from "@heroui/button";
import { ChevronRight, Cross, Menu, Plus, Save, Trash, Trash2 } from "lucide-react";
import { addToast, Checkbox, DatePicker, Input, ScrollShadow, Select, SelectItem } from "@heroui/react";
import Tiptaptext from "../richtextbox/tiptaptext";
import Tiptap from "../richtextbox/Tiptap";
import { GetListAccount } from "../../services/listservice";
import { fetchCreateTask, fetchDeleteTask, fetchTaskById, fetchUpdateSn, fetchUpdateTask } from "../../services/taskservice";
import { getLocalTimeZone, parseDate, today } from "@internationalized/date";
import { select } from "@heroui/theme";
import { SubtaskComponent } from "./subtaskcomponent";



function TaskLayout({ isOpen, setOpen, taskId = 0, onTaskSaved }) {
    // name
    const [name, setName] = useState('');
    // task infor
    const [task, setTask] = useState(null);
    // descrption input
    const [description, setDescription] = useState('');

    //  listid for selection & list1 to view list in select tag
    const [listId, setListId] = useState(0);
    // list to view
    const [list1, setList1] = useState([]);

    // due date to view and save 
    const [dueDate, setDueDate] = useState(("2024-04-04"));

    // listTag to view
    const [tags, setTags] = useState([]);
    // infor to save to tag
    const [selectedTags, setSelectedTags] = useState(new Set([]));


    const [subTasks, setSubtasks] = useState([]);

    // Reset all states 
    const resetTaskStates = () => {
        setName('');
        setTask(null);
        setDescription('');
        setListId(0);
        setDueDate(("2024-04-04"));
        setSelectedTags(new Set([]));
    };

    useEffect(() => {
        if (!taskId) {
            resetTaskStates();

            // Fetch lists and tags for new task creation
            Promise.all([
                GetListAccount(),
                GetTags()
            ])
                .then(([listResponse, tagResponse]) => {
                    setList1(listResponse.data);
                    setTags(tagResponse.data);


                    const fetchedTags = tagResponse.data;

                    if (listResponse.data && listResponse.data.length > 0) {
                        // setListId();
                        console.log(listResponse.data[0].id);
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
                    // Set listId after both task and lists are loaded
                    if (fetchedTask?.listId) {
                        setListId(fetchedTask.listId);
                    }
                    // set duedate 
                    if (fetchedTask?.dueDate) {
                        setDueDate(fetchedTask.dueDate);
                    }
                    // set tags 
                    if (fetchedTask?.tags) {
                        setSelectedTags(new Set(fetchedTask.tags.map(tag => String(tag.id))));
                    }
                    // setsubtasks
                    if (fetchedTask?.subTasks) {
                        setSubtasks(fetchedTask.subTasks);
                    }


                    // if (fetchedTask?.tags) {
                    // setSelectedTags(fetchedTask.tags);
                    // }
                })
                .catch(error => {
                    console.error("Error fetching data:", error);
                });
        }
    }, [taskId]);


    const handleSaveButon = () => {
        const taskObject = {
            taskId: taskId,
            name: name,
            description: description,
            listId: listId,
            dueDate: dueDate,
            selectedTags: Array.from(selectedTags).map(Number),
        };

        const handleResponse = (response) => {
            if (response.data.status) {
                addToast({
                    title: taskId ? "Update success" : "Create success",
                    color: "success",
                    description: response.data.message,
                    timeout: 3000,
                    shouldShowTimeoutProgress: true,
                });

                // Call the callback with the new/updated task
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

                    // Call the callback with the deleted task and 'delete' action
                    if (onTaskSaved) {
                        onTaskSaved(response.data.task, 'delete');
                    }

                    // Optionally close the task layout
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

    // useEffect(() => {
    //     console.log("Current listId:", dueDate);
    // }, [dueDate]);

    const handleListChange = (e) => {
        setListId(e.target.value);

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
                {/* title */}
                <nav className="text-2xl font-bold pr-4">
                    {taskId ? 'Edit Task' : 'Add New Task'}
                </nav>


                {/* button set */}
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
                            onChange={handleListChange}
                        >
                            {list1.map(listItem =>
                            (
                                <SelectItem key={listItem.id} value={listItem.id}>
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
                                var da = `${value.year}-${String(value.month).padStart(2, '0')}-${String(value.day).padStart(2, '0')}`;
                                console.log("da: ", da);
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
                                console.log(keys);
                                setSelectedTags(keys);
                            }}
                        >
                            {tags.map(tagItem => (
                                <SelectItem key={String(tagItem.id)} value={String(tagItem.id)}>
                                    {tagItem.name}
                                </SelectItem>
                            ))}
                        </Select>

                    </div>
                </div>

                {/* subtask */}
                <SubtaskComponent subtasksImport={subTasks} taskId={taskId} />
                {/* 
                <nav className="my-5 text-2xl font-bold pr-4">SubTask</nav>
                <div className="m-2 flex"><Plus /> add subtask</div>
                <div className="overflow-y-scroll h-[20vh]">
                    <div className="m-2 flex"> iasdfadf asdfa</div>
                    <div className="m-2 flex"> iasdfadf asdfa</div>
                    <div className="m-2 flex"> iasdfadf asdfa</div>
                    <div className="m-2 flex"> iasdfadf asdfa</div>
                    <div className="m-2 flex"> iasdfadf asdfa</div>
                    <div className="m-2 flex"> iasdfadf asdfa</div>
                </div> */}
            </div>

            <div className="w-full flex justify-center h-15 flex-none">
                {task && (
                    <Button className="mr-2 w-[40%]"
                        variant="bordered"
                        color="danger"
                        onPress={handleDelete}>

                        Delete Task
                        {/* <Trash /> */}
                    </Button>
                )}
                <Button className="mr-2 w-[40%]"
                    variant="bordered"
                    color="success"
                    onPress={handleSaveButon}>
                    {task ? 'Save Changes' : 'Create Task'}
                    {/* <Save /> */}
                </Button>
            </div>

            {/* <div className="flex items-center justify-center h-full text-gray-500">
                    Please select a task to view details
                </div> */}
        </motion.div>
    );
}

export default TaskLayout;