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
  return (
    <div className="mb-3">
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
        <button className="btn btn-dark ms-4" type="submit">
          Get achievement by id
        </button>
      </form>
    </div>
  );
};

export default AchievementForm;
