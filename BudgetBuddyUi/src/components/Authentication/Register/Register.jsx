import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";

const registerParams = {
  id: 1,
  registrationDate: new Date(),
  username: "",
  email: "",
  password: "",
  confirmPassword: "",
  achievements: [],
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
    e.preventDefault();
    if (checkPasswordDuplication()) {
      return;
    }
    try {
      setLoading(true);
      const response = await fetchData(registerInfo, "/Register", "POST");
      setLoading(false);
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
      console.log(error);
      setLocalSnackbar({
        open: true,
        message:
          "An error occured during register. Try to login or register again.",
        type: "error",
      });
    }
    setRegisterInfo(registerParams);
  };

  const handleSetRegisterInfo = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setRegisterInfo({ ...registerInfo, [key]: value });
  };

  const checkPasswordDuplication = () => {
    if (registerInfo.password !== registerInfo.confirmPassword) {
      setRegisterInfo(registerParams);
      setLocalSnackbar({
        open: true,
        message: "Passwords do not match.",
        type: "error",
      });
      return true;
    }
    return false;
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
              <div className="mb-3">
                <label className="form-label" htmlFor="registerId">
                  Id
                </label>
                <input
                  className="form-control"
                  value={registerInfo.id}
                  name="id"
                  id="registerId"
                  type="number"
                  required
                  onChange={handleSetRegisterInfo}
                  placeholder="Enter your id"
                />
              </div>
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
