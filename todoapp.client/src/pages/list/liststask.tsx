import { useEffect, useState } from "react";
import DefaultLayout from "../../layouts/default"
import {
    Button, Dropdown, DropdownItem, DropdownMenu, DropdownTrigger,
    Modal,
    ModalContent,
    ModalHeader,
    ModalBody,
    ModalFooter,
    useDisclosure,
    addToast,
} from "@heroui/react";
import { Check, Cross, Save, X } from "lucide-react";
import { ListService, ListItem } from "../../services/listservice";
import { ITask } from "../../types/Task";

export const ListTaskView = () => {
    const { isOpen, onOpen, onClose } = useDisclosure();
    const [isAdd, setIsAdd] = useState(false);
    const [list, setList] = useState<ListItem[]>([]);
    const [selectedItem, setSelectedItem] = useState<ListItem>();
    useEffect(() => {
        ListService.GetLists()
            .then(response => {
                console.log(response.data);
                setList(response.data || []);
            })
            .catch(error => {
                console.error("Error fetching lists:", error);
            });
    }, [])

    const handleOpen = (item: ListItem, isAddStatus: boolean) => {
        setSelectedItem(item);
        setIsAdd(isAddStatus);
        //console.log(selectedItem);
        onOpen();
    };
    const checkedDate = (time: Date) => {
        const now = new Date();

        if (new Date(time) < now) {
            return "past";
        } else if (new Date(time) > now) {
            return "future";
        }
        return "today";
    };
    const formatDate = (date: Date) => {
        return new Date(date).toLocaleDateString("en-US", {
            weekday: "short", // "ddd" -> "Mon", "Tue", etc.
            month: "short",   // "MMM" -> "Jan", "Feb", etc.
            day: "2-digit",   // "dd" -> "01", "02", etc.
            year: "numeric",  // "yyyy" -> "2025"
        });
    };
    const UpdateList = () => {
        //console.log(selectedItem);
        ListService.UpdateList(selectedItem as ListItem)
            .then(response => {
                if (response.data.status) {
                    //console.log(response);
                    addToast({ title: response.data.message, color: "success" });

                    var updateList = list.map((element: ListItem) => {
                        if ((element as ListItem).id == response.data.result.id) {
                            return {
                                ...element as ListItem,
                                id: response.data.result.id,
                                name: response.data.result.name
                            }
                        }
                        return element;
                    })
                    setList(updateList);
                } else {
                    addToast({ title: response.data.message, color: "danger" });
                }
            })
            .catch();
        onClose();

    }
    const CreateList = () => {
        console.log(selectedItem);
        ListService.CreateList(selectedItem as ListItem)
            .then(response => {
                if (response.data.status) {
                    addToast({ title: response.data.message, color: "success" });
                    setList(preList => [...preList, response.data.result]);
                } else {
                    addToast({ title: response.data.message, color: "danger" });
                }
            })
            .catch();
        onClose();
    }
    return (
        <DefaultLayout>
            <div className="border-[0.5px] h-[96%] rounded-2xl w-full p-5  gap-1">
                <p className="font-bold text-5xl pb-4 text-neutral-700">List view</p>
                <div className="w-full flex justify-end my-2 px-2">
                    <Dropdown >
                        <DropdownTrigger>
                            <Button variant="bordered">Open Menu</Button>
                        </DropdownTrigger>
                        <DropdownMenu aria-label="Static Actions">
                            <DropdownItem key="new" onPress={() => handleOpen(selectedItem as ListItem, true)}>Add</DropdownItem>
                            <DropdownItem key="copy">View Task</DropdownItem>
                        </DropdownMenu>
                    </Dropdown>
                </div>
                <div className="relative overflow-x-auto shadow-md sm:rounded-lg ">



                    <table className="w-full text-sm text-left rtl:text-right  ">
                        <thead className="text-xs text-fg1 uppercase bg-bg4 dark:bg-bg4">
                            <tr>
                                <th scope="col" className="px-6 py-3">
                                    No
                                </th>
                                <th scope="col" className="px-6 py-3">
                                    ListName
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {list.map(item => (
                                <tr className=" border-b hover:bg-bg4 dark:hover:bg-bg4" key={item.id}
                                    onClick={() => handleOpen(item, false)}>
                                    <th scope="row" className="px-6 py-4 font-medium text-fg0 whitespace-nowrap ">
                                        #{item.id}
                                    </th>
                                    <td className="px-6 py-4">
                                        {item.name}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
                <Modal isOpen={isOpen} size="lg" onClose={onClose}>
                    <ModalContent>

                        {(onClose) => (
                            <>
                                <ModalHeader className="flex flex-col gap-1">
                                    <input type="hidden" value={selectedItem?.id ?? ''} />
                                    <input type="text" className="font-bold w-full" value={selectedItem?.name ?? ''} placeholder="Title"
                                        onChange={(e) => setSelectedItem({ ...(selectedItem || {}), name: e.target.value } as ListItem)}
                                    />
                                </ModalHeader>

                                <ModalBody>

                                    { selectedItem?.tasks?.map((taskItem: ITask) => (
                                        <div className="flex gap-2 hover:bg-bg4 p-4 rounded-lg"
                                            key={taskItem.id}
                                        >
                                            <div className="col-span-1 inline-flex items-center ">
                                                <label className="relative flex items-center p-2 rounded-full cursor-pointer" htmlFor="customStyle">
                                                    <input type="checkbox"
                                                        className="before:content[''] peer relative h-5 w-5 cursor-pointer appearance-none rounded-full border border-bg0 bg-bg0 transition-all before:absolute before:top-2/4 before:left-2/4 before:block before:h-12 before:w-12 before:-translate-y-2/4 before:-translate-x-2/4 before:rounded-full before:bg-blue-gray-500 before:opacity-0 before:transition-opacity checked:border-amber-800 checked:bg-amber-800 checked:before:bg-amber-800 hover:scale-105 hover:before:opacity-0"
                                                        id="customStyle"
                                                    // onClick="checkBoxHandle(@t.Id)"
                                                    //   @((t.Status == true) ? "checked" : "") 
                                                    />
                                                    <span className="absolute transition-opacity opacity-0 pointer-events-none top-2/4 left-2/4 -translate-y-2/4 -translate-x-2/4 peer-checked:opacity-100">
                                                        <Check size={14} />
                                                    </span>
                                                </label>
                                            </div>

                                            <div className="col-span-3  w-full">
                                                <div className="font-normal w-full"><a href="/task/view?id=@t.Id">{taskItem.name}</a></div>
                                                {/* @{
                  DateTime? nullableDateTime = t.DueDate;
                  DateTime show = nullableDateTime ?? DateTime.Now;
              } */}
                                                <div className="text-amber-800 text-sm">{checkedDate(taskItem.dueDate)} •
                                                    <i className="uil uil-schedule "></i> {formatDate(taskItem.dueDate)} </div>
                                            </div>
                                        </div>
                                    ))}

                                </ModalBody>

                                <ModalFooter>
                                    <Button color="danger" variant="light" onPress={onClose} isIconOnly>
                                        <X />
                                    </Button>
                                    <Button color="primary" variant="light" onPress={isAdd ? (CreateList) : (UpdateList)} isIconOnly>
                                        <Check />
                                    </Button>
                                </ModalFooter>
                            </>
                        )}

                    </ModalContent>
                </Modal>
            </div>
        </DefaultLayout >

    );
}