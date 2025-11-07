import DefaultLayout from "../layouts/default";
import {
  Card,
  CardHeader,
  CardBody,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  useDisclosure,
  Modal,
  addToast,
} from "@heroui/react";
import { useEffect, useState } from "react";
import { Plus, Save, Trash2 } from "lucide-react";
import {
  StickyNoteService
} from "../services/taskservice";
import { IStickyNote } from "../types/StickyNote";


export const StickyNote = () => {
  const [st, setSt] = useState<IStickyNote[]>([]);
  const [selectedItem, setSelectedItem] = useState<IStickyNote>();

  const { isOpen, onOpen, onClose } = useDisclosure();
  const [size, setSize] = useState<string>("md");

  useEffect(() => {
    const fetchSt = async () => {
      try {
        const data = await StickyNoteService.fetchDataSn();
        // Controller returns Ok(notes) which is directly an array
        setSt(Array.isArray(data.data) ? data.data : []);
      } catch (error) {
        console.error("Failed to load sticky notes:", error);
        setSt([]); // Set to empty array on error
      }
    };
    fetchSt();
  }, []);

  const handleCardClick = (item?: IStickyNote) => {
    // If no item provided, initialize with empty object for new note
    setSelectedItem(item || { id: 0, userId: 0, name: "", details: "" });
    setSize("md");
    onOpen();
  };

  const DeleteSn = async () => {
    if (!selectedItem?.id) return;
    
    StickyNoteService.fetchDeleteSn(selectedItem.id)
      .then((response) => {
        // Controller returns NoContent (204) on success
        if (response.status === 204) {
          addToast({ title: "Sticky note deleted successfully", color: "success" });
          setSt((prevSt) =>
            prevSt.filter((item) => item.id !== selectedItem.id)
          );
        }
      })
      .catch((error) => {
        console.error("Failed to delete sticky note:", error);
        addToast({ 
          title: error.response?.data?.message || "Failed to delete sticky note", 
          color: "danger" 
        });
      });
  };

  const CreateSn = async () => {
    if (!selectedItem?.name) {
      addToast({ title: "Name is required", color: "danger" });
      return;
    }

    StickyNoteService.fetchCreateSn(selectedItem as IStickyNote)
      .then((response) => {
        // Controller returns CreatedAtAction with the created note
        if (response.status === 201 && response.data) {
          addToast({ title: "Sticky note created successfully", color: "success" });
          setSt((preSt) => [...preSt, response.data]);
        }
      })
      .catch((error) => {
        console.error("Failed to create sticky note:", error);
        const errorMessage = error.response?.data?.message || 
                            error.response?.data?.errors?.Name?.[0] ||
                            "Failed to create sticky note";
        addToast({ title: errorMessage, color: "danger" });
      });
  };

  const UpdateSn = async () => {
    if (!selectedItem?.id || !selectedItem?.name) {
      addToast({ title: "Name is required", color: "danger" });
      return;
    }

    StickyNoteService.fetchUpdateSn(selectedItem as IStickyNote)
      .then((response) => {
        // Controller returns Ok(note) with the updated note
        if (response.status === 200 && response.data) {
          addToast({ title: "Sticky note updated successfully", color: "success" });
          setSt((prevSt) =>
            prevSt.map((element) =>
              element.id === selectedItem.id ? response.data : element
            )
          );
        }
      })
      .catch((error) => {
        console.error("Failed to update sticky note:", error);
        const errorMessage = error.response?.data?.message || 
                            error.response?.data?.errors?.Name?.[0] ||
                            "Failed to update sticky note";
        addToast({ title: errorMessage, color: "danger" });
      });
  };

  return (
    <DefaultLayout>
      <div className="border-[2px] h-[96%] rounded-2xl border-bg5 mr-2 w-full p-5 grid grid-cols-2 grid-rows-2 md:grid-rows-3 md:grid-cols-3   gap-5">
        {Array.isArray(st) &&
          st.map((item: any) => (
            <Card
              className="py-4 bg-bg0"
              key={item.id}
              onPress={() => handleCardClick(item)}
              isPressable
            >
              <CardHeader className="pb-0 pt-2 px-4 flex-col items-start">
                <p className="text-tiny uppercase font-bold">{item.name}</p>
                {/* <small className="text-default-500">{item.name}</small> */}
              </CardHeader>
              <CardBody className="overflow-visible py-2">
                <small className="">{item.details}</small>
              </CardBody>
            </Card>
          ))}
        <Button
          className="py-4 bg-bg0 w-full h-full shadow text-default-800"
          onPress={() => handleCardClick()}
        >
          <Plus size={40} />
        </Button>
      </div>
      {/* Modal that appears when a card is clicked */}
      <Modal isOpen={isOpen} size={size as "md" | "xs" | "sm" | "lg" | "xl" | "2xl" | "3xl" | "4xl" | "5xl" | "full"} onClose={onClose}>
        <ModalContent>
          {(onClose: () => void) => (
            <>
              <ModalHeader className="flex flex-col gap-1 w-full">
                <input type="hidden" value={selectedItem?.id ?? 0} />
                <input
                  type="text"
                  className="font-bold w-full"
                  value={selectedItem?.name ?? ""}
                  placeholder="Title"
                  onChange={(e) =>
                    setSelectedItem({ ...selectedItem, name: e.target.value } as IStickyNote)
                  }
                />
              </ModalHeader>
              <ModalBody>
                <textarea
                  className="w-full px-5 h-fit bg-transparent border-none shadow-transparent outline-none overflow-auto"
                  rows={5}
                  value={selectedItem?.details ?? ""}
                  onChange={(e) =>
                    setSelectedItem({ ...selectedItem, details: e.target.value } as IStickyNote)
                  }
                ></textarea>
              </ModalBody>
              <ModalFooter>
                <Button
                  color="danger"
                  isIconOnly
                  variant="light"
                  onPress={() => {
                    DeleteSn();
                    onClose();
                  }}
                >
                  <Trash2 />
                </Button>
                {/* <Button color="danger" variant="light" onPress={onClose}>Close</Button> */}
                <Button
                  color="success"
                  variant="flat"
                  onPress={() => {
                    if (!selectedItem?.id || selectedItem.id === 0) {
                      CreateSn();
                    } else {
                      UpdateSn();
                    }
                    onClose();
                  }}
                  isIconOnly
                >
                  <Save />
                </Button>
              </ModalFooter>
            </>
          )}
        </ModalContent>
      </Modal>
    </DefaultLayout>
  );
};

