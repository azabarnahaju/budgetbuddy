import { useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import { UserContext } from "../../context/userContext";
import { logoutUser } from "../../service/authenticationService";
import { SnackbarContext } from "../../context/snackbarContext";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faInfoCircle,
  faHouse,
  faTrophy,
} from "@fortawesome/free-solid-svg-icons";
import "./Navbar.scss";

const Navbar = () => {
  const { currentUser, setCurrentUser } = useContext(UserContext);
  const { setSnackbar } = useContext(SnackbarContext);
  const navigate = useNavigate();

  const handleLogout = async () => {
    const isLoggedOut = await logoutUser();
    if (isLoggedOut) {
      setSnackbar({
        open: true,
        message: "Successfully logged out.",
        type: "info",
      });
      setCurrentUser(null);
      navigate("/");
      return;
    } else {
      setSnackbar({
        open: true,
        message: "Failed to log out.",
        type: "info",
      });
      navigate("/");
      return;
    }
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-container">
      <div className="container-fluid">
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarTogglerDemo03"
          aria-controls="navbarTogglerDemo03"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <Link className="navbar-brand ms-3 me-3 fs-3" to="/">
          <img className="logo-img" src="/assets/logo.svg" />{" "}
          <span className="logo-text">BudgetBuddy</span>
        </Link>
        <div className="collapse navbar-collapse" id="navbarTogglerDemo03">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link ms-5 fs-3" to="/">
                Home <FontAwesomeIcon icon={faHouse} />
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link ms-4 fs-3" to="/about">
                About <FontAwesomeIcon icon={faInfoCircle} />
              </Link>
            </li>
            {currentUser && (
              <li className="nav-item">
                <Link className="nav-link ms-4 fs-3" to="/achievements">
                  Achievements <FontAwesomeIcon icon={faTrophy} />
                </Link>
              </li>
            )}
          </ul>
          <div className="me-2 mb-2">
            {currentUser ? (
              <span className="nav-item">
                <button className="nav-link ms-4 fs-3" onClick={handleLogout}>
                  Logout
                </button>
              </span>
            ) : (
              <div className="d-flex nav-item">
                <Link to="/login" className="fs-3 nav-link">
                  Login
                </Link>
                <span className="fs-3 mx-2">or</span>
                <Link to="/register" className="fs-3 nav-link">
                  Register
                </Link>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
