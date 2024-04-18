import React from 'react'
import { useEffect, useState } from 'react'
import { fetchData } from '../../../../service/connectionService';
import { useNavigate } from 'react-router-dom';
import SnackBar from '../../../Snackbar/Snackbar';
import AdminNavbar from '../../AdminNavbar/AdminNavbar';
import Loading from '../../../Loading/Loading';
import "./AchievementList.scss";

const AchievementList = () => {
    const [achievements, setAchievements] = useState("");
    const [localLoading, setLocalLoading] = useState(false);
    const [localSnackbar, setLocalSnackbar] = useState({
      open: false,
      message: "",
      type: "",
    });
    const navigate = useNavigate();

    useEffect(() => {
        const fetchAchievements = async () => {
          setLocalLoading(true)
            const result = await fetchData(null, "/achievement", "GET");
            setAchievements(result.data.data.$values);
            setLocalLoading(false);
        }
        fetchAchievements();
    }, [])

    const handleDeleteAchievement = async (id) => {
      try {
        setLocalLoading(true);
        const response = await fetchData(
          null,
          `/achievement/delete/${id}`,
          "DELETE"
        );
        
        if (response.ok) {
          setLocalSnackbar({
            open: true,
            message: response.message,
            type: "success",
          });
          setAchievements((oldAchievements) => oldAchievements.filter(a => a.id != id))
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
    setLocalLoading(false)
    }

    if (localLoading) {
      return <Loading />
    }

  return (
    <div className="admin-page">
      <AdminNavbar />
      <div className="achievement-list-container">
        <div className="admin-title">All achievements</div>
        <SnackBar
          {...localSnackbar}
          setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
        />
        <div className="achievement-content">
          {achievements.length >= 1 ? (
            <table className="table table-dark align-middle table-hover achievements-table">
              <thead className="table-success">
                <tr>
                  <th scope="col">#</th>
                  <th className="text-center" scope="col">
                    Title
                  </th>
                  <th className="text-center" scope="col">
                    Description
                  </th>
                  <th className="text-center" scope="col"></th>
                  <th className="text-center" scope="col"></th>
                </tr>
              </thead>
              <tbody className="table-group-divider">
                {achievements.map((a) => (
                  <tr>
                    <th className="table-info">{a.id}</th>
                    <td className="text-center">{a.name}</td>
                    <td className="text-center">{a.description}</td>
                    <td className="text-center">
                      <button
                        className="btn btn-outline-light me-5"
                        onClick={() => navigate(`/achievements/update/${a.id}`)}
                      >
                        Edit
                      </button>
                    </td>
                    <td className="text-center">
                      <button
                        className="btn btn-outline-light me-5"
                        onClick={() => handleDeleteAchievement(a.id)}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <div>No achievements added yet.</div>
          )}
          <button
            className="btn btn-outline-light me-5"
            onClick={() => navigate("/achievements/create")}
          >
            Create new
          </button>
        </div>
      </div>
    </div>
  );
}

export default AchievementList