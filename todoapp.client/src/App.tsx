import { Route, Routes } from "react-router-dom";

import DocsPage from "./pages/docs";
import PricingPage from "./pages/pricing";
import BlogPage from "./pages/blog";
import AboutPage from "./pages/about";
import { Suspense } from "react";
import AuthPage from "./pages/auth/auth.tsx";
import { AuthFilter, NoAuthRoute } from "./filters/authFilter";
import { getUser } from "./utils/tokenManage";
import { AccountInfo } from "./pages/account/accountInfo.tsx";
import { AccountEdit } from "./pages/account/accountEdit.tsx";
import { StickyNote as StickyNotePage } from "./pages/stickynote";
import { ListTaskView } from "./pages/list/liststask.tsx";
import { TaskListView } from "./pages/task/tasklistview.tsx";
import TaskTimeFilter from "./pages/task/tasktimefilter.tsx";
import ErrorPage from "./pages/error";
import { AuthProvider } from "./contexts/AuthContext.tsx";

// Loading component
const PageLoader = () => (
  <div className="flex items-center justify-center min-h-screen">
    <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900" />
  </div>
);

const App = () => {
  const user = getUser() ? JSON.parse(getUser() || '{}') : null;
  const name = user?.userName || "";
  // //console.log(name);

  return (
    <AuthProvider>

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

        {/* Error Routes */}
        <Route path="/error" element={<ErrorPage />} />
        <Route path="/error/:type" element={<ErrorPage />} />
      </Routes>
    </AuthProvider>
  );
};

export default App;

