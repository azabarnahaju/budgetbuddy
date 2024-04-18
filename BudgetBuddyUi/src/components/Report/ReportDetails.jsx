/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { fetchData } from "../../service/connectionService";
import { useState } from "react";
import { stringToDate } from "../../utils/helperFunctions";
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import "./ReportDetails.scss";
import ChartComponent from "./ChartComponent";

const ReportDetails = () => {
  const { reportId } = useParams();
  const [report, setReport] = useState(null);
  const [loading, setLoading] = useState(false);
  const [chartData, setChartData] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const fetchReport = async () => {
      const report = await fetchData(null, `/report/${reportId}`, "GET");
      const reportData = report.data.data;
      const startDate = reportData.startDate.split("T")[0];
      const endDateString = reportData.endDate.split("T")[0];
      let endDate = new Date(endDateString);
      endDate.setDate(endDate.getDate() + 1);
      endDate = endDate.toISOString().split("T")[0];
      const fetchedTransactions = await fetchData(
        null,
        `/transaction/transactions/account/${reportData.accountId}?startDate=${startDate}&endDate=${endDate}`,
        "GET"
      );
      const transactionData = fetchedTransactions.data.data["$values"];
      const extractedData = extractTransactionData(transactionData);
      setChartData(prepareChartData(extractedData, startDate, endDate));
      setReport(report.data.data);
    };
    fetchReport();
    setLoading(false);
  }, []);

  const extractTransactionData = (data) => {
    const extractedData = [];
    data.forEach((element) => {
      extractedData.push({
        amount: element.amount,
        type: element.type,
        date: element.date.split("T")[0],
      });
    });

    return extractedData;
  };

  const prepareChartData = (data, startDate, endDate) => {
    const chartData = {
      labels: [],
      datasets: [
        {
          label: "Expense",
          backgroundColor: "rgba(255, 99, 132, 0.6)",
          borderColor: "rgba(255, 99, 132, 1)",
          borderWidth: 1,
          hoverBackgroundColor: "rgba(255, 99, 132, 0.8)",
          hoverBorderColor: "rgba(255, 99, 132, 1)",
          data: [],
        },
        {
          label: "Income",
          backgroundColor: "rgba(75, 192, 192, 0.6)",
          borderColor: "rgba(75, 192, 192, 1)",
          borderWidth: 1,
          hoverBackgroundColor: "rgba(75, 192, 192, 0.8)",
          hoverBorderColor: "rgba(75, 192, 192, 1)",
          data: [],
        },
      ],
    };

    for (
      let date = new Date(startDate);
      date <= new Date(endDate);
      date.setDate(date.getDate() + 1)
    ) {
      const dateString = date.toISOString().split("T")[0];
      chartData.labels.push(dateString);

      let totalIncome = 0;
      let totalExpense = 0;

      data.forEach((entry) => {
        if (entry.date === dateString) {
          if (entry.type === "Income") {
            totalIncome += entry.amount;
          } else {
            totalExpense += entry.amount;
          }
        }
      });

      // Add data to datasets
      chartData.datasets[0].data.push(-totalExpense);
      chartData.datasets[1].data.push(totalIncome);
    }

    return chartData;
  };

  if (loading) {
    return <>LOADING</>;
  }

  return (
    <div className="report-detail-container vh-100">
      <Navbar />
      {report ? (
        <>
          <div className="report-content d-flex justify-content-center">
            <div className="border border-dark-subtle rounded m-5 text-center p-4">
              <h3 className="mt-3 mb-5">
                Your {report.reportType.toLowerCase()} report from{" "}
                {stringToDate(report.startDate)} to{" "}
                {stringToDate(report.endDate)}
              </h3>
              <table className="table table-info table-sm">
                <tbody>
                  <tr>
                    <td>Account ID</td>
                    <td>{report.accountId}</td>
                  </tr>
                  <tr>
                    <td>Daily average spending</td>
                    <td>${report.averageSpendingDaily}</td>
                  </tr>
                  <tr>
                    <td>Average spending by transaction</td>
                    <td>${report.averageSpendingTransaction}</td>
                  </tr>
                  <tr>
                    <td>Top expense</td>
                    <td>${report.biggestExpense}</td>
                  </tr>
                  <tr>
                    <td>Categories</td>
                    <td>
                      {report.categories.$values.map((c, i) => (
                        <span key={`${i}category`}>{c} </span>
                      ))}
                    </td>
                  </tr>
                  <tr>
                    <td>Day with the most spending</td>
                    <td>{stringToDate(report.mostSpendingDay)}</td>
                  </tr>
                  <tr>
                    <td>Tag with the most expenses</td>
                    <td>{report.mostSpendingTag}</td>
                  </tr>
                  <tr>
                    <td>Sum of expenses</td>
                    <td>${report.sumExpense}</td>
                  </tr>
                  <tr>
                    <td>Sum of income</td>
                    <td>${report.sumIncome}</td>
                  </tr>
                </tbody>
              </table>
              <h5 className="mt-5 mb-3">Your expenses by tags</h5>
              <div className="d-flex justify-content-center">
                <table className="text-center table table-info table-xsm w-50">
                  <thead>
                    <tr>
                      <th scope="col">Tag</th>
                      <th scope="col">Amount</th>
                    </tr>
                  </thead>
                  <tbody>
                    {Object.entries(report.spendingByTags).map(
                      (item, i) =>
                        i > 0 &&
                        item[0] !== "Income" && (
                          <tr key={`tableinfo${i}`}>
                            <td>{item[0]}</td>
                            <td>{item[1]}</td>
                          </tr>
                        )
                    )}
                  </tbody>
                </table>
              </div>
              <div>Report created at {stringToDate(report.createdAt)}</div>
              <div className="mt-5">
              <ChartComponent data={chartData} />
              </div>
              <button
                className="btn btn-outline-light m-5"
                onClick={() => navigate("/reports")}
              >
                Go back
              </button>
            </div>
          </div>
        </>
      ) : (
        <>ERROR!!</>
      )}
      <Footer />
    </div>
  );
};

export default ReportDetails;
