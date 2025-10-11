import { useNavigate, useParams } from "react-router-dom";
import DefaultLayout from "../../layouts/default"
import { fetchDataTask, fetchDataTaskByList } from "../../services/taskservice";
import { useEffect, useState } from "react";
import TableTaskLayout from "../../components/layout/tabletasklayout";
import { GetListById } from "../../services/listservice";


// tra ve list task theo list 
export const TaskListView = () => {

    const { listId } = useParams();

    const navigate = useNavigate();

    const [tasks, setTasks] = useState([]);
    const [list, setList] = useState([]);

    useEffect(() => {
        Promise.all([
            fetchDataTaskByList(listId),
            GetListById(listId)
        ]).then(([taskResponse, listResponse]) => {
            setTasks(taskResponse.data.tasks);
            setList(listResponse.data.result)
        })

    }, [listId])


    return (
        <>
            <TableTaskLayout title={`List ${list.name}`} tasksQuery={tasks} />
        </>
    );
}