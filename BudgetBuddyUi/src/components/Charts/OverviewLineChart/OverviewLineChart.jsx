import React from "react";
import { useState, useEffect } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import "./OverviewLineChart.scss";
import { getOverviewChartData } from "../../../utils/chartData";
import "../../../Style/designVariables.scss";

const OverviewLineChart = ({ transactions, balance }) => {
    const [data, setData] = useState(null)

    useEffect(() => {
        const newData = getOverviewChartData(transactions, balance);
        setData(newData);
    }, [])

    if (!data){
        return <div>Loading</div>;
    }

  return (
    <div className="overview-linechart-container">
      <ResponsiveContainer>
        <LineChart
          data={data}
          margin={{ top: 5, right: 20, bottom: 0, left: 0 }}
        >
          <Line type="monotone" dataKey="amount" stroke="#1A936F" />
          <XAxis dataKey="name" />
          <YAxis />
          <CartesianGrid stroke="#ccc" strokeDasharray="5 5" />
          <Tooltip />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
}

export default OverviewLineChart