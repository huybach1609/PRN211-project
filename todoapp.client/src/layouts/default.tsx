import { Link } from "@heroui/link";
import React, { useState } from "react";
import { Navbar } from "../components/ui/navbar";
import { SideBar } from "../components/ui/sizebar";
import { Button } from "@heroui/button";
import { Menu } from "lucide-react";

function DefaultLayout({
  children,
  rightBar,
}: {
  children: React.ReactNode;
  rightBar?: React.ReactNode;
}) {
  const [isOpen, setOpen] = useState(true);

  return (
    <div className="flex h-screen bg-primary-900">
      <SideBar isOpen={isOpen} />

      <Button
        className="absolute w-10 h-10 top-5 left-5 z-50 bg-red-500/50  text-white transition-all duration-300 ease-in-out"
        style={{ opacity: isOpen ? 0 : 100 }}
        isIconOnly
        onPress={() => setOpen(!isOpen)}
      >
        <Menu />
      </Button>

      <div className="flex-1 ">
        <main className="flex justify-center items-center h-full w-full">
          {children}
        </main>
      </div>

      {rightBar && rightBar}
    </div>
  );
}

export default DefaultLayout;

