import React, { useState, useEffect } from 'react'
import SideBar from '../../components/SideBar/SideBar'
import { UserContext } from '../../context/userContext';
import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import OverviewLineChart from '../../components/Charts/OverviewLineChart/OverviewLineChart';
import DashboardSideChart from '../../components/Charts/DashboardSideChart/DashboardSideChart';
import TransactionTable from '../../components/TransactionTable/TransactionTable';
import "./Dashboard.scss";
import { fetchData } from '../../service/connectionService';
import { getOverviewChartData, getBalance } from "../../utils/chartData";


const Dashboard = () => {
  const { currentUser } = useContext(UserContext);
  const [balance, setBalance] = useState(0);
  const [transactions, setTransactions] = useState(null);
  const [latestTransactions, setLatestTransactions] = useState(null);
  const [incomeTransactions, setIncomeTransactions] = useState(null);
  const [expenseTransactions, setExpenseTransactions] = useState(null);
  const [accounts, setAccounts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!currentUser){
      return;
    }

    const fetchAccounts = async () => {
      const response = await fetchData(null, `/account/${currentUser.userId}`, "GET");
      if (response.ok) {
        setAccounts(response.data.data.$values);
        setBalance(getBalance(response.data.data.$values))
      }
    }

    const fetchTransactions = async () => {
      const responseLatest = await fetchData(
        null,
        `/Transaction/transactions/user/${currentUser.userId}?count=4`,
        "GET"
      );
      const responseAll = await fetchData(
        null,
        `/Transaction/transactions/user/${currentUser.userId}`,
        "GET"
      );
      
      if (responseLatest.ok) {
        setLatestTransactions(responseLatest.data.data.$values);
      } 

      if (responseAll.ok) {
        setTransactions(responseAll.data.data.$values);
        setIncomeTransactions(
          responseAll.data.data.$values.filter((t) => t.type === "Income")
        );
        setExpenseTransactions(
          responseAll.data.data.$values.filter((t) => t.type === "Expense")
        );
      }

      if (responseLatest.message === "Transactions not found.") {
        setLatestTransactions([]);
      }

      if (responseAll.message === "Transactions not found.") {
        setTransactions([])
        setIncomeTransactions([]);
        setExpenseTransactions([]);
      }
    };

    fetchAccounts();
    fetchTransactions();

  }, [currentUser]);

  if (!currentUser) {
    if (!localStorage.getItem("accessToken")) {
      navigate("/");
      return;
    }
    return <div>Loading</div>;
  }

  if (latestTransactions === null || transactions === null) {
    return <div>Loading</div>;
  }

  return (
    <div className="d-flex dashboard">
      <SideBar />
      <div className="dashboard-content ms-5">
        <div className="db-top-bar d-flex justify-content-around flex-wrap">
          <div className="db-main-content">
            <h2 className="dashboard-title mb-3">Overview - Last 7 days</h2>
            <div className="balance-container">
              <h5 className="db-chart-title">Total balance:</h5>
              <h4 className="db-chart-title">${balance}</h4>
              <OverviewLineChart
                transactions={transactions}
                balance={balance}
              />
            </div>

            <div className="latest-transactions-container">
              <h3>Latest transactions</h3>
              <TransactionTable transactions={latestTransactions} message={"No transactions for this period"}/>
            </div>
          </div>
          <div className="db-side-content">
            <h2 className="welcome-msg">Welcome {currentUser.username}!</h2>
            <div className="income-container">
              <h4 className="db-chart-title">Income</h4>
              <DashboardSideChart transactions={incomeTransactions} />
            </div>
            <div className="expenses-container">
              <h4 className="db-chart-title">Expenses</h4>
              <DashboardSideChart transactions={expenseTransactions} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Dashboard