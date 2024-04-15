const ProgressBar = ({ userAchievements, allAchievements }) => {
  return (
    <div class="progress">
      <div
        className="progress-bar"
        role="progressbar"
        style={{
          width: `${(userAchievements.length / allAchievements.length) * 100}%`,
          ariaValuenow: 100,
          ariaValuemin: 0,
          ariaValuemax: 100,
        }}
      >
        {Math.round((userAchievements.length / allAchievements.length) * 100)}%
      </div>
    </div>
  );
};

export default ProgressBar;
