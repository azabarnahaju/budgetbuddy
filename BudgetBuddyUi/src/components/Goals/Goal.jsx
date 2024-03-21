/* eslint-disable react-hooks/exhaustive-deps */
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import "./Goal.scss";
import { useState, useEffect, useContext } from "react";
import { fetchData } from "../../service/connectionService";
import { UserContext } from "../../context/userContext";
import Loading from "../Loading/Loading";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleMinus, faPlusCircle } from "@fortawesome/free-solid-svg-icons";
import GoalCreator from "../Create/GoalCreator/GoalCreator";
import { calculatePercentage, formatDate } from "../../utils/helperFunctions";
import AccountSelectComponent from "../FormElements/AccountSelectComponent";

const sampleGoal = {
  userId: "",
  accountId: "",
  type: "",
  target: "",
  currentProgress: 0,
  completed: false,
};

const Goal = () => {
  const [accounts, setAccounts] = useState([]);
  const [account, setAccount] = useState("");
  const { currentUser, loading } = useContext(UserContext);
  const [goal, setGoal] = useState(sampleGoal);
  const [goalList, setGoalList] = useState([]);
  const [showInput, setShowInput] = useState(false);

  const fetchGoals = async (accountId) => {
    const response = await fetchData(null, `/Goal/${accountId}`, "GET");
    if (response.ok) {
      setGoalList(response.data.data);
    }
  };

  const fetchAccounts = async (accountId) => {
    const response = await fetchData(
      null,
      `/Account/${currentUser.userId}`,
      "GET"
    );
    if (response.ok) {
      const accountList = response.data.data["$values"];
      setAccounts(accountList);
      if (accountList.length > 0) {
        const id = accountId ? accountId : accountList[0].id;
        setGoal({ ...goal, accountId: id });
        if (!account) {
          setAccount(accountList[0]);
        }
        fetchGoals(id);
      }
    }
  };

  useEffect(() => {
    if (currentUser) {
      fetchAccounts();
    }
  }, [currentUser]);

  const handleShowInput = () => {
    setShowInput(!showInput);
  };

  const handleSetAccount = (e) => {
    const id = e.target.value;
    const acc = accounts.find((acc) => acc.id == id);
    setAccount(acc);
    fetchAccounts(id);
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="goal-container vh-100">
      <Navbar />
      <div className="goal-content">
        <div className="text-center mt-5">
          <h1>Goals</h1>
        </div>
        <div className="container mt-5">
          <h3>Select your account</h3>
          <div className="row">
            <div className="col-md-4">
              <AccountSelectComponent
                account={account}
                id="account"
                text=""
                array={accounts}
                handleSetAccount={handleSetAccount}
              />
            </div>
          </div>
        </div>
        <div className="container my-5">
          <h3 className="mb-5">Your Goals in your {account.name} account</h3>
          <div className="row">
            <div className="col-md-9">
              <div className="row">
                {goalList.map((goal) => (
                  <div key={goal.id} className="col-md-6">
                    <div className="border rounded p-3 mb-3">
                      <h5>
                        Goal type: {goal.type}{" | "}
                        <span className="lead">{formatDate(goal.startDate)}</span>
                      </h5>
                      <h6>(Target: {goal.target}$)</h6>
                      <h6>
                        Current progress:{" "}
                        {calculatePercentage(goal.currentProgress, goal.target)}
                        %
                      </h6>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
        <div className="text-end me-5 add-goals">
          <div className="add-goal-container">
            <FontAwesomeIcon
              onClick={handleShowInput}
              className="fs-1 goal-icon"
              icon={showInput ? faCircleMinus : faPlusCircle}
            />
            {showInput && (
              <GoalCreator
                goal={goal}
                setGoal={setGoal}
                goalList={goalList}
                currentAccountId={account.id}
                setGoalList={setGoalList}
                setShowInput={setShowInput}
                accounts={accounts}
                sampleGoal={sampleGoal}
              />
            )}
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Goal;
