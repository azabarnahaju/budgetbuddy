/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import { fetchData } from "../../service/connectionService";
import Loading from "../Loading/Loading";
import SnackBar from "../Snackbar/Snackbar";
import { useParams, useNavigate } from "react-router-dom";

const Achievement = () => {
  const [loading, setLoading] = useState(true);
  const [achievement, setAchievement] = useState({});
  const { id } = useParams();
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  useEffect(() => {
    handleGetAchievements(id);
  }, [id]);

  const handleGetAchievements = async (achievementId) => {
    try {
      const response = await fetchData(
        null,
        `/Achievement/${achievementId}`,
        "GET"
      );
      if (response.ok) {
        console.log(response);
        setAchievement(response.data.data);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setAchievement(false);
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setAchievement(false);
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setLoading(false);
  };

  const handleBack = () => {
    navigate("/");
  }

  if (loading) {
    return <Loading />;
  }

  return (
    <div>
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h2>Achievement</h2>
      {achievement ? (
        <div>
          <h4>Account id: {achievement.id}</h4>
          <h4>Name: {achievement.name}</h4>
          <h4>Description: {achievement.description}</h4>
          <h4>User ids:</h4>
          <div>
            {achievement.users ? achievement.users.map((id) => (
              <div className="mx-5" key={id}>
                <h6>Id: {id}</h6>
              </div>
            )) : <div>No users found.</div>}
          </div>
        </div>
      ) : (
        <h4>Achievement not found</h4>
      )}
      <button onClick={handleBack} className="btn btn-lg btn-dark">Back</button>
    </div>
  );
};

export default Achievement;
