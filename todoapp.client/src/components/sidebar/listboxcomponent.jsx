import { Badge, ListboxItem } from "@heroui/react";
import { useEffect, useState } from "react";

export const ListboxWrapper = ({ header, children }) => (
    <div className="w-full max-w-[260px] pt-2  ">
        <div className="text-sm font-bold ml-1">{header}</div>
        {children}
        <hr className="border-t border-bg5 my-1" />
    </div>
);
export const ListboxItemView = (label, key, color, count = 0) => {

    return (
        <ListboxItem className="text-sm hover:bg-green" key={key}
            endContent={
                count != 0 ? (
                    <div className="p-3 w-4 h-4 rounded-lg bg-bg5 flex justify-center items-center">{count}</div>
                ) : (<></>)
            }
        >{label}
        </ListboxItem>
    )

}