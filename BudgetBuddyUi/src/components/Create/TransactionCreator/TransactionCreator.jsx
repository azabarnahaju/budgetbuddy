import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import { useNavigate } from "react-router-dom";
import { tags, types } from "../../../utils/categories";

const sampleTransaction = {
  id: 0,
  date: new Date(),
  name: "",
  amount: 0,
  tag: "",
  type: "",
  accountId: 1,
};

const TransactionCreator = () => {
  const [transaction, setTransaction] = useState(sampleTransaction);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleTransactionChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setTransaction({ ...transaction, [key]: value });
  };

  const handleBack = () => {
    navigate("/");
  };

  const handleCreateTransaction = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      const response = await fetchData(transaction, "/Transaction/add", "POST");
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
    setLoading(false);
    setTransaction(sampleTransaction);
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="container mt-5">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h1>Create new transaction:</h1>
      <form onSubmit={handleCreateTransaction}>
        <label className="form-label mb-3" htmlFor="id">
          Transaction id
        </label>
        <input
          onChange={handleTransactionChange}
          className="form-control mb-3"
          value={transaction.id}
          required
          type="number"
          id="id"
          name="id"
          placeholder="Enter the transaction id"
        />
        <label className="form-label mb-3" htmlFor="name">
          Name
        </label>
        <input
          onChange={handleTransactionChange}
          className="form-control mb-3"
          required
          value={transaction.name}
          type="text"
          id="name"
          name="name"
          placeholder="Enter the name of the transaction"
        />
        <label className="form-label mb-3" htmlFor="amount">
          Amount
        </label>
        <input
          onChange={handleTransactionChange}
          className="form-control mb-3"
          value={transaction.amount}
          required
          type="number"
          id="amount"
          name="amount"
          placeholder="Enter the transaction amount"
        />
        <label className="form-label mb-3" htmlFor="tag">
          Tag
        </label>
        <select
          onChange={handleTransactionChange}
          className="form-control mb-3"
          value={transaction.tag}
          required
          id="tag"
          name="tag"
        >
          <option value="">Select Tag</option>
          {tags.map((tag, index) => (
            <option key={index} value={tag}>
              {tag}
            </option>
          ))}
        </select>
        <label className="form-label mb-3" htmlFor="type">
          Type
        </label>
        <select
          onChange={handleTransactionChange}
          className="form-control mb-3"
          value={transaction.type}
          required
          id="type"
          name="type"
        >
          <option value="">Select Type</option>
          {types.map((tag, index) => (
            <option key={index} value={tag}>
              {tag}
            </option>
          ))}
        </select>
        <div>
          <div className="mb-5">
            <button className="btn btn-info ms-4" type="submit">
              Submit
            </button>
            <button onClick={handleBack} className="btn btn-dark ms-4">
              Back
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default TransactionCreator;
