import React, { useState } from 'react';
import Grid from '../common/grid/grid';
import { ColumnDef, createColumnHelper } from '@tanstack/react-table';
import { FaEllipsisV } from "react-icons/fa";
import { useGetAdminsQuery } from '@/services/auth';
import { Admins } from '@/model/Responses/AuthResponse';


const AdminGrid = () => {
    const [openDropdown, setOpenDropdown] = useState(null);
    const { data, isLoading, isError } = useGetAdminsQuery();
    const toggleDropdown = (id: any) => {
        setOpenDropdown(openDropdown === id ? null : id);
    };

    const handleDelete = (id: any) => {
        console.log("Delete", id);
        setOpenDropdown(null);
    };

    const handleEdit = (row: any) => {
        console.log("Edit", row);
        setOpenDropdown(null);
    };



    const columns: ColumnDef<Admins>[] = [
        {
            accessorKey: 'id',
            header: 'Id',
            cell: ({ row }) => row.index + 1,
        },
        {
            accessorKey: 'firstName', // Changed from 'Name' to 'firstName'
            header: 'Name',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'email', // Changed from 'Email' to 'email'
            header: 'Email',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'phoneNumber', // Changed from 'PhoneNumber' to 'phoneNumber'
            header: 'Phone Number',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'address', // Changed from 'Address' to 'address'
            header: 'Address',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'status',
            header: 'Account Status',
            cell: info => (info.getValue() ? 'Enabled' : 'Disabled'), // Assuming status is a boolean
        },

        {
            header: "Options",
            accessorKey: "options",
            cell: ({ row }) => (
                <div className="relative">
                    <button
                        className="text-gray-500 hover:text-gray-700 focus:outline-none"
                        onClick={() => toggleDropdown(row.original.id)}
                    >
                        <FaEllipsisV />
                    </button>
                    {openDropdown === row.original.id && (
                        <div className="absolute right-0 mt-2 w-48 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5">
                            <div
                                className="py-1"
                                role="menu"
                                aria-orientation="vertical"
                                aria-labelledby="options-menu"
                            >
                                <button
                                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 w-full text-left"
                                    role="menuitem"
                                    onClick={() => handleEdit(row.original)}
                                >
                                    Edit
                                </button>
                                <button
                                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900 w-full text-left"
                                    role="menuitem"
                                    onClick={() => handleDelete(row.original.id)}
                                >
                                    Delete
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            ),
        },
    ];
    const admins: Admins[] = Array.isArray(data?.data) ? data.data : [];
    console.log("adminlist", admins);
    return (
        <Grid
            columns={columns} data={admins}
        />
    )
}

export default AdminGrid