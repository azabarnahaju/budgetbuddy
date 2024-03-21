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
import AccountSelectComponent from "../FormElements/AccountSelectComponent";
import { useNavigate } from "react-router";
import SnackBar from "../Snackbar/Snackbar";
import GoalList from "./GoalList";

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
  const navigate = useNavigate();
  const { currentUser, loading } = useContext(UserContext);
  const [pageLoading, setPageLoading] = useState(false);
  const [goal, setGoal] = useState(sampleGoal);
  const [goalList, setGoalList] = useState([]);
  const [showInput, setShowInput] = useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const fetchGoals = async (accountId) => {
    setPageLoading(true);
    try {
      const response = await fetchData(null, `/Goal/${accountId}`, "GET");
      if (response.ok) {
        setGoalList(response.data.data);
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "The server not responding",
        type: "error",
      });
    }
    setPageLoading(false);
  };

  const fetchAccounts = async (accountId) => {
    setPageLoading(true);
    try {
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
        message: "The server not responding",
        type: "error",
      });
    }

    setPageLoading(false);
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

  if (loading || pageLoading) {
    return <Loading />;
  }

  if (!loading && !currentUser) {
    navigate("/");
  }

  return (
    <div className="goal-container vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
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
        <GoalList account={account} goalList={goalList} />
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
