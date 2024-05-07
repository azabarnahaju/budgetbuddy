import { useContext, useEffect, useState } from "react";
import { fetchData } from "../../service/connectionService";
import { UserContext } from "../../context/userContext";
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import SnackBar from "../Snackbar/Snackbar";
import Loading from "../Loading/Loading";
import SideBar from "../SideBar/SideBar";

import "./AchievementDashBoard.scss";
import ProgressBar from "./ProgressBar";

const AchievementDashBoard = () => {
  const { currentUser, loading } = useContext(UserContext);
  const [allAchievements, setAllAchievements] = useState([]);
  const [userAchievements, setUserAchievements] = useState([]);
  const [pageLoading, setPageLoading] = useState(false);
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const fetchAllAchievements = async () => {
    setPageLoading(true);
    try {
      const response = await fetchData(null, "/Achievement", "GET");
      if (response.ok) {
        const achievements = response.data.data["$values"];
        setAllAchievements(achievements);
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "The server not responding",
        type: "error",
      });
    }
    setPageLoading(false);
  };

  const fetchUserAchievements = async () => {
    setPageLoading(true);
    console.log(loading)
    console.log(currentUser)
    const response = await fetchData(
      null,
      `/Achievement/user/${currentUser.userId}`,
      "GET"
    );
    if (response.ok) {
      const achievements = response.data.data["$values"];
      setUserAchievements(achievements);
    }
    setPageLoading(false);
  };

  useEffect(() => {
    fetchAllAchievements();
    if (currentUser) {
      fetchUserAchievements();
    }
  }, [currentUser]);

  if (loading || pageLoading) {
    return <Loading />;
  }

  return (
    <div className="achievement-container vh-100">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <SideBar />
      <div className="achievement-content">
        <div className="achievement-title text-center mt-5">
          <h1>Achievements</h1>
        </div>
        <div className="achievement-title user-achievement-container my-5">
          <h3>
            Your Achievements: {userAchievements.length}/
            {allAchievements.length}
          </h3>
          <ProgressBar
            allAchievements={allAchievements}
            userAchievements={userAchievements}
          />
          <div className="row">
            {userAchievements.map((a, index) => (
              <div className="col-md-2 mb-1" key={index}>
                <div className="border rounded p-3">
                  <h4>{a.name}</h4>
                  <h6>
                    <i>{a.description}</i>
                  </h6>
                </div>
              </div>
            ))}
          </div>
          <hr />
          <h3 onClick={() => console.log("asd")}>All Achievements:</h3>
          <div className="row">
            {allAchievements
              .sort((a, b) => {
                if (userAchievements.find((au) => au.id === a.id)) return 1;
                else if (userAchievements.find((au) => au.id === b.id))
                  return -1;
                else return 0;
              })
              .map((a, index) =>
                userAchievements.find((au) => au.id === a.id) ? (
                  <div className="col-md-2 mb-1" key={index}>
                    <div className="border rounded p-3">
                      <h4>{a.name}</h4>
                      <h6>
                        <i>{a.description}</i>
                      </h6>
                    </div>
                  </div>
                ) : (
                  <div className="col-md-2 mb-2" key={index}>
                    <div
                      className="border rounded p-3"
                      style={{
                        backgroundColor: "lightgray",
                        color: "gray",
                        pointerEvents: "none",
                      }}
                    >
                      <h4>{a.name}</h4>
                      <h6>
                        <i>{a.description}</i>
                      </h6>
                    </div>
                  </div>
                )
              )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AchievementDashBoard;
