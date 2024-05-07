/* eslint-disable react/prop-types */
/* eslint-disable react-hooks/exhaustive-deps */
import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import { tags, types } from "../../../utils/categories";
import InputComponent from "../../FormElements/InputComponent";
import SelectComponent from "../../FormElements/SelectComponent";
import "./TransactionCreator.scss";

const sampleTransaction = {
  date: new Date(),
  name: "",
  amount: 0,
  tag: "",
  type: "",
  accountId: "",
};

const TransactionCreator = ({ pageLoading, setPageLoading, selectedAccountIndex, accounts, setAccounts, setAddingNewTransaction }) => {
  const [transaction, setTransaction] = useState(sampleTransaction);
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
      setPageLoading(true);
      transaction.accountId = accounts[selectedAccountIndex].id;
      transaction.date = new Date();
      console.log(transaction);
      const response = await fetchData(transaction, "/Transaction/add", "POST");
      if (response.ok) {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
        let newAmount;
        if (transaction.type == "Expense") {
          newAmount =
            Number(accounts[selectedAccountIndex].balance) -
            Number(transaction.amount);
        } else {
          newAmount =
            Number(accounts[selectedAccountIndex].balance) +
            Number(transaction.amount);
        }
        setAccounts((prevAccounts) => {
          return prevAccounts.map((account) => {
            if (account.id === accounts[selectedAccountIndex].id) {
              return { ...account, balance: newAmount };
            }
            return account;
          });
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
    setPageLoading(false);
    setTransaction(sampleTransaction);
    setAddingNewTransaction(false);
  };

  if (pageLoading) {
    return <Loading />;
  }

  if (!accounts.length) {
    return;
  }

  return (
    <div className="transaction-creator-container">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <form
        onSubmit={handleCreateTransaction}
        className="transaction-creator-form"
      >
        <h4 className="mb-4">New transaction</h4>
        <div>
          <InputComponent
            text="Name"
            name="name"
            type="text"
            value={transaction.name}
            onChange={handleTransactionChange}
          />
        </div>
        <div>
          <InputComponent
            text="Amount"
            name="amount"
            type="number"
            value={transaction.amount}
            onChange={handleTransactionChange}
          />
        </div>
        <div>
          <SelectComponent
            text="Select Tag"
            id="tag"
            value={transaction.tag}
            array={tags}
            onchange={handleTransactionChange}
          />
        </div>
        <div>
          <SelectComponent
            text="Select Type"
            id="type"
            value={transaction.type}
            array={types}
            onchange={handleTransactionChange}
          />
        </div>
        <button
          className="btn btn-lg transaction-creator-form-btn"
          type="submit"
        >
          Submit
        </button>
      </form>
    </div>
  );
};

export default TransactionCreator;
