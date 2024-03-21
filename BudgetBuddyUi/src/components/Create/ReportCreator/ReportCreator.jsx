/* eslint-disable react/prop-types */
import { useContext, useEffect, useState } from "react";
import { reportTypes } from "../../../utils/categories";
import { SnackbarContext } from "../../../context/snackbarContext";
import { fetchData } from "../../../service/connectionService";
import Loading from "../../Loading/Loading";
import { UserContext } from "../../../context/userContext";
import { useNavigate } from "react-router-dom";
import SnackBar from "../../Snackbar/Snackbar";

const sampleReport = {
  accountId: "",
  reportType: "",
};

const ReportCreator = () => {
  const [report, setReport] = useState(sampleReport);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
     open: false,
     message: "",
     type: "",
   });

   useEffect(() => {
     setTimeout(() => {
       setLocalSnackbar({
         open: false,
         message: "",
         type: "",
       });
     }, 6000);
   }, [setLocalSnackbar]);

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
    setReport(sampleReport);
  };

  const handleChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setReport({ ...report, [key]: value });
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div>
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <div className="col-md-6 border rounded p-3 mb-5">
        <form onSubmit={handleSubmit}>
          <h2>Choose an account</h2>
          <input
            type="number"
            name="accountId"
            onChange={handleChange}
            value={report.accountId}
            required
          />
          <h2>Choose a type</h2>
          <select
            onChange={handleChange}
            className="form-control mb-3"
            value={report.type}
            required
            id="type"
            name="reportType"
          >
            <option disabled value="">
              Select Report Type
            </option>
            {reportTypes.map((type, index) => (
              <option key={index} value={type}>
                {type}
              </option>
            ))}
          </select>
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
          <button type="submit" className="btn btn-info">
            Create report
          </button>
        </form>
      </div>
      <div>
        <button
          className="btn btn-outline-dark m-2"
          onClick={() => navigate("/reports")}
        >
          Go back
        </button>
      </div>
    </div>
  );
};

export default ReportCreator;
