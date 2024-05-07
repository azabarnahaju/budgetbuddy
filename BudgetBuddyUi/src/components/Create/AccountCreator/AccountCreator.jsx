/* eslint-disable react/prop-types */
/* eslint-disable react-hooks/exhaustive-deps */
import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import InputComponent from "../../FormElements/InputComponent";
import "./AccountCreator.scss";

const sampleAccount = {
  balance: 0,
  name: "",
  type: "",
  userId: "",
};

const AccountCreator = ({ currentUser, setAddingNewAcc }) => {
  const [account, setAccount] = useState(sampleAccount);
  const [loading, setLoading] = useState(false);

  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleAccountChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setAccount({ ...account, [key]: value });
  };

  const handleCreateAccount = async (e) => {
    e.preventDefault();
    if (currentUser) {
      try {
        account.userId = currentUser.userId;
        setLoading(true);
        const response = await fetchData(account, "/Account", "POST");
        if (response.ok) {
          setLocalSnackbar({
            open: true,
            message: response.message,
            type: "success",
          });
          window.location.reload();
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
    }
    setLoading(false);
    setAccount(sampleAccount);
    setAddingNewAcc(false)
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="account-creator-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <form onSubmit={handleCreateAccount} className="acc-creator-form">
        <h4 className="mb-4">New account</h4>
        <div>
          <InputComponent
            text="Balance"
            name="balance"
            type="number"
            value={account.balance}
            onChange={handleAccountChange}
          />
        </div>
        <div className="mb-3">
          <InputComponent
            text="Name"
            name="name"
            type="text"
            value={account.name}
            onChange={handleAccountChange}
          />
        </div>
        <div className="mb-3">
          <InputComponent
            text="Type"
            name="type"
            type="text"
            value={account.type}
            onChange={handleAccountChange}
          />
        </div>
        <button className="btn btn-lg acc-creator-form-btn" type="submit">
          Submit
        </button>
      </form>
    </div>
  );
};

export default AccountCreator;
