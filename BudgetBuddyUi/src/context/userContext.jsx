/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable react/prop-types */
import { createContext, useState, useEffect } from "react";
import { getAuth } from "../service/authenticationService";
import { useNavigate } from "react-router-dom";

export const UserContext = createContext({
  currentUser: null,
  loading: true,
  setCurrentUser: () => null,
});

export const UserProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const value = { currentUser, loading, setCurrentUser };
  const navigate = useNavigate();

  const fetchAuthStatus = async () => {
    try {
      const isAuthenticated = await getAuth();
      setCurrentUser(isAuthenticated);
      setLoading(false);
    } catch (error) {
      console.error("Error while fetching authentication status:", error);
      setLoading(false);
    }
  };

  useEffect(() => {
    const unlisten = () => {
      fetchAuthStatus();
    };
    navigate(unlisten);

    return () => {
      unlisten();
    };
  }, [navigate]);
  
  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
};