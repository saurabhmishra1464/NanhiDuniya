// 'use client'
// import React, { useState } from "react";
// import { FiChevronDown, FiPlus, FiChevronRight, FiSearch, FiDownload, FiX } from "react-icons/fi";

// const AdminList = () => {
//   const [isDropdownOpen, setIsDropdownOpen] = useState({});
//   const [expandedAddresses, setExpandedAddresses] = useState({});
//   const [searchTerm, setSearchTerm] = useState("");
//   const [isPopupOpen, setIsPopupOpen] = useState(false);

//   const title = "Admin List";
//   const breadcrumb = ["Dashboard", "User Management", "Admin List"];
//   const buttonText = "Create New Admin";

//   const columns = [
//     { type: "number", key: "id", label: "ID" },
//     { type: "profile", key: "profile", subKey: "name", label: "Name" },
//     { type: "info", key: "email", label: "Email" },
//     { type: "info", key: "role", label: "Role" },
//     {
//       type: "status",
//       key: "status",
//       label: "Status",
//       badgeColor: { active: "green", inactive: "red" },
//       enabledText: "Active",
//       disabledText: "Inactive"
//     },
//     {
//       type: "userinfo",
//       key: "userinfo",
//       label: "User Info"
//     },
//     {
//       type: "dropdown",
//       key: "actions",
//       label: "Actions",
//       options: ["Edit", "Delete", "Disable"]
//     }
//   ];

//   const rows = [
//     {
//       id: 1,
//       profile: {
//         name: "John Doe",
//         avatar: "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
//       },
//       email: "john@example.com",
//       role: "Super Admin",
//       status: "active",
//       userinfo: {
//         phoneNumber: "+1 (555) 123-4567",
//         address: "123 Main St, Anytown, USA 12345"
//       }
//     },
//     {
//       id: 2,
//       profile: {
//         name: "Jane Smith",
//         avatar: "https://images.unsplash.com/photo-1494790108377-be9c29b29330?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
//       },
//       email: "jane@example.com",
//       role: "Admin",
//       status: "inactive",
//       userinfo: {
//         phoneNumber: "+1 (555) 987-6543",
//         address: "456 Elm St, Another City, USA 67890"
//       }
//     }
//   ];

//   const toggleDropdown = (id) => {
//     setIsDropdownOpen((prev) => ({
//       ...prev,
//       [id]: !prev[id]
//     }));
//   };

//   const toggleAddressExpansion = (id) => {
//     setExpandedAddresses((prev) => ({
//       ...prev,
//       [id]: !prev[id]
//     }));
//   };

//   const handleSearch = (event) => {
//     setSearchTerm(event.target.value);
//   };

//   const filteredRows = rows.filter((row) =>
//     row.profile.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
//     row.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
//     row.role.toLowerCase().includes(searchTerm.toLowerCase())
//   );

//   const renderCell = (column, row) => {
//     switch (column.type) {
//       case "number":
//         return <span className="text-gray-900">{row[column.key]}</span>;
//       case "profile":
//         return (
//           <div className="flex items-center">
//             <img
//               src={row[column.key].avatar}
//               alt={row[column.key][column.subKey]}
//               className="h-8 w-8 md:h-10 md:w-10 rounded-full mr-2 md:mr-3"
//             />
//             <span className="text-gray-900 text-sm md:text-base">{row[column.key][column.subKey]}</span>
//           </div>
//         );
//       case "info":
//         return <span className="text-gray-500 text-sm md:text-base">{row[column.key]}</span>;
//       case "status":
//         return (
//           <span
//             className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
//               row[column.key] === "active"
//                 ? "bg-green-100 text-green-800"
//                 : "bg-red-100 text-red-800"
//             }`}
//           >
//             {row[column.key] === "active" ? column.enabledText : column.disabledText}
//           </span>
//         );
//       case "userinfo":
//         return (
//           <div className="text-sm">
//             <p className="text-gray-900">{row[column.key].phoneNumber}</p>
//             <div className="flex items-center">
//               <button
//                 onClick={() => toggleAddressExpansion(row.id)}
//                 className="text-blue-600 hover:text-blue-800 focus:outline-none"
//               >
//                 <FiChevronRight
//                   className={`h-4 w-4 transform ${expandedAddresses[row.id] ? "rotate-90" : ""}`}
//                 />
//               </button>
//               <span className="ml-1 text-gray-500">
//                 {expandedAddresses[row.id] ? row[column.key].address : "View Address"}
//               </span>
//             </div>
//           </div>
//         );
//       case "dropdown":
//         return (
//           <div className="relative inline-block text-left">
//             <div>
//               <button
//                 type="button"
//                 className="inline-flex justify-center w-full rounded-md border border-gray-300 shadow-sm px-2 py-1 md:px-4 md:py-2 bg-white text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-100 focus:ring-indigo-500"
//                 onClick={() => toggleDropdown(row.id)}
//               >
//                 Actions
//                 <FiChevronDown className="-mr-1 ml-2 h-4 w-4 md:h-5 md:w-5" aria-hidden="true" />
//               </button>
//             </div>
//             {isDropdownOpen[row.id] && (
//               <div className="origin-top-right absolute right-0 mt-2 w-40 md:w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none z-10">
//                 <div className="py-1" role="menu" aria-orientation="vertical" aria-labelledby="options-menu">
//                   {column.options.map((option) => (
//                     <a
//                       key={option}
//                       href="#"
//                       className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900"
//                       role="menuitem"
//                       onClick={(e) => {
//                         e.preventDefault();
//                         console.log(`${option} clicked for row ${row.id}`);
//                         toggleDropdown(row.id);
//                       }}
//                     >
//                       {option}
//                     </a>
//                   ))}
//                 </div>
//               </div>
//             )}
//           </div>
//         );
//       default:
//         return null;
//     }
//   };

