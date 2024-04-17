/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable react/prop-types */
import { createContext, useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { getAuth, logoutUser } from "../service/authenticationService";

export const UserContext = createContext({
  currentUser: null,
  loading: true,
  setCurrentUser: () => {},
});

export const UserProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const value = { currentUser, loading, setCurrentUser };
  const navigate = useNavigate();

  const fetchAuthStatus = async () => {
    try {
      const userAuth = await getAuth();

      if (!userAuth.ok) {
        logoutUser();
        setCurrentUser(null);
      } else {
        console.log(userAuth.data.data);
        setCurrentUser(userAuth.data.data);
      }
    } catch (error) {
      logoutUser();
      setCurrentUser(null);
      console.error("Error while fetching authentication status:", error);
    }
    setLoading(false);
  };

  useEffect(() => {
    const unlisten = () => {
      fetchAuthStatus();
    };

    return () => {
      unlisten();
    };
  }, [navigate]);

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>;
};
