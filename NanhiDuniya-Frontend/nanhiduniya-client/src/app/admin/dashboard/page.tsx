'use client';
import React, { useState } from 'react'
import Navbar from '@/components/navbar/Navbar';
import Sidebar from '@/components/sidebar/Sidebar';
import { useSession } from 'next-auth/react';


const  AdminDashBoardPage = ()=> {
  const { data: session } = useSession();
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  return (
    <div>
<Sidebar/>

<Navbar/>

    </div> 
  )
}

export default AdminDashBoardPage;
