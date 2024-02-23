import { useNavigate } from "react-router-dom";
import { useState } from "react";
import SnackBar from "../Snackbar/Snackbar";
import { fetchData } from "../../service/connectionService";

const AchievementForm = () => {
  const [achievementId, setAchievementId] = useState("");
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleSetAchievementId = (e) => {
    setAchievementId(e.target.value);
  };

  const handleDelete = async () => {
    try {
      const response = await fetchData(
        null,
        `/Achievement/delete/${achievementId}`,
        "DELETE"
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
    setAchievementId("");
  };

  const handleNavigateToAchievement = (e) => {
    e.preventDefault();
    if (e.nativeEvent.submitter.id === "getAchievementButton") {
      achievementId && navigate(`/achievement/${achievementId}`);
    } else if (e.nativeEvent.submitter.id === "updateAchievementButton") {
      achievementId && navigate(`/achievement/update/${achievementId}`)
    } else {
      handleDelete();
    }
  };

  const navigateToCreateAchievement = () => {
    navigate("/achievement/create");
  };

  return (
    <div className="mb-3  form-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h1>Achievement</h1>
      <form onSubmit={handleNavigateToAchievement}>
        <label className="form-label mb-3" htmlFor="achievement">
          Achievement id
        </label>
        <input
          className="form-control mb-3"
          required
          value={achievementId}
          onChange={(e) => handleSetAchievementId(e)}
          type="number"
          id="achievement"
          placeholder="Enter the achievement id"
        />
        <div>
          <div className="mb-5">
            <button
              id="getAchievementButton"
              className="btn btn-sm btn-info mx-2 mb-2"
              type="submit"
            >
              Get achievement by id
            </button>
            <button id="updateAchievementButton" className="btn btn-sm btn-warning mx-2 mb-2" type="submit">
              Update achievement by id
            </button>
            <button className="btn btn-sm btn-danger mx-2 mb-2" type="submit">
              Delete achievement by id
            </button>
          </div>
          <div className="mb-5">
            <button
              onClick={navigateToCreateAchievement}
              className="btn btn-success mx-2 mb-2"
            >
              + Create achievement
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default AchievementForm;
