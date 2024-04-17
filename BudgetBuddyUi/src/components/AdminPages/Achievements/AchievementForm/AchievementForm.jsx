import React, { useEffect } from 'react'
import { useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchData } from '../../../../service/connectionService';
import "./AchievementForm.scss";

const AchievementForm = ({
  handleSubmit,
  setAchievementsToAdd,
  achievementsToAdd,
}) => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const { achievementId } = useParams();

    useEffect(() => {
      if (achievementId) {
        const fetchAchievement = async () => {
          const response = await fetchData(
            null,
            `/achievement/${achievementId}`,
            "GET"
          );
          setName(response.data.data.name);
          setDescription(response.data.data.description);
        };
        fetchAchievement();
      }
    }, [achievementId]);

  const addAchievement = (e) => {
    e.preventDefault();
    console.log(achievementsToAdd);
    setAchievementsToAdd((oldAchievements) => [
      ...oldAchievements,
      { name, description },
    ]);
    setName("");
    setDescription("");
  };

  const handleSubmitOnForm = (e) => {
    e.preventDefault();

    const achivementToUpdate = {
      id: achievementId,
      name: name,
      description: description,
    };

    handleSubmit(achivementToUpdate);
  }

  return (
    <div className="ach-form-container">
      <form>
        <div className="mb-3">
          <label for="newName" className="form-label">
            Name
          </label>
          <input
            type="text"
            className="form-control"
            id="newName"
            onChange={(e) => setName(e.target.value)}
            value={name}
          />
        </div>
        <div className="mb-3">
          <label for="newDescription" className="form-label">
            Description
          </label>
          <textarea
            className="form-control"
            id="newDescription"
            onChange={(e) => setDescription(e.target.value)}
            value={description}
          />
        </div>
        <div className="btn-container">
          {achievementId ? (
            ""
          ) : (
            <button className="ach-form-btn" onClick={addAchievement}>
              Add
            </button>
          )}
          <button
            onClick={handleSubmitOnForm}
            type="submit"
            className="ach-form-btn"
          >
            Submit
          </button>
        </div>
      </form>
    </div>
  );
};

export default AchievementForm