import React, { useContext, useEffect, useState } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { AuthContext, Private } from "context/auth";
import SignInPage from "pages/authentication/sign-in";
import Home from "pages/home";

function DynamicRouter() {
  const { tenantData } = useContext(AuthContext);
  const [RenderedRoutes, setRenderedRoutes] = useState(false);
  const [generatedRoutes, setGeneratedRoutes] = useState([]);

  useEffect(() => {
    if (tenantData && !RenderedRoutes) {
      const routes = getRoutes(tenantData.menu);
      setGeneratedRoutes(routes);
      setRenderedRoutes(true);
    }
  }, [tenantData, RenderedRoutes]);

  const getRoutes = (allRoutes) => {
    return allRoutes.flatMap((route) => {
      if (route.collapse) {
        return getRoutes(route.collapse);
      }

      if (route.route) {
        if (route.renderable) {
          let component = require(`../../${route.component}`);
          return (
            <Route
              exact
              path={route.route}
              element={
                route.private ? (
                  <Private>{React.createElement(component.default)}</Private>
                ) : (
                  React.createElement(component.default)
                )
              }
              key={route.key}
            />
          );
        }
        return null;
      }
      else{
        <Route exact path="/" element={<Home />} />
      }
    });
  };

  return (
    <Routes>
      {generatedRoutes}
      <Route exact path="/signIn" element={<SignInPage />} />
      <Route exact path="/" element={<Home />} />
    </Routes>
  );
}

export default DynamicRouter;
