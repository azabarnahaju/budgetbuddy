/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import Footer from "../Footer/Footer";
import Navbar from "../Navbar/Navbar";
import { fetchData } from "../../service/connectionService";
import { stringToDate } from "../../utils/helperFunctions";
import { useParams, useLocation } from "react-router-dom";

const Transactions = () => {
  const [transactions, setTransactions] = useState([]);
  const [accountName, setAccountName] = useState("");
  const { id } = useParams();
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);

  useEffect(() => {
    const fetchTransactions = async () => {
      const fetchedTransactions = await fetchData(
        null,
        `/transaction/transactions/account/${id}`,
        "GET"
      );
      const transactionData = fetchedTransactions.data.data["$values"];
      setTransactions(transactionData);
      const accountNameParam = searchParams.get("name");
      setAccountName(accountNameParam);
    };
    fetchTransactions();
  }, []);

  return (
    <div className="transaction-container vh-100">
      <Navbar />
      <div className="transaction-content py-5">
        <div className="text-center">
          <h2>Transactions for your {accountName} account</h2>
        </div>
        <div className="container my-5">
          {transactions.length ? (
            <div className="d-flex justify-content-center m-5 mt-0 table-responsive">
              <table className="table table-dark align-middle table-hover">
                <thead className="table-success">
                  <tr>
                    <th scope="col">Date</th>
                    <th className="text-center" scope="col">
                      Amount
                    </th>
                    <th className="text-center" scope="col">
                      Name
                    </th>
                    <th className="text-center" scope="col">
                      Tag
                    </th>
                    <th className="text-center" scope="col">
                      Type
                    </th>
                    <th></th>
                  </tr>
                </thead>
                <tbody className="table-group-divider">
                  {transactions.map((t) => {
                    return (
                      <tr key={t.id}>
                        <th className="table-info" scope="row">
                          {stringToDate(t.date)}
                        </th>
                        <td className="text-center">{t.amount}</td>
                        <td className="text-center">{t.name}</td>
                        <td className="text-center">{t.tag}</td>
                        <td className="text-center">{t.type}</td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="text-center">
              <h2>You have no transactions yet!</h2>
            </div>
          )}
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Transactions;
