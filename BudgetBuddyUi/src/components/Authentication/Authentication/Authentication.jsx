import Login from "../Login/Login";
import Register from "../Register/Register";
import { useContext } from "react";
import { UserContext } from "../../../context/userContext";
import { useNavigate } from "react-router-dom";

const Authentication = () => {
  const { currentUser } = useContext(UserContext);
  const navigate = useNavigate();

  if (currentUser) {
    navigate("/");
    return;
  }

  const handleBack = () => {
    navigate("/");
  };

  return (
    <div className="main vh-100">
      <div className=" content container d-flex justify-content-center align-items-center">
        <div className="container">
          <div className="row pt-5">
            <div className="col-md-6">
              <div className="mx-5">
                <Register />
              </div>
            </div>
            <div className="col-md-6">
              <div className="mx-5">
                <Login />
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="text-center pb-5">
        <button onClick={handleBack} className="btn btn-info">
          Back
        </button>
      </div>
    </div>
  );
};

export default Authentication;
