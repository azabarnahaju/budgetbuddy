import AccountForm from "../Forms/AccountForm";
import AchievementForm from "../Forms/AchievementForm";
import TransactionForm from "../Forms/TransactionForm";
import { useContext, useEffect, useState } from "react";
import { UserContext } from "../../context/userContext";
import { useNavigate } from "react-router-dom";
import { fetchData } from "../../service/connectionService";
import { SnackbarContext } from "../../context/snackbarContext";
import SnackBar from "../Snackbar/Snackbar";
import "./Home.css";
import GoalCreator from "../Create/GoalCreator/GoalCreator";
import { formatDate } from "../../utils/helperFunctions";

const Home = () => {
  const { currentUser } = useContext(UserContext);
  const { snackbar, setSnackbar } = useContext(SnackbarContext);
  const [goals, setGoals] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    setTimeout(() => {
      setSnackbar({
        open: false,
        message: "",
        type: "",
      });
    }, 6000);
  }, [setSnackbar]);

  useEffect(() => {
    const fetchGoals = async () => {
      const response = await fetchData(null, "/Goal/1", "GET");
      if (response.ok) {
        setGoals(response.data.data);
      }
    };
    fetchGoals();
  }, []);

  const handleLogout = async () => {
    const response = await fetchData(null, "/User/Logout", "POST");
    if (response.ok) {
      window.location.reload();
      return;
    } else {
      setSnackbar({
        open: true,
        message: response.message,
        type: "error",
      });
    }
  };

  const handleLogging = () => {
    if (currentUser) {
      handleLogout();
      return;
    } else {
      navigate("/authentication");
      return;
    }
  };

  return (
    <div className="container mt-4">
      <SnackBar
        {...snackbar}
        setOpen={() => setSnackbar({ ...snackbar, open: false })}
      />
      <div className="row justify-content-between">
        {goals && (
          <h2>
            {goals.map((goal, index) => (
              <div key={index}>
                <h4>
                  <span>Goal: {goal.type}</span>
                  <span> - </span>
                  <span>{goal.currentProgress}%</span>
                  <span> - </span>
                  <span>Set at {formatDate(goal.startDate)}</span>
                </h4>
              </div>
            ))}
          </h2>
        )}
        <GoalCreator goals={goals} setGoals={setGoals} />
        <div className="col-md-6 mx-2">
          <AccountForm />
          <AchievementForm />
          <TransactionForm />
        </div>
        <div className="my-8 col-md-2 d-flex justify-content">
          <div className="h-stack">
            {currentUser && (
              <h4 className="welcome-msg display-6">
                Hello {currentUser.userName}!
              </h4>
            )}
          </div>
          <div className="mt-3 mx-5">
            <button onClick={handleLogging} className="btn btn-lg btn-info">
              {currentUser ? "Logout" : "Login"}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