//   const openPopup = () => {
//     setIsPopupOpen(true);
//   };

//   const closePopup = () => {
//     setIsPopupOpen(false);
//   };

//   return (
//     <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
//       <div className="mb-6 md:mb-8">
//         <h1 className="text-2xl md:text-3xl font-bold leading-tight text-gray-900">{title}</h1>
//         <nav className="flex mt-2" aria-label="Breadcrumb">
//           <ol className="inline-flex items-center space-x-1 md:space-x-3 text-sm md:text-base">
//             {breadcrumb.map((item, index) => (
//               <li key={index} className="inline-flex items-center">
//                 {index > 0 && (
//                   <svg
//                     className="w-4 h-4 md:w-6 md:h-6 text-gray-400"
//                     fill="currentColor"
//                     viewBox="0 0 20 20"
//                     xmlns="http://www.w3.org/2000/svg"
//                   >
//                     <path
//                       fillRule="evenodd"
//                       d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
//                       clipRule="evenodd"
//                     />
//                   </svg>
//                 )}
//                 <a
//                   href="#"
//                   className={`ml-1 ${
//                     index === breadcrumb.length - 1
//                       ? "text-gray-500 hover:text-gray-700"
//                       : "text-blue-600 hover:text-blue-800"
//                   }`}
//                 >
//                   {item}
//                 </a>
//               </li>
//             ))}
//           </ol>
//         </nav>
//       </div>
//       <div className="mb-4 md:mb-6 flex flex-col md:flex-row md:items-center md:justify-between">
//         <div className="flex flex-col md:flex-row md:items-center space-y-2 md:space-y-0 md:space-x-2">
//           <button
//             type="button"
//             className="inline-flex items-center px-3 py-1.5 md:px-4 md:py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
//             onClick={openPopup}
//           >
//             <FiPlus className="-ml-1 mr-2 h-4 w-4 md:h-5 md:w-5" aria-hidden="true" />
//             {buttonText}
//           </button>
//           <button
//             type="button"
//             className="inline-flex items-center px-3 py-1.5 md:px-4 md:py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
//           >
//             <FiDownload className="-ml-1 mr-2 h-4 w-4 md:h-5 md:w-5" aria-hidden="true" />
//             Export
//           </button>
//         </div>
//         <div className="relative mt-2 md:mt-0">
//           <input
//             type="text"
//             placeholder="Search admins..."
//             className="w-full md:w-64 pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
//             value={searchTerm}
//             onChange={handleSearch}
//           />
//           <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
//             <FiSearch className="h-5 w-5 text-gray-400" />
//           </div>
//         </div>
//       </div>
//       <div className="flex flex-col">
//         <div className="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
//           <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
//             <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
//               <div className="overflow-x-auto">
//                 <table className="min-w-full divide-y divide-gray-200">
//                   <thead className="bg-gray-50">
//                     <tr>
//                       {columns.map((column) => (
//                         <th
//                           key={column.key}
//                           scope="col"
//                           className="px-4 md:px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
//                         >
//                           {column.label}
//                         </th>
//                       ))}
//                     </tr>
//                   </thead>
//                   <tbody className="bg-white divide-y divide-gray-200">
//                     {filteredRows.map((row) => (
//                       <tr key={row.id}>
//                         {columns.map((column) => (
//                           <td key={column.key} className="px-4 md:px-6 py-3 md:py-4 whitespace-nowrap">
//                             {renderCell(column, row)}
//                           </td>
//                         ))}
//                       </tr>
//                     ))}
//                   </tbody>
//                 </table>
//               </div>
//             </div>
//           </div>
//         </div>
//       </div>

//       {isPopupOpen && (
//         <div className="fixed inset-0 z-50 overflow-y-auto">
//           <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
//             <div className="fixed inset-0 transition-opacity" aria-hidden="true">
//               <div className="absolute inset-0 bg-gray-500 opacity-75"></div>
//             </div>
//             <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
//             <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-2xl sm:w-full">
//               <div className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
//                 <div className="sm:flex sm:items-start">
//                   <div className="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
//                     <h3 className="text-lg leading-6 font-medium text-gray-900" id="modal-title">
//                       Create New Admin
//                     </h3>
//                     <div className="mt-2">
//                       <p className="text-sm text-gray-500">
//                         Enter the details for the new admin account.
//                       </p>
//                       <div className="mt-4">
//                         <label htmlFor="name" className="block text-sm font-medium text-gray-700">Name</label>
//                         <input type="text" name="name" id="name" className="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
//                       </div>
//                       <div className="mt-4">
//                         <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
//                         <input type="email" name="email" id="email" className="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
//                       </div>
//                       <div className="mt-4">
//                         <label htmlFor="role" className="block text-sm font-medium text-gray-700">Role</label>
//                         <select id="role" name="role" className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
//                           <option>Super Admin</option>
//                           <option>Admin</option>
//                         </select>
//                       </div>
//                     </div>
//                   </div>
//                 </div>
//               </div>
//               <div className="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
//                 <button type="button" className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:ml-3 sm:w-auto sm:text-sm">
//                   Create
//                 </button>
//                 <button type="button" className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm" onClick={closePopup}>
//                   Cancel
//                 </button>
//               </div>
//             </div>
//           </div>
//         </div>
//       )}
//     </div>
//   );
// };

// export default AdminList;
