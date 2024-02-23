import { useNavigate } from "react-router-dom";
import { useState } from "react";
import SnackBar from "../Snackbar/Snackbar";
import { fetchData } from "../../service/connectionService";

const TransactionForm = () => {
  const [transactionId, setTransactionId] = useState("");
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleSetTransactionId = (e) => {
    setTransactionId(e.target.value);
  };

  const handleDelete = async () => {
    try {
      const response = await fetchData(
        null,
        `/Transaction/delete/${transactionId}`,
        "DELETE"
      );
      if (response.ok) {
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
    setTransactionId("");
  };

  const handleNavigateToTransaction = (e) => {
    e.preventDefault();
    if (e.nativeEvent.submitter.id === "getTransactionButton") {
      transactionId && navigate(`/transaction/${transactionId}`);
    } else if (e.nativeEvent.submitter.id === "updateTransactionButton"){
      transactionId && navigate(`/transaction/update/${transactionId}`)
    } else {
      handleDelete();
    }
  };

  const navigateToCreateTransaction = () => {
    navigate("/transaction/create");
  };

  return (
    <div className="mb-3 form-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h1>Transaction</h1>
      <form onSubmit={handleNavigateToTransaction}>
        <label className="form-label mb-3" htmlFor="transaction">
          Transaction id
        </label>
        <input
          className="form-control mb-3"
          required
          value={transactionId}
          onChange={(e) => handleSetTransactionId(e)}
          type="number"
          id="transaction"
          placeholder="Enter the transaction id"
        />
        <div>
          <div className="mb-5">
            <button
              id="getTransactionButton"
              className="btn btn-sm btn-info mx-2 mb-2"
              type="submit"
            >
              Get transaction by id
            </button>
            <button id="updateTransactionButton" className="btn btn-sm btn-warning mx-2 mb-2" type="submit">
              Update transaction by id
            </button>
            <button className="btn btn-sm btn-danger mx-2 mb-2" type="submit">
              Delete transaction by id
            </button>
          </div>
          <div className="mb-5">
            <button
              onClick={navigateToCreateTransaction}
              className="btn btn-success mx-2 mb-2"
            >
              + Create transaction
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default TransactionForm;
