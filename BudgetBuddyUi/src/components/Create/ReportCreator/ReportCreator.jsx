/* eslint-disable react/prop-types */
import { useContext, useEffect, useState } from "react";
import { reportTypes } from "../../../utils/categories";
import { SnackbarContext } from "../../../context/snackbarContext";
import { fetchData } from "../../../service/connectionService";
import Loading from "../../Loading/Loading";
import { UserContext } from "../../../context/userContext";
import { useNavigate } from "react-router-dom";
import SnackBar from "../../Snackbar/Snackbar";
import SelectComponent from "../../FormElements/SelectComponent";
import Navbar from "../../Navbar/Navbar";
import Footer from "../../Footer/Footer";
import SideBar from "../../SideBar/SideBar";
import "./ReportCreator.scss";

const sampleReport = {
  accountId: "",
  reportType: reportTypes[0],
};

const ReportCreator = () => {
  const [accounts, setAccounts] = useState([]);
  const [account, setAccount] = useState("");
  const [report, setReport] = useState(sampleReport);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { currentUser } = useContext(UserContext);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const fetchAccounts = async () => {
    setLoading(true);
    const response = await fetchData(
      null,
      `/Account/${currentUser.userId}`,
      "GET"
    );
    if (response.ok) {
      const accountList = response.data.data["$values"];
      setAccounts(accountList);
      console.log(accountList);
      if (account == "" || !account) {
        setAccount(accountList[0]);
        setReport({ ...report, ["accountId"]: accountList[0].id });
      }
    }
    setLoading(false);
  };

  useEffect(() => {
    if (currentUser) {
      fetchAccounts();
    }
  }, [currentUser]);

  useEffect(() => {
    setTimeout(() => {
      setLocalSnackbar({
        open: false,
        message: "",
        type: "",
      });
    }, 6000);
  }, [setLocalSnackbar]);

  const handleSetAccount = (e) => {
    const id = e.target.value;
    const acc = accounts.find((acc) => acc.id == id);
    setAccount(acc);
    setReport({ ...report, ["accountId"]: acc.id });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    console.log(report);
    try {
      setLoading(true);
      const response = await fetchData(report, "/Report/Add", "POST");
      console.log(response);
      if (response.ok) {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setLoading(false);
    setReport({ accountId: accounts[0].id, reportType: reportTypes[0] });
  };

  const handleChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setReport({ ...report, [key]: value });
  };

  if (loading) {
    return <Loading />;
  }
  if (!loading && !currentUser) {
    navigate("/");
  }

  return (
    <div className="report-detail-container vh-100">
      <SideBar />
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <div className="report-creator-content d-flex justify-content-center py-5 text-center">
        <div className="report-form-container col-md-6 p-5">
          <form onSubmit={handleSubmit}>
            <h3 className="report-creator-form-title">Choose an account</h3>
            <select
              onChange={handleSetAccount}
              className="form-control mb-3"
              value={account.id}
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
            <h3 className="report-creator-form-title">Choose a type</h3>
            <SelectComponent
              text="Select Report Type"
              id="reportType"
              value={report.type}
              array={reportTypes}
              onchange={handleChange}
            />
            <label htmlFor="target">Start date (optional)</label>
            <input
              id="startDate"
              name="startDate"
              value={report.startDate}
              className="form-control mb-3"
              type="date"
              onChange={handleChange}
            />
            <label htmlFor="target">End date (optional)</label>
            <input
              id="endDate"
              name="endDate"
              value={report.endDate}
              className="form-control mb-3"
              type="date"
              onChange={handleChange}
            />
            <button type="submit" className="btn report-creator-submit-btn">
              Create report
            </button>
          </form>
          <div>
            <button
              className="btn report-creator-back-btn m-2"
              onClick={() => navigate("/reports")}
            >
              Go back
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ReportCreator;
