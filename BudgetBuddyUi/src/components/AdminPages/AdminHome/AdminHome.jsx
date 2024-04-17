import React from "react";
import AdminNavbar from "../AdminNavbar/AdminNavbar";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AchievementList from "../Achievements/AchievementList/AchievementList";


const AdminHome = () => {
  return (
    <div>
      <AchievementList />
    </div>
  );
};

export default AdminHome;
