/* eslint-disable react/prop-types */

const Loading = ({ message = "Loading..." }) => {
  return (
    <>
      <div className="container text-center py-5">
        <div>{message}</div>
        <img className="w-25" src="/assets/loading.gif" alt="Loading" />
      </div>
    </>
  );
};

export default Loading;
