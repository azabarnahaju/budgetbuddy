/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { fetchData } from "../../service/connectionService";
import { useState } from "react";
import { stringToDate } from "../../utils/helperFunctions";
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import "./ReportDetails.scss";

const ReportDetails = () => {
  const { reportId } = useParams();
  const [report, setReport] = useState(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const fetchReport = async () => {
      const report = await fetchData(null, `/report/${reportId}`, "GET");
      setReport(report.data.data);
    };
    fetchReport();
    setLoading(false);
  }, []);

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
