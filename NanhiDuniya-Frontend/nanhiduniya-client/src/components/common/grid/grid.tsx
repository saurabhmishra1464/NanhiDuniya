import React from 'react'
import { ColumnDef, flexRender, getCoreRowModel, getPaginationRowModel, useReactTable } from '@tanstack/react-table';

type Props<T> = {
    columns: ColumnDef<T, any>[]; // Array of column headings
    data: T[] | undefined; // Array of data rows
  };

  function Grid<T extends object>({ columns, data }: Props<T>) {
    const table = useReactTable({
        data: data ?? [],
        columns,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
      });
  return (
    <table className="w-full border-collapse">
      <thead className="bg-gray-50">
        {table.getHeaderGroups().map(headerGroup => (
          <tr key={headerGroup.id} className="bg-gray-100">
            {headerGroup.headers.map(header => (
              <th
                key={header.id}
           className="p-3 text-left font-semibold text-gray-600 border-b cursor-pointer hover:bg-gray-200 transition duration-200"
              >
                {header.isPlaceholder
                  ? null
                  : flexRender(header.column.columnDef.header, header.getContext())}
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody className="bg-white divide-y divide-gray-200">
        {table.getRowModel().rows.map(row => (
          <tr key={row.id}>
            {row.getVisibleCells().map(cell => (
              <td key={cell.id} className="px-6 py-4 whitespace-nowrap">
                {flexRender(cell.column.columnDef.cell, cell.getContext())}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  )
}

export default Grid