import { useNavigate } from "react-router-dom";
import { useState } from "react";

const AchievementForm = () => {
  const [achievementId, setAchievementId] = useState(false);
  const navigate = useNavigate();

  const handleSetAchievementId = (e) => {
    setAchievementId(e.target.value);
  };

  const handleNavigateToAchievement = (e) => {
    e.preventDefault();
    achievementId && navigate(`/achievement/${achievementId}`);
  };

  const navigateToCreateAchievement = () => {
    navigate("/achievement/create");
  }

  return (
    <div className="mb-3  form-container">
      <h1>Achievement</h1>
      <form onSubmit={handleNavigateToAchievement}>
        <label className="form-label mb-3" htmlFor="achievement">Achievement id</label>
        <input
        className="form-control mb-3"
          required
          onChange={(e) => handleSetAchievementId(e)}
          type="number"
          id="achievement"
          placeholder="Enter the achievement id"
        />
        <div>
          <div className="mb-5">
            <button className="btn btn-info ms-4" type="submit">
              Get achievement by id
            </button>
          </div>
          <div className="mb-5">
            <button onClick={navigateToCreateAchievement} className="btn btn-success ms-4">
              + Create achievement
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default AchievementForm;
