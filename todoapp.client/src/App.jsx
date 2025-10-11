import { json, Route, Routes, useNavigate } from "react-router-dom";

import IndexPage from "./pages/index";
import DocsPage from "./pages/docs";
import PricingPage from "./pages/pricing";
import BlogPage from "./pages/blog";
import AboutPage from "./pages/about";
import { Suspense, use, useEffect } from "react";
import AuthPage from "./pages/auth/auth";
import { AuthFilter, NoAuthRoute } from "./filters/authFilter";
import { nav } from "framer-motion/client";
import { getToken, getUser, isTokenValid } from "./utils/tokenManage";
import { AccountInfo } from "./pages/account/accountInfo";
import { AccountEdit } from "./pages/account/accountEdit";
import { StickyNote as StickyNotePage } from "./pages/stickynote";
import { ListTaskView } from "./pages/liststask";
import TaskLayout from "./components/layout/tasklayout";
import { TaskListView } from "./pages/task/tasklistview";
import { TaskView } from "./pages/task/taskview";
import TaskTimeFilter from "./pages/task/tasktimefilter";
import { StickyNote } from "lucide-react";


// Loading component
const PageLoader = () => (
  <div className="flex items-center justify-center min-h-screen">
    <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900" />
  </div>
);

const App = () => {
  const user = getUser() ? JSON.parse(getUser()) : null;
  const name = user?.userName || "";
  console.log(name);

  return (
    <Suspense fallback={<PageLoader />}>
      <Routes>
        {/* Public Routes */}
        <Route path="/docs" element={<DocsPage />} />
        <Route path="/pricing" element={<PricingPage />} />
        <Route path="/blog" element={<BlogPage />} />
        <Route path="/about" element={<AboutPage />} />

        {/* Authenticated Routes */}
        <Route element={<AuthFilter />}>
          <Route path="/" element={<StickyNotePage />} />
          <Route path="/sticky-notes" element={<StickyNotePage />} />
          <Route path="/lists" element={<ListTaskView />} />

          <Route path={`user/${name}`}>
            <Route path={`info`} element={<AccountInfo />} />
            <Route path={`edit`} element={<AccountEdit />} />
          </Route>

          <Route path="task">
            <Route path=":type" element={<TaskTimeFilter />} />
            <Route path="list/:listId" element={<TaskListView />} />
          </Route>

        </Route>

        {/* Non-Authenticated Routes */}
        <Route element={<NoAuthRoute />}>
          <Route path="/auth" element={<AuthPage />} />
        </Route>
      </Routes>
    </Suspense>
  );
};

export default App;
