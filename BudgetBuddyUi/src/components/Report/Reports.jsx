import Navbar from "../Navbar/Navbar";
import Loading from "../Loading/Loading";
import { useContext, useEffect, useState } from "react";
import { fetchData } from "../../service/connectionService";
import { UserContext } from "../../context/userContext";
import { useNavigate } from "react-router-dom";
import { stringToDate } from "../../utils/helperFunctions";
import Footer from "../Footer/Footer";
import "./Reports.scss";

const Reports = () => {
  const [reports, setReports] = useState([]);
  const [pageLoading, setPageLoading] = useState(false);
  const { currentUser, loading } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    setPageLoading(true);
    const fetchReports = async () => {
      const result = await fetchData(
        null,
        `/report/report/user/${currentUser.userId}`,
        "GET"
      );
      setReports(result.data.data.$values);
    };
    if (currentUser) {
      fetchReports();
    }
    setPageLoading(false);
  }, [currentUser]);

  if (pageLoading || loading) {
    return <Loading message="Logging in..." />;
  }

  return (
    <div className="reports-container vh-100">
      <Navbar />
      <div className="report-content p-5 pb-0">
        {reports ? (
          <div className="d-flex justify-content-center m-5 mt-0 table-responsive">
            <table className="table table-dark align-middle table-hover">
              <thead className="table-success">
                <tr>
                  <th scope="col">Created</th>
                  <th className="text-center" scope="col">
                    Type
                  </th>
                  <th className="text-center" scope="col">
                    Account
                  </th>
                  <th className="text-center" scope="col">
                    From
                  </th>
                  <th className="text-center" scope="col">
                    End
                  </th>
                  <th></th>
                </tr>
              </thead>
              <tbody className="table-group-divider">
                {reports.map((r) => {
                  return (
                    <tr key={r.id}>
                      <th className="table-info" scope="row">
                        {stringToDate(r.createdAt)}
                      </th>
                      <td className="text-center">{r.reportType}</td>
                      <td className="text-center">{r.accountId}</td>
                      <td className="text-center">
                        {stringToDate(r.startDate)}
                      </td>
                      <td className="text-center">{stringToDate(r.endDate)}</td>
                      <td className="text-center">
                        <button
                          className="btn btn-outline-light"
                          onClick={() => navigate(`/reports/${r.id}`)}
                        >
                          View details
                        </button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        ) : (
          <div>You have no reports yet!</div>
        )}
        <div className="d-flex justify-content-end">
          <button
            className="btn btn-outline-light me-5"
            onClick={() => navigate("/reports/add")}
          >
            Generate new report
          </button>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Reports;
