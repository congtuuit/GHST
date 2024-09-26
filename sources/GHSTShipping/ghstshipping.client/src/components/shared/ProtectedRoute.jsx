// ProtectedRoute.js
import React from "react";
import { Navigate, Outlet , useNavigate} from "react-router-dom";
import UserAvatar from "@/components/UserAvatar";
import {
  ProCard,
  ProFormGroup,
  ProFormSwitch,
} from '@ant-design/pro-components';

const ProtectedRoute = () => {
  const navigate = useNavigate();

  // Example token check
  // const valid = authServices.check();
  // if (!valid) {
  //   // Redirect to login if no token is found
  //   return <Navigate to="/login" replace />;
  // }

  //const userSession = authServices.getUserSession();

  function handleLogout() {
    //authServices.logout();
    navigate("/login");
  }

  // If token exists, render the child components
  return (
    <ProCard title={<UserAvatar user={{}} onLogout={handleLogout} />}>
      <Outlet />
    </ProCard>
  );
};

export default ProtectedRoute;