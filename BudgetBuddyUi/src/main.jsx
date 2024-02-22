import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App.jsx";
import { UserProvider } from "./context/userContext.jsx";
import { SnackbarProvider } from "./context/snackbarContext.jsx";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <BrowserRouter>
      <UserProvider>
        <SnackbarProvider>
          <App />
        </SnackbarProvider>
      </UserProvider>
    </BrowserRouter>
  </React.StrictMode>
);
