import { ToastProvider } from "@heroui/react";
import { HeroUIProvider } from "@heroui/system";
import { useHref, useNavigate } from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";

export function Provider({ children }: { children: React.ReactNode }) {
  const navigate = useNavigate();

  return (
    <HeroUIProvider navigate={navigate} useHref={useHref}>
        <ToastProvider />
        {children}
    </HeroUIProvider>
  );
}

