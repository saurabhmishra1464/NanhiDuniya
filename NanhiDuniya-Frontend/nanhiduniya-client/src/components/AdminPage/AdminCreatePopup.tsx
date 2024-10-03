import React from 'react'
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { AdminCreateValidation } from '@/lib/validation';
type CreateAdminProps = {
    closePopup: ()=> void;
}

const AdminCreatePopup = ({closePopup}:CreateAdminProps) => {
  const { register, handleSubmit, formState: { errors }, setValue } = useForm<z.infer<typeof AdminCreateValidation>>({
    resolver: zodResolver(AdminCreateValidation),
});

const onSubmit = async(data: z.infer<typeof AdminCreateValidation>) =>{}

  return (
    <div className="fixed inset-0 z-999 overflow-y-auto">
    <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
      <div className="fixed inset-0 transition-opacity" aria-hidden="true">
        <div className="absolute inset-0 bg-gray-500 opacity-75"></div>
      </div>
      <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
      <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-2xl sm:w-full">
        <div className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div className="sm:flex sm:items-start">
            <div className="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
              <h3 className="text-lg leading-6 font-medium text-gray-900" id="modal-title">
                Create New Admin
              </h3>
              <form onSubmit={handleSubmit(onSubmit)}>
              <div className="mt-2">
                <p className="text-sm text-gray-500">
                  Enter the details for the new admin account.
                </p>
                <div className="mt-4">
                  <label htmlFor="name" className="block text-sm font-medium text-gray-700">First Name</label>
                  <input type="text"  {...register("firstName")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                </div>
                {errors?.firstName && (<span className='text-red-500 text-sm mt-1'>{`${errors?.firstName?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="name" className="block text-sm font-medium text-gray-700">Last Name</label>
                  <input type="text" {...register("lastName")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                </div>
                {errors?.lastName && (<span className='text-red-500 text-sm mt-1'>{`${errors?.lastName?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="date" className="block text-sm font-medium text-gray-700">BirthDay</label>
                  <input
                    type="date"
                    {...register("birthDay")}

                    className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                  />
                </div>
                {errors?.birthDay && (<span className='text-red-500 text-sm mt-1'>{`${errors?.birthDay?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="role" className="block text-sm font-medium text-gray-700">Gender</label>
                  <select   {...register("gender")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
                  <option value="">Select gender</option>
                    <option>Male</option>
                    <option>Female</option>
                  </select>
                </div>
                {errors?.gender && (<span className='text-red-500 text-sm mt-1'>{`${errors?.gender?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="address" className="block text-sm font-medium text-gray-700">Address</label>
                  <textarea
                  {...register("address")}

                    className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    placeholder="Enter your address here..."
                  />
                </div>
                {errors?.address && (<span className='text-red-500 text-sm mt-1'>{`${errors?.address?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="name" className="block text-sm font-medium text-gray-700">Blood Group</label>
                  <input type="text" {...register("bloodGroup")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                </div>
                {errors?.bloodGroup && (<span className='text-red-500 text-sm mt-1'>{`${errors?.bloodGroup?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="phone" className="block text-sm font-medium text-gray-700">Phone Number</label>
                  <div className="mt-1 flex items-center">
                   
                    <input
                      type="text"
                      {...register("phoneNumber")}
                      className="block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm rounded-l-none focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                      placeholder="Enter your phone number"
                    />
                  </div>
                </div>
                {errors?.phoneNumber && (<span className='text-red-500 text-sm mt-1'>{`${errors?.phoneNumber?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="email" className="block text-sm font-medium text-gray-700">Email</label>
                  <input type="email"   {...register("email")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                </div>
                {errors?.email && (<span className='text-red-500 text-sm mt-1'>{`${errors?.email?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="name" className="block text-sm font-medium text-gray-700">Password</label>
                  <input type="text" {...register("password")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                </div>
                {errors?.password && (<span className='text-red-500 text-sm mt-1'>{`${errors?.password?.message}`}</span>)}
                <div className="mt-4">
                  <label htmlFor="role" className="block text-sm font-medium text-gray-700">Role</label>
                  <select  {...register("role")} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
                  <option value="">Select Role</option>
                    <option>Super Admin</option>
                    <option>Admin</option>
                  </select>
                </div>
                {errors?.role && (<span className='text-red-500 text-sm mt-1'>{`${errors?.role?.message}`}</span>)}
              </div>
              <div className="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
          <button type="submit" className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:ml-3 sm:w-auto sm:text-sm">
            Create
          </button>
          <button type="button" className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm" onClick={closePopup}>
            Cancel
          </button>
        </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  )
}

export default AdminCreatePopup