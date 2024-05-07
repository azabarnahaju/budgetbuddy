import React from 'react'
import { Link, useNavigate } from "react-router-dom";
import { useContext } from 'react';
import { UserContext } from '../../context/userContext';
import { SnackbarContext } from '../../context/snackbarContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { logoutUser } from '../../service/authenticationService';
import {
  faHouse,
  faWallet,
  faBullseye,
  faChartLine,
  faTrophy,
  faUser,
  faHammer,
} from "@fortawesome/free-solid-svg-icons";
import "./SideBar.scss";

const SideBar = () => {
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
    <div className="position-fixed">
      <div
        className="sidebar d-flex flex-shrink-0 bg-light"
        style={{ width: 4 + "rem" }}
      >
        <div className="sidebar-content d-flex flex-column justify-content-between">
          <ul className="nav nav-pills nav-flush flex-column mb-auto text-center">
            <li>
              <Link
                to="/dashboard"
                className="nav-link py-3 border-bottom"
                title="Dashboard"
                data-bs-toggle="tooltip"
                data-bs-placement="right"
              >
                <FontAwesomeIcon
                  className="sidebar-icon"
                  icon={faHouse}
                  size="xl"
                />
              </Link>
            </li>
            <li>
              <Link
                to="/accounts"
                className="nav-link py-3 border-bottom"
                title="Accounts"
                data-bs-toggle="tooltip"
                data-bs-placement="right"
              >
                <FontAwesomeIcon
                  className="sidebar-icon"
                  icon={faWallet}
                  size="xl"
                />
              </Link>
            </li>
            <li>
              <Link
                to="/goals"
                className="nav-link py-3 border-bottom"
                title="Goals"
                data-bs-toggle="tooltip"
                data-bs-placement="right"
              >
                <FontAwesomeIcon
                  className="sidebar-icon"
                  icon={faBullseye}
                  size="xl"
                />
              </Link>
            </li>
            <li>
              <Link
                to="/reports"
                className="nav-link py-3 border-bottom"
                title="Reports"
                data-bs-toggle="tooltip"
                data-bs-placement="right"
              >
                <FontAwesomeIcon
                  className="sidebar-icon"
                  icon={faChartLine}
                  size="xl"
                />
              </Link>
            </li>
            <li>
              <Link
                to="/achievements"
                className="nav-link py-3 border-bottom"
                title="Achievements"
                data-bs-toggle="tooltip"
                data-bs-placement="right"
              >
                <FontAwesomeIcon
                  className="sidebar-icon"
                  icon={faTrophy}
                  size="xl"
                />
              </Link>
            </li>
            {currentUser && currentUser.role === "Admin" && (
              <li>
                <Link
                  to="/admin"
                  className="nav-link py-3 border-bottom"
                  title="Admin Panel"
                  data-bs-toggle="tooltip"
                  data-bs-placement="right"
                >
                  <FontAwesomeIcon
                    className="sidebar-icon"
                    icon={faHammer}
                    size="xl"
                  />
                </Link>
              </li>
            )}
          </ul>
          <div className="dropdown border-top">
            <Link
              className="d-flex align-items-center justify-content-center p-3 link-dark text-decoration-none dropdown-toggle"
              id="dropdownUser3"
              data-bs-toggle="dropdown"
              aria-expanded="false"
            >
              <FontAwesomeIcon icon={faUser} size="lg" />
            </Link>
            <ul
              className="dropdown-menu text-small shadow"
              aria-labelledby="dropdownUser3"
            >
              <li>
                <Link className="dropdown-item" onClick={handleLogout}>
                  Sign out
                </Link>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}

export default SideBar;