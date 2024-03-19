/* eslint-disable react/prop-types */
import { useContext, useState } from "react";
import { goalTypes } from "../../../utils/categories";
import { SnackbarContext } from "../../../context/snackbarContext";
import { fetchData } from "../../../service/connectionService";
import Loading from "../../Loading/Loading";

const sampleGoal = {
  userId: 1,
  type: "",
  target: "",
  currentProgress: 0,
  completed: false,
  startDate: new Date(),
};

const GoalCreator = ({ goals, setGoals }) => {
  const [goal, setGoal] = useState(sampleGoal);
  const [loading, setLoading] = useState(false);
  const { setSnackbar } = useContext(SnackbarContext);

  const handleSetGoal = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      const response = await fetchData(goal, "/Goal", "POST");
      if (response.ok) {
        response.data.data.startDate = new Date() - 3600 * 1000;
        setGoals([...goals, response.data.data]);
        setSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setLoading(false);
    setGoal(sampleGoal);
  };

  const handleChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setGoal({ ...goal, [key]: value });
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div>
      <div className="col-md-6 border rounded p-3 mb-5">
        <form onSubmit={handleSetGoal}>
          <h2>Set a goal</h2>
          <select
            onChange={handleChange}
            className="form-control mb-3"
            value={goal.type}
            required
            id="type"
            name="type"
          >
            <option disabled value="">
              Select Goal Type
            </option>
            {goalTypes.map((goal, index) => (
              <option key={index} value={goal}>
                {goal}
              </option>
            ))}
          </select>
          <label htmlFor="target">Set the amount to reach</label>
          <input
            id="target"
            name="target"
            value={goal.target}
            className="form-control mb-3"
            type="number"
            onChange={handleChange}
            required
          />
          <button type="submit" className="btn btn-info">
            Set target
          </button>
        </form>
      </div>
    </div>
  );
};

export default GoalCreator;
