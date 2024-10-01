

import React from 'react'
import { FiDownload, FiPlus, FiSearch } from 'react-icons/fi';
interface GenericToolbarProps {
    buttonText: string;
    searchPlaceholder: string;
    searchTerm: string;
    onSearch: (e: React.ChangeEvent<HTMLInputElement>) => void;
    openPopup: () => void;
    onExport: () => void;
  }
const GenericToolbar = ({

    buttonText,
    searchPlaceholder,
    searchTerm,
    onSearch,
    openPopup,
    onExport

}: GenericToolbarProps) => {
  return (
    <div className="mb-4 md:mb-6 flex flex-col md:flex-row md:items-center md:justify-between">
    <div className="flex flex-col md:flex-row md:items-center space-y-2 md:space-y-0 md:space-x-2">
      <button
        type="button"
        className="inline-flex items-center px-3 py-1.5 md:px-4 md:py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        onClick={openPopup}
      >
        <FiPlus className="-ml-1 mr-2 h-4 w-4 md:h-5 md:w-5" aria-hidden="true" />
        {buttonText}
      </button>
      <button
        type="button"
        className="inline-flex items-center px-3 py-1.5 md:px-4 md:py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
        onClick={onExport}
      >
        <FiDownload className="-ml-1 mr-2 h-4 w-4 md:h-5 md:w-5" aria-hidden="true" />
        Export
      </button>
    </div>
    <div className="relative mt-2 md:mt-0">
      <input
        type="text"
        placeholder="Search admins..."
        className="w-full md:w-64 pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
        value={searchTerm}
        onChange={onSearch}
      />
      <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
        <FiSearch className="h-5 w-5 text-gray-400" />
      </div>
    </div>
  </div>
  )
}

export default GenericToolbar