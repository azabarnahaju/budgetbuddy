import { useContext, useEffect, useState } from "react";
import SideBar from "../../components/SideBar/SideBar";
import { fetchData } from "../../service/connectionService";
import { UserContext } from "../../context/userContext";
import Loading from "../../components/Loading/Loading";
import TransactionSelector from "../../components/TransactionAccount/TransactionSelector";
import AccountCreator from "../../components/Create/AccountCreator/AccountCreator";
import { useNavigate } from "react-router";
import "./Accounts.scss";
import TransactionTable from "../../components/TransactionTable/TransactionTable";
import Transactions from "../../components/TransactionAccount/Transactions";
import TransactionCreator from "../../components/Create/TransactionCreator/TransactionCreator";
import AccountSelector from "../../components/AccountSelector/AccountSelector";

const Accounts = () => {
  const [accounts, setAccounts] = useState([]);
  const [selectedAccountIndex, setSelectedAccountIndex] = useState(0);
  const [pageLoading, setPageLoading] = useState(false);
  const [transactions, setTransactions] = useState([]);
  const [addingNewAcc, setAddingNewAcc] = useState(false);
  const [addingNewTransaction, setAddingNewTransaction] = useState(false);
  const { currentUser, loading } = useContext(UserContext);
  const navigate = useNavigate();

  const fetchAccounts = async () => {
    setPageLoading(true);
    const response = await fetchData(
      null,
      `/Account/${currentUser.userId}`,
      "GET"
    );
    if (response.ok) {
      const accountList = response.data.data["$values"];
      console.log(accountList)
      setAccounts(accountList);
    }
    setPageLoading(false);
  };

  const fetchTransactions = async () => {
    console.log(accounts);
    if (accounts.length){
        const account = accounts[selectedAccountIndex];
        const response = await fetchData(
          null,
          `/transaction/transactions/account/${account.id}`,
          "GET"
        );

        if (response.ok) {
          console.log(response.data.data.$values);
          setTransactions(response.data.data.$values);
        }
    }
  };

    useEffect(() => {
        if (currentUser) {
        fetchAccounts();
        }
    }, [currentUser]);

    useEffect(() => {
        fetchTransactions();
    }, [pageLoading, accounts, selectedAccountIndex])

  const handleShowTransactions = () => {
    const account = accounts[selectedAccountIndex];
    const accountId = account.id;
    const name = account.name;
    navigate(`/account/transactions/${accountId}?name=${name}`);
  };

  if (loading || pageLoading) {
    return <Loading />;
  }
  if (!loading && !currentUser) {
    navigate("/");
  }


  return (
    <div className="accounts-page vh-100">
      <SideBar />
      <h2 className="accounts-title">Your accounts</h2>
      <div className="accounts-content d-flex justify-content-around">
        <div className="accounts-left-container">
          {accounts.length ? (
            <AccountSelector
              selectedAccountIndex={selectedAccountIndex}
              setSelectedAccountIndex={setSelectedAccountIndex}
              accounts={accounts}
              setAccounts={setAccounts}
              navigate={navigate}
            />
          ) : (
            <div>No accounts yet. Add one! :)</div>
          )}
          <div className="creators">
            <div className="creator-btn-container">
              <button
                onClick={() => setAddingNewAcc(true)}
                className="btn btn-lg btn-outline-dark"
              >
                Create new account
              </button>
              <button
                onClick={() => setAddingNewTransaction(true)}
                className="btn btn-lg btn-outline-dark"
              >
                Add new transaction
              </button>
            </div>
            <div className="creator-container">
              {addingNewAcc && (
                <AccountCreator
                  currentUser={currentUser}
                  setAddingNewAcc={setAddingNewAcc}
                />
              )}
              {addingNewTransaction && (
                <TransactionCreator
                  setPageLoading={setPageLoading}
                  pageLoading={pageLoading}
                  selectedAccountIndex={selectedAccountIndex}
                  accounts={accounts}
                  setAccounts={setAccounts}
                  setAddingNewTransaction={setAddingNewTransaction}
                />
              )}
            </div>
          </div>
        </div>
        <div className="accounts-right-container">
          <h4>Your transactions</h4>
          <TransactionTable
            transactions={transactions}
            message={"No transactions added to this account yet."}
          />
        </div>
      </div>
    </div>
  );
};

export default Accounts;
