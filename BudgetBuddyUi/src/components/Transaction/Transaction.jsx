/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import { fetchData } from "../../service/connectionService";
import Loading from "../Loading/Loading";
import SnackBar from "../Snackbar/Snackbar";
import { useParams, useNavigate } from "react-router-dom";

const Transaction = () => {
  const [loading, setLoading] = useState(true);
  const [transaction, setTransaction] = useState({});
  const { id } = useParams();
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  useEffect(() => {
    handleGetTransactions(id);
  }, [id]);

  const handleGetTransactions = async (transactionId) => {
    try {
      const response = await fetchData(null, `/Transaction/transactions/${transactionId}`, "GET");
      if (response.ok) {
        setTransaction(response.data.data);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setTransaction(false);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setTransaction(false);
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setLoading(false);
  };

  const handleBack = () => {
    navigate("/");
  }

  if (loading) {
    return <Loading />;
  }

  return (
    <div>
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h2>Transaction</h2>
      {transaction ? (
        <div>
          <h4>Account id: {transaction.accountId}</h4>
          <h4>Transaction id: {transaction.id}</h4>
          <h4>Amount: {transaction.amount}</h4>
          <h4>Date: {transaction.date}</h4>
          <h4>Name: {transaction.name}</h4>
          <h4>Tag: {transaction.tag}</h4>
          <h4>Type: {transaction.type}</h4>
        </div>
      ) : (
        <h4>Transaction not found</h4>
      )}
      <button onClick={handleBack} className="btn btn-lg btn-dark">Back</button>
    </div>
  );
};

export default Transaction;
