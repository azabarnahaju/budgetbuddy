/* eslint-disable react-hooks/exhaustive-deps */
import { useContext, useEffect, useState } from "react";
import Footer from "../Footer/Footer";
import Navbar from "../Navbar/Navbar";
import "./TransactionAccount.scss";
import { fetchData } from "../../service/connectionService";
import { UserContext } from "../../context/userContext";
import Loading from "../Loading/Loading";

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
    const acc = accounts.find(acc => acc.id == id);
    setAccount(acc);
  };

  if (loading || pageLoading) {
    return <Loading />;
  }

  return (
    <div className="transaction-container vh-100">
      <Navbar />
      <div className="transaction-content">
        <div className="container mt-5">
          <div className="container mt-5">
            <h3>Select your account</h3>
            <div className="row">
              <div className="col-md-4">
                <select
                  onChange={handleSetAccount}
                  className="form-control mb-3"
                  value={account ? account.id : ""}
                  required
                  id="account"
                  name="account"
                >
                  {accounts.map((acc) => (
                    <option key={acc.id} value={acc.id}>
                      {acc.name}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <h5>{account.name}</h5>
              </div>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default TransactionAccount;
