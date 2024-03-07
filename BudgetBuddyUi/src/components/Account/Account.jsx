/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import { fetchData } from "../../service/connectionService";
import Loading from "../Loading/Loading";
import SnackBar from "../Snackbar/Snackbar";
import { useParams, useNavigate } from "react-router-dom";

const Account = () => {
  const [loading, setLoading] = useState(true);
  const [account, setAccount] = useState({});
  const { id } = useParams();
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  useEffect(() => {
    handleGetAccounts(id);
  }, [id]);

  const handleGetAccounts = async (accountId) => {
    try {
      const response = await fetchData(null, `/Account/${accountId}`, "GET");
      if (response.ok) {
        setAccount(response.data.data);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setAccount(false);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setAccount(false);
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setLoading(false);
  };

  if (loading) {
    return <Loading />;
  }

  const handleBack = () => {
    navigate("/");
  }

  return (
    <div>
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h2>Account</h2>
      {account ? (
        <div>
          <h4>Id: {account.id}</h4>
          <h4>Balance: {account.balance}</h4>
          <h4>Date: {account.date}</h4>
          <h4>Name: {account.name}</h4>
          <h4>Type: {account.type}</h4>
          <div>
            <h4>Transactions:</h4>
            {account.transactions ? account.transactions.map((transaction) => (
              <div key={transaction.accountId}>
                <h6>Amount: {transaction.amount}</h6>
                <h6>Date {transaction.date}</h6>
                <h6>Name: {transaction.name}</h6>
                <h6>Tag: {transaction.tag}</h6>
                <h6>Type: {transaction.type}</h6>
              </div>
            )) : <h5>No transactions yet.</h5>}
          </div>
        </div>
      ) : (
        <h4>Account not found</h4>
      )}
      <button onClick={handleBack} className="btn btn-lg btn-dark">Back</button>
    </div>
  );
};

export default Account;
