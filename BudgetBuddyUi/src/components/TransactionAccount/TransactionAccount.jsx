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
import { useNavigate } from "react-router";

const TransactionAccount = () => {
  const [accounts, setAccounts] = useState([]);
  const [selectedAccountIndex, setSelectedAccountIndex] = useState(0);
  const [pageLoading, setPageLoading] = useState(false);
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
      setAccounts(accountList);
    }
    setPageLoading(false);
  };

  useEffect(() => {
    if (currentUser) {
      fetchAccounts();
    }
  }, [currentUser]);

  if (loading || pageLoading) {
    return <Loading />;
  }
  if (!loading && !currentUser) {
    navigate("/");
  }

  return (
    <div className="transaction-container vh-100">
      <Navbar />
      <div className="transaction-content">
        {accounts.length ? (
          <TransactionSelector
            selectedAccountIndex={selectedAccountIndex}
            setSelectedAccountIndex={setSelectedAccountIndex}
            accounts={accounts}
            setAccounts={setAccounts}
          />
        ) : (
          <></>
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
