import { useNavigate, useParams } from "react-router-dom"
import DefaultLayout from "../../layouts/default"
import { useEffect, useState } from "react";
import { fetchDataTask, fetchSetStatus } from "../../services/taskservice";
import { ArrowRightToLine, BadgeInfo, Calendar1, Check, ChevronLeft, Plus, SquareArrowDownRightIcon } from "lucide-react";
import { color } from "framer-motion";
import { Button } from "@heroui/button";
import TaskLayout from "../../components/layout/tasklayout";


// tra ve task future, today, past
export const TaskView = () => {
    const { type } = useParams();
    const navigate = useNavigate();

    const [tasks, setTasks] = useState([]);
    const [taskIdSelected, setTaskIdSelect] = useState(0);

    useEffect(() => {
        const fetch = async () => {
            fetchDataTask(type)
                .then(response => {
                    setTasks(response.data.tasks);
                    console.log(response);
                }).catch(error => {
                    navigate("/")
                })
        }
        fetch();
    }, [type])

    const getColor = (date) => {
        const today = new Date().setHours(0, 0, 0, 0); // Normalize today’s date
        const givenDate = new Date(date).setHours(0, 0, 0, 0); // Normalize input date

        if (givenDate > today) {
            return "#0096c7"; // Future date
        } else if (givenDate < today) {
            return "#ee6055"; // Past date
        } else {
            return "#57cc99"; // Today
        }
    };

    const [isOpen, setOpen] = useState(false);

    const handleRightBar = (task) => {
        console.log(task);
        setOpen(true);
        setTaskIdSelect(task.id);
    }

    const checkBoxHandle = (taskId, status) => {
        fetchSetStatus(taskId, status)
            .then(response => {
                console.log(response)
            })
            .catch();
    }

    const handleTaskCreatedOrUpdated = (newTask, action = 'update') => {
        if (action === 'delete') {
            // Remove the task 
            setTasks(currentTasks =>
                currentTasks.filter(t => t.id !== newTask.id)
            );
        } else {
            const existingTaskIndex = tasks.findIndex(t => t.id === newTask.id);
            if (existingTaskIndex !== -1) {
                // Update existing task
                const updateTasks = [...tasks];
                updateTasks[existingTaskIndex] = newTask;
                setTasks(updateTasks);
            } else {
                // Add new task
                setTasks([...tasks, newTask]);
            }
        }
    };
    return (
        <DefaultLayout rightBar={<TaskLayout isOpen={isOpen} setOpen={setOpen} taskId={taskIdSelected} onTaskSaved={handleTaskCreatedOrUpdated} />}>
            <div className="border-[2px] border-bg5 h-[96%] rounded-2xl w-full p-5  gap-1 flex flex-col mr-2">
                <p className="font-bold text-5xl pb-4 text-neutral-700">Task {type}</p>

                {/* add task  */}
                <div className="border-[2px] border-bg4 rounded-xl p-5 flex gap-5 m-4"
                    onClick={() => { setOpen(true); setTaskIdSelect(0) }}
                >
                    <Plus /><div>Add new task</div>
                </div>

                {tasks.map(item => {
                    return (
                        <div key={item.id} className="border-b-[2px] border-bg4 p-5 flex items-center">
                            <Button isIconOnly variant="light" className="text-fg0" color="primary" onPress={() => handleRightBar(item)}>
                                <ChevronLeft size={35} />
                            </Button>
                            <div>
                                {/* line 1 */}
                                <div className="flex items-center gap-3">
                                    <div className=" col-span-1 flex items-center ">
                                        <label className=" relative flex items-center p-2 rounded-full cursor-pointer" htmlFor="customStyle">
                                            <input type="checkbox"
                                                className="bg-bg5 before:content[''] peer relative h-5 w-5 cursor-pointer appearance-none rounded-full border border-gray-900/20 bg-gray-900/10 transition-all before:absolute before:top-2/4 before:left-2/4 before:block before:h-12 before:w-12 before:-translate-y-2/4 before:-translate-x-2/4 before:rounded-full before:bg-blue-gray-500 before:opacity-0 before:transition-opacity checked:border-amber-800 checked:bg-amber-800 checked:before:bg-amber-800 hover:scale-105 hover:before:opacity-0"
                                                id="customStyle"

                                                onClick={() => checkBoxHandle(item.id, !item.status)}
                                                defaultChecked={item.status}
                                            />
                                            <span className="absolute text-white transition-opacity opacity-10 pointer-events-none top-2/4 left-2/4 -translate-y-2/4 -translate-x-2/4 peer-checked:opacity-100">
                                                <Check size={16} />
                                            </span>
                                        </label>
                                    </div>
                                    <div>{item.name}</div>


                                </div>
                                {/* line 2 */}
                                <div className="flex items-center gap-5 font-bold">
                                    {/* Due Date */}
                                    <div className="flex items-center gap-2  text-fg1 "
                                        style={{ color: getColor(item.dueDate) }}>
                                        <Calendar1 size={15} />
                                        {item.dueDate}
                                    </div>

                                    {/* Separator */}
                                    <div className="h-5 w-px bg-bg5"></div>

                                    {/* Subtasks */}
                                    <div className=" text-sm">
                                        Subtasks
                                    </div>

                                    {/* Separator */}
                                    <div className="h-5 w-px bg-bg5"></div>

                                    {/* Tags */}
                                    <div className="flex gap-2">
                                        {item.tags.map(tag => (
                                            <span key={tag.id} className="inline-flex items-center rounded-md bg-red-50 px-2 py-1 text-xs font-medium text-red-700 ring-1 ring-red-600/10 ring-inset">
                                                {tag.name}
                                            </span>
                                        ))}
                                    </div>
                                </div>
                            </div>
                        </div>
                    )
                })}
            </div>

        </DefaultLayout >
    )
}