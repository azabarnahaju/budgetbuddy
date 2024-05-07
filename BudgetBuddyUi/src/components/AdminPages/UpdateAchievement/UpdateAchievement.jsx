import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import AchievementForm from "../Achievements/AchievementForm/AchievementForm";
import AdminNavbar from "../AdminNavbar/AdminNavbar";
import { useNavigate } from "react-router-dom";
import { fetchData } from "../../../service/connectionService";
import Loading from "../../Loading/Loading";
import SnackBar from "../../Snackbar/Snackbar";
import SideBar from "../../SideBar/SideBar";
import "./UpdateAchievement.scss";

const UpdateAchievement = () => {
  const { achievementId } = useParams();
  const [achievement, setAchievement] = useState("");
  const [localLoading, setLocalLoading] = useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });
  const navigate = useNavigate();

  const handleSubmit = async (achivementToUpdate) => {
    try {
      setLocalLoading(true);

      const response = await fetchData(
        achivementToUpdate,
        `/achievement/update`,
        "PATCH"
      );
      console.log(response);
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
    setLocalLoading(false);
    setAchievement("");
    navigate("/admin");
  };

  if (localLoading){
    return <Loading />
  }

  return (
    <div className="ach-update-content">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <SideBar />
      <div className="ach-update-container">
        <h3 className="update-ach-title">
          Updating Achievement #{achievementId}
        </h3>
        <AchievementForm
          handleSubmit={handleSubmit}
          setAchievementsToAdd={null}
          achievementsToAdd={null}
        />
        <button type="submit" form="new-ach-form" className="ach-form-btn">
          Submit
        </button>
      </div>
    </div>
  );
};

export default UpdateAchievement;
