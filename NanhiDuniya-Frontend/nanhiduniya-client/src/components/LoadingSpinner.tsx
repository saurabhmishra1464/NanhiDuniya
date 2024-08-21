// components/LoadingSpinner.tsx
import React from 'react';

const LoadingSpinner = () => {
  return (
    <div className="flex justify-center items-center h-screen">
      <div className="animate-spin h-8 w-8 border-t-2 border-blue-500 rounded-full"></div>
      <span className="ml-4 text-gray-500">Component Loading...</span>
      </div>
  );
};

export default LoadingSpinner;
