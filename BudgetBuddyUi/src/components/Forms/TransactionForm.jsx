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
  return (
    <div className="mb-3">
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
        <button className="btn btn-dark ms-4" type="submit">
          Get transaction by id
        </button>
      </form>
    </div>
  );
};

export default TransactionForm;
