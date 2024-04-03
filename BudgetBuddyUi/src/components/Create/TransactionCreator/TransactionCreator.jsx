/* eslint-disable react/prop-types */
/* eslint-disable react-hooks/exhaustive-deps */
import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import { tags, types } from "../../../utils/categories";
import InputComponent from "../../FormElements/InputComponent";
import SelectComponent from "../../FormElements/SelectComponent";

const sampleTransaction = {
  date: new Date(),
  name: "",
  amount: 0,
  tag: "",
  type: "",
  accountId: "",
};

const TransactionCreator = ({ account, setAccount }) => {
  const [transaction, setTransaction] = useState(sampleTransaction);
  const [loading, setLoading] = useState(false);
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

  const handleCreateTransaction = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      transaction.accountId = account.id;
      const response = await fetchData(transaction, "/Transaction/add", "POST");
      if (response.ok) {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
        if (transaction.type == "Expense") {
          const newAmount =
            Number(account.balance) - Number(transaction.amount);
          setAccount({
            ...account,
            balance: `${newAmount}`,
          });
        } else {
          const newAmount =
            Number(account.balance) + Number(transaction.amount);
          setAccount({
            ...account,
            balance: `${newAmount}`,
          });
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
    <div className="container mt-1">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <form onSubmit={handleCreateTransaction} className="rounded border p-4">
        <h3 className="my-2">New transaction on {account.name}</h3>
        <div className="mb-3">
          <InputComponent
            text="Name"
            name="name"
            type="text"
            value={transaction.name}
            onChange={handleTransactionChange}
          />
        </div>
        <div className="mb-3">
          <InputComponent
            text="Amount"
            name="amount"
            type="number"
            value={transaction.amount}
            onChange={handleTransactionChange}
          />
        </div>
        <div className="my-4">
          <SelectComponent
            text="Select Tag"
            id="tag"
            value={transaction.tag}
            array={tags}
            onchange={handleTransactionChange}
          />
        </div>
        <div className="my-4">
          <SelectComponent
            text="Select Type"
            id="type"
            value={transaction.type}
            array={types}
            onchange={handleTransactionChange}
          />
        </div>
        <div>
          <div className="mb-3">
            <button className="btn btn-lg btn-outline-light" type="submit">
              Submit
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default TransactionCreator;
