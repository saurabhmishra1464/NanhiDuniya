import React, { useState } from 'react'
import GenericToolbar from '../common/GenericToolbar/genericToolbar'
import AdminGrid from './AdminGrid';
import AdminCreatePopup from './AdminCreatePopup';

const AdminScreen = () => {
    const title = "Admin List";
    const [searchTerm, setSearchTerm] = useState("");
    const [isPopupOpen, setIsPopupOpen] = useState(false);
    const handleSearch = (event: any) => {
        setSearchTerm(event.target.value);
    };
    const openPopup = () => {
        setIsPopupOpen(true);
    };

    const closePopup = () => {
        setIsPopupOpen(false);
    };

    const handleExport = () => {
        // Logic to export the data
    };
    return (
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="mb-6 md:mb-8">
                <h1 className="text-2xl md:text-3xl font-bold leading-tight text-gray-900">{title}</h1>
                <nav className="flex mt-2" aria-label="Breadcrumb">
                    <ol className="inline-flex items-center space-x-1 md:space-x-3 text-sm md:text-base">

                    </ol>
                </nav>
            </div>
            <GenericToolbar
                buttonText="Create Admin"
                searchPlaceholder="Search admins..."
                searchTerm={searchTerm}
                onSearch={handleSearch}
                openPopup={openPopup}
                onExport={handleExport}
            />
            <AdminGrid />

            {isPopupOpen && (
                <AdminCreatePopup closePopup={closePopup} />
            )}
        </div>
    )
}

export default AdminScreen