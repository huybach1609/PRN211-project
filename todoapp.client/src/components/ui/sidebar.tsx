import { motion } from "framer-motion";
import { useEffect, useState } from "react";
import React from "react";
import { MoonFilledIcon, SunFilledIcon } from "./icons";
import { useTheme } from "@heroui/use-theme";
import { LogOut as LogOutAuth } from "../../pages/auth/auth";
import { useNavigate } from "react-router-dom";
import { ListboxItemView, ListboxWrapper } from "../../components/sidebar/listboxcomponent";
import { getToken, getUser } from "../../utils/tokenManage";
import axios from "axios";
import { API_URL } from "../../constrains";
import { ListService, ListItem } from "../../services/listservice";
import { IUser } from "../../types/User";
import { ITag } from "../../types/Tag";
import { Input, Listbox, ListboxItem } from "@heroui/react";
import { LogOut, Plus, Search, Settings2 } from "lucide-react";
import { TaskService } from "../../services/taskservice";
import { TaskCountsDto } from "../../types/Task";
import { ListResponseDto } from "../../types/list";
import { TagService } from "../../services/tagservice";

interface SideBarProps {
    isOpen: boolean;
}

export const UseSelectionView = () => {
    const { theme, setTheme } = useTheme();
    const user: IUser = JSON.parse(getUser() || '{}');
    const navigate = useNavigate();
    return (
        <Listbox
            aria-label="Listbox menu with descriptions"
        >
            <ListboxItem
                startContent={
                    theme === 'light' ? <SunFilledIcon size={20} /> : <MoonFilledIcon size={20} />
                }
                aria-label="Listbox menu with descriptions"
                onPress={() => setTheme(theme === "light" ? "dark" : "light")}
                color="warning"
            >
                Theme: {theme}
            </ListboxItem>
            <ListboxItem
                onPress={() => navigate("/user/" + user.userName + "/info")}
                color="warning"
                aria-label="setting field"
                startContent={<Settings2 size={20} strokeWidth={2.5} />}>Setting</ListboxItem>

            <ListboxItem
                color="warning"
                aria-label="log out field"
                startContent={<LogOut size={20} strokeWidth={2.5}
                />
                }
                onPress={() => LogOutAuth()}
            >Log out</ListboxItem>
        </Listbox>
    );
}
const HeadingBar = () => {
    return (
        <>
            <h2 className={`text-xl font-bold mb-4 `}>Sidebar</h2>
            <Input type="text"
                placeholder="Type to search..."
                radius="lg"
                startContent={<Search className="text-xs" />}
            ></Input>
        </>
    );
}

export const GetTags = async () => {
    const response = await TagService.GetTags();
    return response.data;
}


export const SideBar = ({ isOpen }: SideBarProps) => {
    const [lists, setLists] = useState<ListResponseDto[]>([]);
    const [tags, setTags] = useState<ITag[]>([]);
    const navigate = useNavigate();
    useEffect(() => {
        const fetchData = async () => {
            const response = await ListService.GetLists();
            console.log("lists", response.data);
            setLists(response.data || []);

            const tagsResponse = await GetTags();
            setTags(tagsResponse.data || []);
        }
        fetchData();
    }, [])


    const [tasks, setTasks] = useState<Object[]>([
        { label: "UpComing", key: "/task/future", timestamp: "upcoming", count: 0 },
        { label: "Today", key: "/task/today", timestamp: "today", count: 0 },
        { label: "Lated", key: "/task/past", color: "danger", timestamp: "overdue", count: 0 },
        { label: "Sticky Note", key: "/sticky-notes", timestamp: "total", count: 0 }
    ]);

    useEffect(() => {
        const fetchCounts = async () => {
            try {
                const response: { data: TaskCountsDto } = await TaskService.fetchTaskCount();
                console.log(response.data);
                setTasks((prevTasks) => 
                    prevTasks.map((task: any) => ({ 
                        ...task, 
                        count: response.data[task.timestamp as keyof TaskCountsDto] || 0 
                    }))
                );
            } catch (error) {
                console.error("Error fetching task counts:", error);
            }
        };
        fetchCounts();
    }, []);


    return (
        <motion.div
            initial={false}
            animate={{ width: isOpen ? 300 : 0, opacity: isOpen ? 100 : 0 }}
            transition={{ duration: 0.3, ease: 'easeInOut' }}
            style={{ display: isOpen ? "block" : "none" }}
            className="bg-bg3  m-2 h-[98vh] flex flex-col rounded-xl p-5 shadow-lg "
        >
            <HeadingBar />
            <div id="task" className=" flex-auto">
                <ListboxWrapper header='Tasks'>
                    <Listbox aria-label="Actions" onAction={(key) => navigate(key as string)}>
                        {tasks.map((item: any) => ListboxItemView(item.label, item.key, item.color, item.count))}
                    </Listbox>
                </ListboxWrapper>
            </div>

            <div id="list" className="flex-auto">
                <ListboxWrapper header='Lists'>
                    <Listbox aria-label="Actions" onAction={(key) => navigate(key as string)}>

                        <>
                            {lists.slice(0, 4).map((item: ListResponseDto) => (
                                <React.Fragment key={`/task/list/${item.result.id}`}>
                                    {ListboxItemView(item.result.name, `/task/list/${item.result.id}`, undefined, item.numberOfTaskInfo)}
                                </React.Fragment>
                            ))}
                        </>

                        <ListboxItem className="text-sm" key="/lists"
                            startContent={<Plus size={20} strokeWidth={1.5} />}
                        >
                            Add new list
                        </ListboxItem>
                    </Listbox>
                </ListboxWrapper>
            </div>
            <div id="tag" className="flex-auto ">
                {tags.map((item) => (
                    <span key={item.id} className="m-1 inline-flex items-center rounded-md bg-red-50 px-2 py-1 text-xs font-medium text-red-700 ring-1 ring-red-600/10 ring-inset">{item.name}</span>
                ))}
            </div>
            <UseSelectionView />
        </motion.div>
    );
};

