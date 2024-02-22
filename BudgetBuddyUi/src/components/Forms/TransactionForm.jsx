import { useNavigate } from "react-router-dom";
import { useState } from "react";

const TransactionForm = () => {
  const [transactionId, setTransactionId] = useState(false);
  const navigate = useNavigate();

  const handleSetTransactionId = (e) => {
    setTransactionId(e.target.value);
  };

  const handleNavigateToTransaction = (e) => {
    e.preventDefault();
    transactionId && navigate(`/transaction/${transactionId}`);
  };

  const navigateToCreateTransaction = () => {
    navigate("/transaction/create");
  }

  return (
    <div className="mb-3 form-container">
      <h1>Transaction</h1>
      <form onSubmit={handleNavigateToTransaction}>
        <label className="form-label mb-3" htmlFor="transaction">Transaction id</label>
        <input
        className="form-control mb-3"
          required
          onChange={(e) => handleSetTransactionId(e)}
          type="number"
          id="transaction"
          placeholder="Enter the transaction id"
        />
        <div>
          <div className="mb-5">
            <button className="btn btn-info ms-4" type="submit">
              Get transaction by id
            </button>
          </div>
          <div className="mb-5">
            <button onClick={navigateToCreateTransaction} className="btn btn-success ms-4">
              + Create transaction
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default TransactionForm;
