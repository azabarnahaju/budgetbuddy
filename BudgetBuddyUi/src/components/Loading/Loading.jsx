/* eslint-disable react/prop-types */
import loadingGIF from "/loading.gif";

const Loading = ({ message = "Loading..." }) => {
  return (
    <>
      <div className="container text-center py-5">
        <div>{message}</div>
        <img className="w-25" src={loadingGIF} alt="Loading" />
      </div>
    </>
  );
};

export default Loading;
