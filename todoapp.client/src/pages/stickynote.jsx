import DefaultLayout from "../layouts/default"
import { Card, CardHeader, CardBody, ModalContent, ModalHeader, ModalBody, ModalFooter, Button, useDisclosure, Modal, Input, Form, addToast } from '@heroui/react';
import { useEffect, useState } from "react";
import { Plus, Save, Trash2 } from "lucide-react";
import { fetchCreateSn, fetchDataSn, fetchDeleteSn, fetchUpdateSn } from "../services/taskservice";



export const StickyNote = () => {

    const [st, setSt] = useState([]);
    const [selectedItem, setSelectedItem] = useState({ id: 0, name: '', details: '' });
    const { isOpen, onOpen, onClose } = useDisclosure();
    const [size, setSize] = useState("md");

    useEffect(() => {
        const fetchSt = async () => {
            try {
                const data = await fetchDataSn();
                //console.log(data);
                setSt(data.data)
            } catch (error) {
                console.error("Failed to load account info:", error);
            }
        };
        fetchSt();
    }, [])

    const handleCardClick = (item = { id: 0, name: '', details: '' }) => {
        setSelectedItem(item);
        setSize("md");
        onOpen();
    };

    const DeleteSn = async () => {
        fetchDeleteSn(selectedItem.id)
            .then(response => {
                if (response.data.status) {
                    addToast({ title: response.data.message, color: "success" });
                    setSt(prevSt => prevSt.filter(item => item.id !== response.data.obj.id));
                }
            })
            .catch();

    }
    const CreateSn = async () => {
        fetchCreateSn(selectedItem)
            .then(response => {
                //console.log(response);
                setSt(preSt => [...preSt, response.data.obj])

            })
            .catch();
    }

    const UpdateSn = async () => {
        fetchUpdateSn(selectedItem).then((response) => {
            //console.log(response)
            if (response.data.status) {
                addToast({ title: response.data.message, color: "success" });
                var updatest = st.map(element => {
                    if (element.id == selectedItem.id) {
                        return {
                            ...element,
                            name: selectedItem.name,
                            details: selectedItem.details
                        }
                    }
                    return element;
                })
                setSt(updatest);
            }

        }).catch((err) => {
            //console.log(err);
        });
    };

    return (
        <DefaultLayout>
            <div className="border-[2px] h-[96%] rounded-2xl border-bg5 mr-2 w-full p-5 grid grid-cols-2 grid-rows-2 md:grid-rows-3 md:grid-cols-3   gap-5">
                {st.map(item => (
                    <Card className="py-4 bg-bg0" key={item.id}
                        onPress={() => handleCardClick(item)} isPressable>

                        <CardHeader className="pb-0 pt-2 px-4 flex-col items-start">
                            <p className="text-tiny uppercase font-bold">{item.name}</p>
                            {/* <small className="text-default-500">{item.name}</small> */}
                        </CardHeader>
                        <CardBody className="overflow-visible py-2">
                            <small className="">{item.details}</small>
                        </CardBody>
                    </Card>
                ))}
                <Button className="py-4 bg-bg0 w-full h-full shadow text-default-800"
                    isPressable onPress={() => handleCardClick()}>
                    <Plus size={40} />
                </Button>
            </div>
            {/* Modal that appears when a card is clicked */}
            <Modal isOpen={isOpen} size={size} onClose={onClose}>
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalHeader className="flex flex-col gap-1 w-full">
                                <input type="hidden" value={selectedItem.id} />
                                <input type="text" className="font-bold w-full" value={selectedItem.name} placeholder="Title" onChange={(e) => setSelectedItem({ ...selectedItem, name: e.target.value })} />
                            </ModalHeader>
                            <ModalBody>
                                <textarea className="w-full px-5 h-fit bg-transparent border-none shadow-transparent outline-none overflow-auto" rows="5" value={selectedItem.details} onChange={(e) => setSelectedItem({ ...selectedItem, details: e.target.value })}></textarea>
                            </ModalBody>
                            <ModalFooter>
                                <Button color="danger" isIconOnly
                                    variant="light"
                                    onPress={() => {
                                        DeleteSn();
                                        onClose();
                                    }}
                                ><Trash2 /></Button>
                                {/* <Button color="danger" variant="light" onPress={onClose}>Close</Button> */}
                                <Button color="success" variant="flat" onPress={() => {
                                    if (selectedItem.id == 0) {
                                        CreateSn();
                                    } else {
                                        UpdateSn();
                                    }
                                    onClose();
                                }} isIconOnly><Save /></Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>
        </DefaultLayout>
    )
}