import { useNavigate, useParams } from "react-router-dom";
import { fetchDataTask } from "../../services/taskservice";
import { useEffect, useState } from "react";
import TableTaskLayout from "../../components/layout/tabletasklayout";

const TaskTimeFilter = () => {
    const { type } = useParams();

    const navigate = useNavigate();

    const [tasks, setTasks] = useState([]);

    useEffect(() => {
        const fetch = async () => {
            fetchDataTask(type)
                .then(response => {
                    setTasks(response.data.tasks);
                    //console.log(response);
                }).catch(error => {
                    navigate("/")
                })
        }
        fetch();
    }, [type])

    return (
        <>
            <TableTaskLayout title={`Task ${type}`} tasksQuery={tasks} />
        </>
    );
}
export default TaskTimeFilter;