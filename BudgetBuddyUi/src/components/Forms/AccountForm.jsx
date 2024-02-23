import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { fetchData } from "../../service/connectionService";
import SnackBar from "../Snackbar/Snackbar";

const AccountForm = () => {
  const [accountId, setAccountId] = useState("");
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleSetAccountId = (e) => {
    setAccountId(e.target.value);
  };

  const handleDelete = async () => {
    try {
      const response = await fetchData(null, `/Account/${accountId}`, "DELETE");
      if (response.ok){
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
    setAccountId("");
  }

  const handleNavigateToAccount = (e) => {
    e.preventDefault();
    if (e.nativeEvent.submitter.id === "getAccountButton"){
      accountId && navigate(`/account/${accountId}`);
    } else if (e.nativeEvent.submitter.id === "updateAccountButton") {
      accountId && navigate(`/account/update/${accountId}`)
    } else {
      handleDelete();
    }
    
  };

  const navigateToCreateAccount = () => {
    navigate("/account/create");
  }

  return (
    <div className="mb-3 form-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h1>Account</h1>
      <form onSubmit={handleNavigateToAccount}>
        <label className="form-label mb-3" htmlFor="account">
          Account id
        </label>
        <input
          className="form-control mb-3"
          required
          value={accountId}
          onChange={(e) => handleSetAccountId(e)}
          type="number"
          id="account"
          placeholder="Enter the account id"
        />
        <div>
          <div className="mb-5">
            <button id="getAccountButton" className="btn btn-sm btn-info mx-2 mb-2" type="submit">
              Get account by id
            </button>
            <button id="updateAccountButton" className="btn btn-sm btn-warning mx-2 mb-2" type="submit">
              Update account by id
            </button>
            <button className="btn btn-sm btn-danger mx-2 mb-2" type="submit">
              Delete account by id
            </button>
          </div>
          <div className="mb-5">
            <button onClick={navigateToCreateAccount} className="btn btn-success mx-2">
              + Create account
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default AccountForm;
