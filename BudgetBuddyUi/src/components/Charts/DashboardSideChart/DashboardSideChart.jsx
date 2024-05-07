import React from 'react'
import { BarChart, Bar, ResponsiveContainer, XAxis, YAxis, Tooltip } from "recharts";
import { useState, useEffect } from 'react';
import { getSideChartData } from '../../../utils/chartData';
import "./DashboardSideChart.scss";

const DashboardSideChart = ({ transactions }) => {
 const [data, setData] = useState(null);
 useEffect(() => {
   const newData = getSideChartData(transactions);
   setData(newData);
 }, []);

 return (
   <div className="barchart-container">
     <ResponsiveContainer>
       <BarChart width={150} height={40} data={data}>
         <Bar dataKey="amount" fill="#1A936F" />
         <XAxis dataKey="name" />
         <YAxis />
         <Tooltip />
       </BarChart>
     </ResponsiveContainer>
   </div>
 );
}

export default DashboardSideChart