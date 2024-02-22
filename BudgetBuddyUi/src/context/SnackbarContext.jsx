/* eslint-disable react/prop-types */
import { createContext, useState } from "react";

export const SnackbarContext = createContext({
  snackbar: {
    open: false,
    message: "",
    type: "",
  },
  setSnackbar: () => {},
});

export const SnackbarProvider = ({ children }) => {
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });
  const value = { snackbar, setSnackbar };

  return (
    <SnackbarContext.Provider value={value}>
      {children}
    </SnackbarContext.Provider>
  );
};