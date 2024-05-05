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
  const [allAchievementTypes, setAllAchievementTypes] = useState([]);
  const [allObjectives, setAllObjectives] = useState([]);
  const [allTransactionTypes, setAllTransactionTypes] = useState([]);
  const [allTransactionTags, setAllTransactionTags] = useState([]);

  const [name, setName] = useState("");
  const [achievementType, setAchievementType] = useState("");
  const [criteria, setCriteria] = useState();
  const [objective, setObjective] = useState();
  const [transactionType, setTransactionType] = useState(null);
  const [transactionTag, setTransactionTag] = useState(null);
  const [description, setDescription] = useState("");

  const { achievementId } = useParams();
  // fetch select options
  useEffect(() => {
    const fetchAchievementDetails = async () => {
      const typesResponse = await fetchData(
        null,
        "/achievement/achievementtypes",
        "GET"
      );
      const objectiveResponse = await fetchData(
        null,
        "/achievement/achievementobjectives",
        "GET"
      );
      setAllAchievementTypes(typesResponse.data.data);
      setAllObjectives(objectiveResponse.data.data);
      if (!achievementId){
        setAchievementType(typesResponse.data.data[0]);
        setObjective(objectiveResponse.data.data[0]);
      }
    };

    const fetchTransactionAchievementDetails = async () => {
      const typesResponse = await fetchData(
        null,
        "/transaction/transactiontypes",
        "GET"
      );
      const tagResponse = await fetchData(
        null,
        "/transaction/transactiontags",
        "GET"
      );
      setAllTransactionTypes(typesResponse.data.data);
      setAllTransactionTags(tagResponse.data.data);
    };

    fetchAchievementDetails();
    fetchTransactionAchievementDetails();
  }, []);

  // fetch achievement if received achievement ID
  useEffect(() => {
    if (achievementId) {
      const fetchAchievement = async () => {
        const response = await fetchData(
          null,
          `/achievement/${achievementId}`,
          "GET"
        );
        setName(response.data.data.name);
        setAchievementType(response.data.data.type);
        setCriteria(response.data.data.criteria);
        setObjective(response.data.data.objective);
        setTransactionTag(response.data.data.transactionTag);
        setTransactionType(response.data.data.transactionType);
        setDescription(response.data.data.description);
      };
      fetchAchievement();
    }
  }, [achievementId]);

  const handleAchievementTypeChange = e => {
    setAchievementType(e.target.value);

    if (achievementType === "Exploration"){
      setAchievementType(allAchievementTypes[0])
    } else {
      setAchievementType("TransactionType");
    }
  }

  const handleAchievementObjectiveChange = e => {
    setObjective(e.target.value);
    if (achievementType === "TransactionType") {
      setTransactionType(allTransactionTypes[0]);
    } else if (achievementType === "TransactionTag") {
      setTransactionTag(allTransactionTags[0]);
    } else {
      setTransactionTag(null);
      setTransactionType(null);
    }
  }

  // add achievement to be submitted later
  const addAchievement = (e) => {
    e.preventDefault();
    console.log(achievementsToAdd);
    setAchievementsToAdd((oldAchievements) => [
      ...oldAchievements,
      {
        name,
        type: achievementType,
        criteria,
        objective,
        transactionTag,
        transactionType,
      },
    ]);

    setName("");
    setAchievementType(allAchievementTypes[0]);
    setObjective(allObjectives[0]);
    setCriteria(0);
    setTransactionType(allTransactionTypes[0]);
    setTransactionTag(allTransactionTags[0]);
  };

  // submit form
  const handleSubmitOnForm = (e) => {
    e.preventDefault();

    const achivementToUpdate = {
      id: achievementId,
      name,
      description,
      type: achievementType,
      criteria,
      objective,
      transactionTag,
      transactionType,
    };

    handleSubmit(achivementToUpdate);
  };

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
        {achievementId && (
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
        )}
        <div className="mb-3">
          <label for="newAchievementType" className="form-label">
            Achievement Type
          </label>
          <select
            name="newAchievementType"
            value={achievementType}
            onChange={handleAchievementTypeChange}
            className="form-control"
          >
            {allAchievementTypes.map((type) => (
              <option value={type}>{type}</option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label for="newAchievementObjective" className="form-label">
            Achievement Objective
          </label>
          <select
            name="newAchievementObjective"
            value={objective}
            onChange={handleAchievementObjectiveChange}
            className="form-control"
          >
            {achievementType === "Exploration" ? (
              allObjectives.map((obj) => <option value={obj}>{obj}</option>)
            ) : (
              <>
                <option value="TransactionType">Transaction Type</option>
                <option value="TransactionTag">Transaction Tag</option>
              </>
            )}
          </select>
        </div>
        <div className="mb-3">
          <label htmlFor="achievementCriteria" className="form-label">
            Criteria
          </label>
          <input
            name="achievementCriteria"
            value={criteria}
            onChange={(e) => setCriteria(e.target.value)}
            className="form-control"
            type="number"
            min={0}
          />
        </div>
        {objective === "TransactionType" && (
          <div className="mb-3">
            <label for="newTransactionType" className="form-label">
              Transaction Type
            </label>
            <select
              name="newTransactionType"
              value={transactionType}
              onChange={(e) => setTransactionType(e.target.value)}
              className="form-control"
            >
              {allTransactionTypes.map((type) => (
                <option value={type}>{type}</option>
              ))}
            </select>
          </div>
        )}
        {objective === "TransactionTag" && (
          <div className="mb-3">
            <label for="newTransactionTag" className="form-label">
              Transaction Tag
            </label>
            <select
              name="newTransactionTag"
              value={transactionTag}
              onChange={(e) => setTransactionTag(e.target.value)}
              className="form-control"
            >
              {allTransactionTags.map((tag) => (
                <option value={tag}>{tag}</option>
              ))}
            </select>
          </div>
        )}

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