import { useState, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { fetchData } from "../../../service/connectionService";
import { SnackbarContext } from "../../../context/snackbarContext";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import "./Login.scss";
import InputComponent from "../../FormElements/InputComponent";
import Navbar from "../../Navbar/Navbar";
import Footer from "../../Footer/Footer";

const authParams = { email: "", password: "" };

const Login = () => {
  const [userInfo, setUserInfo] = useState(authParams);
  const navigate = useNavigate();
  const { snackbar, setSnackbar } = useContext(SnackbarContext);
  const [loading, setLoading] = new useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  useEffect(() => {
    setTimeout(() => {
      setSnackbar({
        open: false,
        message: "",
        type: "",
      });
    }, 6000);
  }, [setSnackbar]);

  const handleLogin = async (e) => {
    setLoading(true);
    e.preventDefault();
    try {
      const response = await fetchData(
        {
          password: userInfo.password,
          email: userInfo.email,
        },
        "/Auth/Login",
        "POST"
      );

      if (response.ok) {
        localStorage.setItem("accessToken", response.data.data.token);
        setSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });

        navigate("/");
        return;
      } else {
        setSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
        if (response.status === 403) {
          navigate("/activateAccount");
        }
      }
    } catch (error) {
      console.log(error);
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setUserInfo(authParams);
    setLoading(false);
  };

  const handleSetUserInfo = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setUserInfo({ ...userInfo, [key]: value });
  };

  if (loading) {
    return <Loading message="Logging in..." />;
  }

  return (
    <div className="vh-100 login-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <SnackBar
        {...snackbar}
        setOpen={() => setSnackbar({ ...snackbar, open: false })}
      />
      <Navbar />
      <div className="login-content">
        <div className="container mt-5">
          <div className="text-center">
            <h4>Already a Member? Login Here:</h4>
            <h1>Login</h1>
          </div>
          <form onSubmit={handleLogin}>
            <div className="row d-flex justify-content-center">
              <div className="mb-3 col-md-4 border rounded p-4 my-3">
                <div className="mb-3">
                  <InputComponent
                    text="Email"
                    name="email"
                    type="email"
                    value={userInfo.email}
                    onChange={handleSetUserInfo}
                  />
                </div>
                <div className="mb-3">
                  <InputComponent
                    text="Password"
                    name="password"
                    type="password"
                    value={userInfo.password}
                    onChange={handleSetUserInfo}
                  />
                </div>
              </div>
            </div>
            <div className="container d-flex justify-content-center align-items-center">
              <button type="submit" className="btn btn-lg btn-outline-light">
                Login
              </button>
            </div>
          </form>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Login;
