import { useContext, useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import InputComponent from "../../FormElements/InputComponent";
import Navbar from "../../Navbar/Navbar";
import Footer from "../../Footer/Footer";
import "./Register.scss";
import { useNavigate } from "react-router-dom";
import { SnackbarContext } from "../../../context/snackbarContext";

const registerParams = {
  username: "",
  email: "",
  password: "",
  confirmPassword: "",
};

const Register = () => {
  const [registerInfo, setRegisterInfo] = useState(registerParams);
  const [loading, setLoading] = useState(false);
  const { setSnackbar } = useContext(SnackbarContext);
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const registerUser = async (e) => {
    setLoading(true);
    if (registerInfo.confirmPassword !== registerInfo.password) {
      setLocalSnackbar({
        open: true,
        message: "Passwords do not match.",
        type: "error",
      });
      return;
    }
    e.preventDefault();
    try {
      const response = await fetchData(
        {
          username: registerInfo.username,
          email: registerInfo.email,
          password: registerInfo.password,
        },
        "/Auth/Register",
        "POST"
      );
      if (response.ok) {
        setSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
        navigate("/login");
      } else {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setRegisterInfo(registerParams);
    setLoading(false);
  };

  if (loading) {
    return <Loading />;
  }

  const handleSetRegisterInfo = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setRegisterInfo({ ...registerInfo, [key]: value });
  };

  if (loading) {
    return <Loading message="Sending activation email." />;
  }

  return (
    <div className="vh-100 register-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <Navbar />
      <div className="register-content">
        <div className="container mt-5">
          <div className="text-center">
            <h4>New Here? Join Us Today!</h4>
            <h1>Register</h1>
          </div>
          <div className="form-container">
            <form onSubmit={registerUser}>
              <div className="row d-flex justify-content-center mb-5">
                <div className="mb-3 col-md-4 border rounded p-4 my-3">
                  <div className="mb-3">
                    <InputComponent
                      text="Email"
                      name="email"
                      value={registerInfo.email}
                      type="email"
                      onChange={handleSetRegisterInfo}
                    />
                  </div>
                  <div className="mb-3">
                    <InputComponent
                      text="Username"
                      name="username"
                      value={registerInfo.username}
                      type="text"
                      onChange={handleSetRegisterInfo}
                    />
                  </div>
                  <div className="mb-3">
                    <InputComponent
                      text="Password"
                      name="password"
                      value={registerInfo.password}
                      type="password"
                      onChange={handleSetRegisterInfo}
                    />
                  </div>
                  <div className="mb-5">
                    <InputComponent
                      text="Confirm password"
                      name="confirmPassword"
                      value={registerInfo.confirmPassword}
                      type="password"
                      onChange={handleSetRegisterInfo}
                    />
                  </div>
                  <div className="text-center">
                    <button
                      type="submit"
                      className="btn btn-lg btn-outline-light"
                    >
                      Register
                    </button>
                  </div>
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Register;
