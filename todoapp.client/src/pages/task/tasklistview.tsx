import { useNavigate, useParams } from "react-router-dom";
import { TaskService } from "../../services/taskservice";
import { useEffect, useState } from "react";
import TableTaskLayout from "../../components/layout/tabletasklayout";
import { ListItem, ListService } from "../../services/listservice";


// tra ve list task theo list 
export const TaskListView = () => {

    const { listId } = useParams();

    const navigate = useNavigate();

    const [tasks, setTasks] = useState([]);
    const [list, setList] = useState<ListItem>();

    useEffect(() => {
        Promise.all([
            TaskService.fetchDataTaskByList(Number(listId)),
            ListService.GetListById(Number(listId))
        ]).then(([taskResponse, listResponse]) => {
            setTasks(taskResponse.data.tasks);
            setList(listResponse.data.result)
        })

    }, [listId])


    return (
        <>
            <TableTaskLayout title={`List ${list?.name}`} tasksQuery={tasks} />
        </>
    );
}