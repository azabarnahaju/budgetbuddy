/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import Footer from "../Footer/Footer";
import Navbar from "../Navbar/Navbar";
import { fetchData } from "../../service/connectionService";
import { prepareChartData, stringToDate } from "../../utils/helperFunctions";
import { useParams, useLocation } from "react-router-dom";
import ChartComponent from "../Report/ChartComponent";

const Transactions = () => {
  const [transactions, setTransactions] = useState([]);
  const [transactionsForChart, setTransactionsForChart] = useState([]);
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

      const latestTransaction = selectLatestTransaction(transactionData);
      const transactionDate = new Date(latestTransaction.date);
      const thirtyDaysAgo = new Date(transactionDate);
      thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
      const startDate = thirtyDaysAgo.toISOString().split("T")[0];
      transactionDate.setDate(transactionDate.getDate() + 1);
      const endDate = transactionDate.toISOString().split("T")[0];
      const filteredTransactions = transactionData.filter((t) => {
        const tDate = new Date(t.date);
        return tDate > thirtyDaysAgo;
      });
      setTransactionsForChart(
        prepareChartData(filteredTransactions, startDate, endDate)
      );
      const accountNameParam = searchParams.get("name");
      setAccountName(accountNameParam);
    };
    fetchTransactions();
  }, []);

  const selectLatestTransaction = (transactionData) => {
    const latestTransaction = transactionData.reduce((max, current) => {
      // Parse the dates
      const maxDate = new Date(max.date);
      const currentDate = new Date(current.date);

      // Compare the dates
      if (currentDate > maxDate) {
        return current;
      } else {
        return max;
      }
    }, transactionData[0]);
    return latestTransaction;
  };

  return (
    <div className="transaction-container vh-100">
      <Navbar />
      <div className="transaction-content py-5">
        <div className="text-center">
          <h2>Transactions for your {accountName} account</h2>
        </div>
        <div className="container my-5">
          {transactions.length ? (
            <>
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
              <ChartComponent data={transactionsForChart} />
            </>
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
