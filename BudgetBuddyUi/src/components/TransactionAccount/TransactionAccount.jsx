/* eslint-disable react-hooks/exhaustive-deps */
import { useContext, useEffect, useState } from "react";
import Footer from "../Footer/Footer";
import Navbar from "../Navbar/Navbar";
import "./TransactionAccount.scss";
import { fetchData } from "../../service/connectionService";
import { UserContext } from "../../context/userContext";
import Loading from "../Loading/Loading";
import TransactionSelector from "./TransactionSelector";
import AccountCreator from "../Create/AccountCreator/AccountCreator";

const TransactionAccount = () => {
  const [accounts, setAccounts] = useState([]);
  const [account, setAccount] = useState("");
  const [pageLoading, setPageLoading] = useState(false);
  const { currentUser, loading } = useContext(UserContext);

  const fetchAccounts = async () => {
    setPageLoading(true);
    const response = await fetchData(
      null,
      `/Account/${currentUser.userId}`,
      "GET"
    );
    if (response.ok) {
      const accountList = response.data.data["$values"];
      setAccounts(accountList);
      if (!account) {
        setAccount(accountList[0]);
      }
    }
    setPageLoading(false);
  };

  useEffect(() => {
    if (currentUser) {
      fetchAccounts();
    }
  }, [currentUser]);

  const handleSetAccount = (e) => {
    const id = e.target.value;
    const acc = accounts.find((acc) => acc.id == id);
    setAccount(acc);
  };

  if (loading || pageLoading) {
    return <Loading />;
  }

  return (
    <div className="transaction-container vh-100">
      <Navbar />
      <div className="transaction-content">
        {accounts.length && (
          <TransactionSelector
            account={account}
            setAccount={setAccount}
            accounts={accounts}
            handleSetAccount={handleSetAccount}
          />
        )}
        <div className="container">
          <div className="row">
            <div className="col-md-5">
              <AccountCreator currentUser={currentUser} />
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default TransactionAccount;
