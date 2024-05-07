import React, { useEffect, useState } from "react";
import AchievementForm from "../Achievements/AchievementForm/AchievementForm";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import { useNavigate } from "react-router-dom";
import AdminNavbar from "../AdminNavbar/AdminNavbar";
import SideBar from "../../SideBar/SideBar";
import "./CreateAchievement.scss";

const CreateAchievement = () => {
  const [achievementsToAdd, setAchievementsToAdd] = useState([]);
  const [localLoading, setLocalLoading] = useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });
  const navigate = useNavigate();
  
  const handleRemoveFromAddTable = (name) => {
    setAchievementsToAdd(currAchievements => currAchievements.filter(a => a.name !== name));
  }

  const handleSubmit = async () => {
    try {
      setLocalLoading(true);
      console.log(achievementsToAdd)
      const response = await fetchData(achievementsToAdd, "/achievement/add", "POST");
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
    setLocalLoading(false);
    setAchievementsToAdd([]);
    navigate("/admin")
  };

  if (localLoading) {
    return <Loading />
  }

  return (
    <div className="create-ach-container">
      <SideBar />
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <div className="create-ach-content">
        <div>
          <h3 className="text-center mt-5 create-ach-title">New achievement</h3>
          <AchievementForm
            handleSubmit={handleSubmit}
            setAchievementsToAdd={setAchievementsToAdd}
            achievementsToAdd={achievementsToAdd}
            setAchievementToEdit={null}
          />
        </div>
        {achievementsToAdd.length !== 0 && (
          <div className="add-ach-container">
            <h2 className="mb-4">Achievements to be submitted</h2>
            <table className="text-center table mx-5 table-responsive">
              <thead className="table-success">
                <tr>
                  <th>Name</th>
                  <th>Achievement Type</th>
                  <th>Achievement Objective</th>
                  <th>Criteria</th>
                  <th>Transaction Tag</th>
                  <th>Transaction Type</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {achievementsToAdd.map((a) => (
                  <tr>
                    <td className="text-center">{a.name}</td>
                    <td className="text-center">{a.type}</td>
                    <td className="text-center">{a.objective}</td>
                    <td className="text-center">{a.criteria}</td>
                    <td className="text-center">{a.transactionTag}</td>
                    <td className="text-center">{a.transactionType}</td>
                    <td className="text-center">
                      <button
                        className="btn add-ach-remove-btn"
                        onClick={() => handleRemoveFromAddTable(a.name)}
                      >
                        Remove
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <button type="submit" form="new-ach-form" className="ach-form-btn">
              Submit
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default CreateAchievement;
