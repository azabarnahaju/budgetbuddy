import { baseUrl } from "../utils/urls";

export const fetchData = async (body, path, method) => {
  try {
    const url = `${baseUrl}${path}`;
    const options = {
      method: method,
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    };

    if (body !== null) {
      options.body = JSON.stringify(body);
    }

    const response = await fetch(url, options);
    console.log(response);
    const data = await response.json();
    if (response.ok) {
      return { ok: true, message: data.message, data: data };
    } else {
      if (!data.message) {
        return { ok: false, message: "An unexpected error occured." };
      }
      return { ok: false, message: data.message };
    }
  } catch (error) {
    return { ok: false, message: "Error: The server not responding." };
  }
};
