import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { fetchData } from "../../../service/connectionService";
import { SnackbarContext } from "../../../context/snackbarContext";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import "./Login.css";

const authParams = { email: "", password: "" };

const Login = () => {
  const [userInfo, setUserInfo] = useState(authParams);
  const navigate = useNavigate();
  const { setSnackbar } = useContext(SnackbarContext);
  const [loading, setLoading] = new useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

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
    <div className="vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <div>
        <div className="text-center">
          <h4>Already a Member? Login Here:</h4>
          <h1>Login</h1>
        </div>
        <form onSubmit={handleLogin} className="form-container">
          <div className="form-group row">
            <div className="mb-3">
              <label className="form-label" htmlFor="loginEmail">
                Email
              </label>
              <input
                className="form-control"
                value={userInfo.email}
                id="loginEmail"
                name="email"
                type="email"
                required
                onChange={handleSetUserInfo}
                placeholder="Enter your email"
              />
            </div>
          </div>
          <div className="form-group row">
            <div className="mb-3">
              <label className="form-label" htmlFor="loginPassword">
                Password
              </label>
              <input
                className="form-control"
                value={userInfo.password}
                id="loginPassword"
                name="password"
                type="password"
                required
                onChange={handleSetUserInfo}
                placeholder="Enter your password"
              />
            </div>
          </div>
          <div className="container d-flex justify-content-center align-items-center">
            <button type="submit" className="btn btn-lg btn-dark">
              Login
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Login;
