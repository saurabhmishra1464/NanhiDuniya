'use client';
import React, { useState } from 'react'
import Navbar from '@/components/navbar/Navbar';
import Sidebar from '@/components/sidebar/Sidebar';

const  AdminDashBoardPage = ()=> {
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  return (
    <div>
<Sidebar/>

<Navbar/>

    </div> 
  )
}

export default AdminDashBoardPage;
