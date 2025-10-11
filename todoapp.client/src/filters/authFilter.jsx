import { Navigate, Outlet } from "react-router-dom"
import { isAuthenticated } from "../utils/tokenManage";

export const AuthFilter = () => {
  if (!isAuthenticated()) {
    return <Navigate to="/auth" />
  }
  return <Outlet />; // Use Outlet instead of children
}

export const NoAuthRoute = () => {
  if (isAuthenticated()) {
    return <Navigate to="/" />
  }
  return <Outlet />; // Use Outlet instead of children
}
