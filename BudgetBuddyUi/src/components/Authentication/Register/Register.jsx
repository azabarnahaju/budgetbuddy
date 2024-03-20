import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";

const registerParams = {
  username: "",
  email: "",
  password: "",
  confirmPassword: "",
};

const Register = () => {
  const [registerInfo, setRegisterInfo] = useState(registerParams);
  const [loading, setLoading] = useState(false);
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
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
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
    <div className="vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <div>
        <div className="text-center">
          <h4>New Here? Join Us Today!</h4>
          <h1>Register</h1>
        </div>
        <div className="form-container">
          <form onSubmit={registerUser}>
            <div className="form-group row">
            </div>
            <div className="form-group row">
              <div className="mb-3">
                <label className="form-label" htmlFor="registerEmail">
                  Email
                </label>
                <input
                  className="form-control"
                  value={registerInfo.email}
                  name="email"
                  id="registerEmail"
                  type="email"
                  required
                  onChange={handleSetRegisterInfo}
                  placeholder="Enter your email"
                />
              </div>
            </div>
            <div className="form-group row">
              <div className="mb-3">
                <label className="form-label" htmlFor="registerUsername">
                  Username
                </label>
                <input
                  className="form-control"
                  value={registerInfo.username}
                  name="username"
                  id="registerUsername"
                  type="text"
                  minLength={5}
                  required
                  onChange={handleSetRegisterInfo}
                  placeholder="Enter your username"
                />
              </div>
            </div>
            <div className="form-group row">
              <div className="mb-3">
                <label className="form-label" htmlFor="registerPassword">
                  Password
                </label>
                <input
                  className="form-control"
                  value={registerInfo.password}
                  id="registerPassword"
                  type="password"
                  name="password"
                  minLength={8}
                  required
                  onChange={handleSetRegisterInfo}
                  placeholder="Enter your password"
                />
              </div>
            </div>
            <div className="form-group row">
              <div className="mb-5">
                <label className="form-label" htmlFor="registerConfirmPassword">
                  Confirm password
                </label>
                <input
                  className="form-control"
                  value={registerInfo.confirmPassword}
                  id="registerConfirmPassword"
                  type="password"
                  name="confirmPassword"
                  minLength={8}
                  required
                  onChange={handleSetRegisterInfo}
                  placeholder="Enter your password"
                />
              </div>
            </div>
            <div className="container d-flex justify-content-center align-items-center">
              <button type="submit" className="btn btn-lg btn-dark">
                Register
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default Register;
