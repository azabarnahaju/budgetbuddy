import React from 'react'
import { stringToDate } from '../../utils/helperFunctions';
import "./TransactionTable.scss";

const TransactionTable = ({ transactions, message }) => {

    if (!transactions){
        return <div>Loading</div>
    }

    if (!transactions.length){
        return message;
    }

  return (
    <div className="latest-transaction-table-container d-flex justify-content-center table-responsive">
      <table className="latest-transaction-table table table-info align-middle">
        <thead className="table-success">
          <tr>
            <th scope="col">Date</th>
            <th className="text-center" scope="col">
              Amount
            </th>
            <th className="text-center" scope="col">
              Name
            </th>
            <th className="text-center" scope="col">
              Tag
            </th>
            <th className="text-center" scope="col">
              Type
            </th>
          </tr>
        </thead>
        <tbody className="table-group-divider">
          {transactions.map((t) => {
            return (
              <tr key={t.id}>
                <th className="table-info" scope="row">
                  {stringToDate(t.date)}
                </th>
                <td className="text-center">${t.amount}</td>
                <td className="text-center">{t.name}</td>
                <td className="text-center">{t.tag}</td>
                <td className="text-center">{t.type}</td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}

export default TransactionTable