/* eslint-disable react/prop-types */
import { formatDate, calculatePercentage } from "../../utils/helperFunctions";

const GoalList = ({ account, goalList }) => {
  return (
    <div className="goal-list-container">
      <h3 className="mb-5">Your Goals in your {account.name} account</h3>
      <div className="row">
        <div className="col-md-12">
          <div className="row">
            {goalList.map((goal) => (
              <div key={goal.id} className="col-md-6">
                <div className="goal-item p-3 mb-3">
                  <h5>
                    Goal type: {goal.type}
                    {" | "}
                    <span className="lead">{formatDate(goal.startDate)}</span>
                  </h5>
                  <h6>(Target: {goal.target}$)</h6>
                  <h6>
                    Current progress:{" "}
                    {goal.completed
                      ? "Completed"
                      : `${calculatePercentage(
                          goal.currentProgress,
                          goal.target
                        )}%`}
                  </h6>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default GoalList;
