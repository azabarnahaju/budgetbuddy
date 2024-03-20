import { fetchData } from "./connectionService";

export const logoutUser = async () => {
  try {
    localStorage.removeItem("accessToken");
    return true;
  } catch (error) {
    console.log("Failed to logout.");
    return false;
  }
};

// Function to check authentication status
export const getAuth = async () => {
  try {
    const token = localStorage.getItem("accessToken");
    const userAuth = await fetchData({ token }, "/Auth/Validate", "POST");
    return userAuth;
  } catch (error) {
    console.error("Error. Server not responding.");
    return false;
  }
};
