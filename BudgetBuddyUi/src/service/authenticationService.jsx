import { baseUrl } from "../utils/urls";

export const getAuth = async () => {
    try {
      const response = await fetch(`${baseUrl}/User/AmISignedIn`, {
        credentials: "include",
      });
      if (response.ok) {
        const isAuthenticated = await response.json();
        return isAuthenticated;
      } else {
        console.error(
          "Failed to fetch authentication status:",
          response.statusText
        );
        return false;
      }
    } catch (error) {
      console.error("Error while fetching authentication status.");
      return false;
    }
  };