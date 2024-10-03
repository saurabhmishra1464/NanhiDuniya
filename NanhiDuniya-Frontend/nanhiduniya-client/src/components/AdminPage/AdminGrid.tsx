import React, { useState } from 'react';
import Grid from '../common/grid/grid';
import { ColumnDef, createColumnHelper } from '@tanstack/react-table';
import { FaEllipsisV } from "react-icons/fa";
type Person = {
    id: string
    Name: string
    Email: string
    PhoneNumber: string
    Address: string
    visits: number
    status: string
}

const AdminGrid = () => {
    const [openDropdown, setOpenDropdown] = useState(null);
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

    const defaultData: Person[] = [
        {
            id: "1",
            Name: 'tanner',
            Email: 'linsley',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 100,
            status: 'Enable',
        },
        {
            id: "2",
            Name: 'tandy',
            Email: 'miller',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 40,
            status: 'Disable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
        {
            id: "3",
            Name: 'joe',
            Email: 'dirte',
            PhoneNumber: "8825149794",
            Address: "Muzaffarpur",
            visits: 20,
            status: 'Enable',
        },
    ]

    const columns: ColumnDef<Person>[] = [
        {
            accessorKey: 'id',
            header: '#',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'Name',
            header: 'Name',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'Email',
            header: 'Email',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'PhoneNumber',
            header: 'PhoneNumber',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'Address',
            header: 'Address',
            cell: info => info.getValue(),
        },
        {
            accessorKey: 'status',
            header: 'Account Status',
            cell: info => info.getValue(),
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
    return (
        <Grid
            columns={columns} data={defaultData}
        />
    )
}

export default AdminGrid