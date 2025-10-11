import { Button, Checkbox, Input, ScrollShadow } from "@heroui/react";
import { Plus, Trash2 } from "lucide-react";
import { useEffect, useState } from "react";
import { fetchAddSubTask, fetchDeleteSubTask, fetchUpdateSubTask } from "../../services/taskservice";

export const SubtaskComponent = ({ subtasksImport = [], taskId }) => {
    const [subtasks, setSubtasks] = useState([]);
    const [newSubtask, setNewSubtask] = useState({ id: 0, name: '' });


    useEffect(() => {
        setSubtasks(subtasksImport);
    }, [subtasksImport])

    const addSubtask = () => {
        // process add to backend 
        var request = {
            taskId: taskId,
            name: newSubtask.name,
            status: true
        }

        fetchAddSubTask(request).then(response => {
            console.log(response);
            if (newSubtask.name) {
                setSubtasks([
                    ...subtasks,
                    { id: response.data.subTask.id, name: response.data.subTask.name }
                ]);
                setNewSubtask({ id: 0, name: '' });
            }
        })

    };

    const deleteSubtask = (id) => {
        setSubtasks(subtasks.filter(subtask => subtask.id !== id));
        // process delete in backend 
        fetchDeleteSubTask(id).then(response => {
            console.log(response);
        });
    };

    const checkSubtask = (id) => {
        const subtaskToUpdate = subtasks.find(item => item.id === id);
        if (subtaskToUpdate) {
            const subtasks1 = subtasks.map(item => {
                if (item.id == id) {
                    return { ...item, status: !item.status };
                }
                return item;
            })
            setSubtasks(subtasks1);

            const updatedSubtask = {
                ...subtaskToUpdate,
                taskId: taskId,
                status: !subtaskToUpdate.status
            };

            fetchUpdateSubTask(updatedSubtask)
                .then(response => {
                    console.log(response);
                })
        }

    }

    return (
        <div className="w-full">
            <nav className="my-5 text-2xl font-bold pr-4">SubTask</nav>

            <div className="m-2 flex items-center cursor-pointer" onClick={() => document.getElementById('newSubtaskInput').focus()}>
                <Plus className="mr-2" />
                <Input
                    id="newSubtaskInput"
                    type="text"
                    value={newSubtask.name}
                    onChange={(e) => setNewSubtask({ ...newSubtask, name: e.target.value })}
                    onKeyPress={(e) => e.key === 'Enter' && addSubtask()}
                    placeholder="Add subtask"
                    className=""
                />
                {newSubtask && (
                    <Button
                        onPress={addSubtask}
                        className="ml-2 text-blue-500 hover:text-blue-700"
                    >
                        Add
                    </Button>
                )}
            </div>

            <ScrollShadow className="overflow-y-auto h-[20vh]  mt-2">
                {subtasks.map((subtask) => (
                    <div
                        key={subtask.id}
                        className="m-2 flex items-center justify-between hover:bg-fg1 hover:text-bg0 rounded p-1 group"
                    >
                        <div className="flex items-center">
                            <Checkbox
                                type="checkbox"
                                className="mr-2 form-checkbox text-blue-600"
                                isSelected={subtask.status}
                                onValueChange={() => checkSubtask(subtask.id)}
                            />
                            <span>{subtask.name}</span>
                        </div>
                        <Trash2
                            onClick={() => deleteSubtask(subtask.id)}
                            className="w-5 h-5 p-1 text-red-500 opacity-0 group-hover:opacity-100 cursor-pointer transition-opacity duration-200 ease-in-out m-1 hover:bg-red  rounded-sm"
                            size={16}
                        />
                    </div>
                ))}
            </ScrollShadow>


        </div>
    );
};

