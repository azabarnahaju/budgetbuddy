/* eslint-disable react/prop-types */
import { useContext, useState } from "react";
import { goalTypes } from "../../../utils/categories";
import { fetchData } from "../../../service/connectionService";
import Loading from "../../Loading/Loading";
import { UserContext } from "../../../context/userContext";
import "./GoalCreator.scss";
import InputComponent from "../../FormElements/InputComponent";
import SelectComponent from "../../FormElements/SelectComponent";
import SnackBar from "../../Snackbar/Snackbar";
import AccountSelectComponent from "../../FormElements/AccountSelectComponent";

const GoalCreator = ({
  goal,
  setGoal,
  accounts,
  currentAccountId,
  setShowInput,
  sampleGoal,
  goalList,
  setGoalList,
}) => {
  const [account, setAccount] = useState("");
  const { currentUser } = useContext(UserContext);
  const [loading, setLoading] = useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleSubmitGoal = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      goal.userId = currentUser.userId;
      const response = await fetchData(goal, "/Goal", "POST");
      if (response.ok) {
        if (goal.accountId == currentAccountId) {
          setGoalList([...goalList, response.data.data]);
        }
        setShowInput(false);
        console.log(response);
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
    setLoading(false);
    setGoal(sampleGoal);
    setAccount("");
  };

  const handleSetGoal = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setGoal({ ...goal, [key]: value });
  };

  const handleSetAccount = (e) => {
    const id = e.target.value;
    const acc = accounts.find(acc => acc.id == id);
    setAccount(acc);
    setGoal({ ...goal, accountId: id });
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="p-3 text-center goal-form-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <form onSubmit={handleSubmitGoal}>
        <h2 className="goal-creator-title">Set a goal</h2>
        <SelectComponent
          text="Select Goal Type"
          id="type"
          value={goal.type}
          array={goalTypes}
          onchange={handleSetGoal}
        />
        <AccountSelectComponent
          account={account}
          id="account2"
          text="Select an account."
          array={accounts}
          handleSetAccount={handleSetAccount}
        />
        <InputComponent
          text="Set the amount to reach"
          name="target"
          type="number"
          value={goal.target}
          onChange={handleSetGoal}
        />
        <button type="submit" className="btn goal-creator-submit-btn mt-4">
          Set Goal
        </button>
      </form>
    </div>
  );
};

export default GoalCreator;
