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
import { GetListAccount, GetNumOfTaskInfo, ListItem } from "../../services/listservice";
import { IUser } from "../../types/User";
import { ITag } from "../../types/Tag";
import { Input, Listbox, ListboxItem } from "@heroui/react";
import { LogOut, Plus, Search, Settings2 } from "lucide-react";

interface SideBarProps {
    isOpen: boolean;
}

interface TaskCounts {
    [key: string]: number;
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
    const user: IUser = JSON.parse(getUser() || '{}');
    return await axios.get(API_URL + `/tags/user/${user.id}`,
        {
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${getToken()}`
            }
            ,
        }
    );
}


export const SideBar = ({ isOpen }: SideBarProps) => {
    const [lists, setLists] = useState<ListItem[]>([]);
    const [tags, setTags] = useState<ITag[]>([]);
    const navigate = useNavigate();
    useEffect(() => {
        const fetchData = async () => {
            const data = await GetListAccount();
            setLists(data.data || []);

            const tagsResponse = await GetTags();
            setTags(tagsResponse.data || []);
        }
        fetchData();
    }, [])


    const [taskCounts, setTaskCounts] = useState<TaskCounts>({});

    useEffect(() => {
        const fetchCounts = async () => {
            const tasks = [
                { label: "UpComing", key: "/task/future", timestamp: "future" },
                { label: "Today", key: "/task/today", timestamp: "today" },
                { label: "Lated", key: "/task/past", color: "danger", timestamp: "past" },
                { label: "Sticky Note", key: "/sticky-notes", timestamp: "t" }
            ];

            const counts = await Promise.all(
                tasks.map(async (task) => {
                    try {
                        const response = await GetNumOfTaskInfo(task.timestamp, 0);

                        const count = response.data ?? 0;
                        return { key: task.key, count };
                    } catch (error) {
                        console.error(`Error fetching count for ${task.label}:`, error);
                        return { key: task.key, count: 0 };
                    }
                })
            );

            setTaskCounts(Object.fromEntries(counts.map(({ key, count }) => [key, count])));
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
                        {[
                            { label: "UpComing", key: "/task/future" },
                            { label: "Today", key: "/task/today" },
                            { label: "Lated", key: "/task/past", color: "danger" },
                            { label: "Sticky Note", key: "/sticky-notes" }
                        ].map((item) => ListboxItemView(item.label, item.key, item.color, taskCounts[item.key] || 0))}
                    </Listbox>
                </ListboxWrapper>
            </div>

            <div id="list" className="flex-auto">
                <ListboxWrapper header='Lists'>
                    <Listbox aria-label="Actions" onAction={(key) => navigate(key as string)}>

                        <>
                            {lists.slice(0, 4).map((item) => (
                                <React.Fragment key={`/task/list/${item.id}`}>
                                    {ListboxItemView(item.name, `/task/list/${item.id}`, undefined, item.tasks.length)}
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

