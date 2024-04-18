import React, { useEffect, useState } from "react";
import AchievementForm from "../Achievements/AchievementForm/AchievementForm";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import { useNavigate } from "react-router-dom";
import AdminNavbar from "../AdminNavbar/AdminNavbar";
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
      <AdminNavbar />
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      {achievementsToAdd.length === 0 ? (
        ""
      ) : (
        <div className="add-ach-container">
          <h2 className="mb-4">Achievements to be submitted</h2>
          <table className="text-center table mx-5 table-responsive">
            <thead className="table-success">
              <tr>
                <th>Name</th>
                <th>Description</th>
              </tr>
            </thead>
            <tbody>
              {achievementsToAdd.map((a) => (
                <tr>
                  <td className="text-center">{a.name}</td>
                  <td className="text-center">{a.description}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
      <AchievementForm
        handleSubmit={handleSubmit}
        setAchievementsToAdd={setAchievementsToAdd}
        achievementsToAdd={achievementsToAdd}
        setAchievementToEdit={null}
      />
    </div>
  );
};

export default CreateAchievement;
