import { useState, useEffect } from "react";
import { AuthProvider, Private } from "context/auth";
import { Route, useLocation } from "react-router-dom";
import { ToastContainer } from 'react-toastify';
import { ThemeProvider } from "@mui/material/styles";
import { useMaterialUIController, setMiniSidenav } from "context/material";
import DynamicRouter from "pages/components/DynamicRouter";
import CssBaseline from "@mui/material/CssBaseline";
import Sidenav from "pages/components/Sidenav";
import theme from "assets/theme";

import 'react-toastify/dist/ReactToastify.css';

export default function App() {
  const [controller, dispatch] = useMaterialUIController();
  const { miniSidenav, direction, layout, sidenavColor } = controller;
  const [onMouseEnter, setOnMouseEnter] = useState(false);
  const { pathname } = useLocation();


  const handleOnMouseEnter = () => {
    if (miniSidenav && !onMouseEnter) {
      setMiniSidenav(dispatch, false);
      setOnMouseEnter(true);
    }
  };

  const handleOnMouseLeave = () => {
    if (onMouseEnter) {
      setMiniSidenav(dispatch, true);
      setOnMouseEnter(false);
    }
  };

  useEffect(() => {
    document.body.setAttribute("dir", 'ltr');
  }, [direction]);


  useEffect(() => {
    document.documentElement.scrollTop = 0;
    document.scrollingElement.scrollTop = 0;
  }, [pathname]);


  const getRoutes = (allRoutes) =>
    allRoutes.map((route) => {
      if (route.collapse) {
        return getRoutes(route.collapse);
      }

      if (route.route) {
        return <Route exact path={route.route} element={route.private ? <Private>{route.component}</Private> : route.component} key={route.key} />;
      }
      return null;
    });

  return (
  <>
    <ToastContainer />
    <AuthProvider>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Private>
          {layout === "dashboard" && (
            <Sidenav
              onMouseEnter={handleOnMouseEnter}
              onMouseLeave={handleOnMouseLeave}
            />
          )}
        </Private>
        <DynamicRouter />
      </ThemeProvider>
    </AuthProvider>
  </>
  );
}
