/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import Footer from "../Footer/Footer";
import Navbar from "../Navbar/Navbar";
import { fetchData } from "../../service/connectionService";
import { prepareChartData, stringToDate } from "../../utils/helperFunctions";
import { useParams, useLocation } from "react-router-dom";
import ChartComponent from "../Report/ChartComponent";
import TransactionTable from "../TransactionTable/TransactionTable";

const Transactions = ({ account, pageLoading }) => {
  // const [transactionsForChart, setTransactionsForChart] = useState([]);
  // // const { id } = useParams();
  // const location = useLocation();
  // const searchParams = new URLSearchParams(location.search);

  useEffect(() => {
    
    fetchTransactions();
  }, [pageLoading]);

  // useEffect(() => {
  //   const fetchTransactions = async () => {
  //     const fetchedTransactions = await fetchData(
  //       null,
  //       `/transaction/transactions/account/${id}`,
  //       "GET"
  //     );
  //     const transactionData = fetchedTransactions.data.data["$values"];
  //     setTransactions(transactionData);

  //     const latestTransaction = selectLatestTransaction(transactionData);
  //     const transactionDate = new Date(latestTransaction.date);
  //     const thirtyDaysAgo = new Date(transactionDate);
  //     thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
  //     const startDate = thirtyDaysAgo.toISOString().split("T")[0];
  //     transactionDate.setDate(transactionDate.getDate() + 1);
  //     const endDate = transactionDate.toISOString().split("T")[0];
  //     const filteredTransactions = transactionData.filter((t) => {
  //       const tDate = new Date(t.date);
  //       return tDate > thirtyDaysAgo;
  //     });
  //     setTransactionsForChart(
  //       prepareChartData(filteredTransactions, startDate, endDate)
  //     );
  //     const accountNameParam = searchParams.get("name");
  //     setAccountName(accountNameParam);
  //   };
  //   fetchTransactions();
  // }, []);

  // const selectLatestTransaction = (transactionData) => {
  //   const latestTransaction = transactionData.reduce((max, current) => {
  //     // Parse the dates
  //     const maxDate = new Date(max.date);
  //     const currentDate = new Date(current.date);

  //     // Compare the dates
  //     if (currentDate > maxDate) {
  //       return current;
  //     } else {
  //       return max;
  //     }
  //   }, transactionData[0]);
  //   return latestTransaction;
  // };
  if (!account) {
    <div className="transaction-container"></div>;
  }

  return (
    <div className="transaction-container">
      <div className="text-center">
        <h2>Transactions for your {account.name} account</h2>
        <TransactionTable
          transactions={transactions}
          message={"No transactions for this account yet."}
        />
      </div>
    </div>
  );
};

export default Transactions;
