"use client";
import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
import ChartOne from "@/components/Charts/ChartOne";
import ChartTwo from "@/components/Charts/ChartTwo";
import dynamic from "next/dynamic";
import { HiUserGroup, HiClipboardList, HiAcademicCap, HiBookOpen, HiDocumentText } from 'react-icons/hi';
import React from "react";

const ChartThree = dynamic(() => import("@/components/Charts/ChartThree"), {
  ssr: false,
});

const Chart: React.FC = () => {
  return (
    <div className="flex h-screen bg-gray-100">

      {/* Main Content */}
      <main className="flex-1 p-6">
        <header className="flex items-center justify-between">
          <h2 className="text-3xl font-bold text-gray-800">Dashboard</h2>
        </header>

        {/* Cards */}
        <div className="grid grid-cols-1 gap-6 mt-6 md:grid-cols-2 lg:grid-cols-4">
          <div className="p-6 bg-gradient-to-r from-blue-400 to-blue-600 rounded-lg shadow-lg text-white">
            <h3 className="text-lg font-semibold">Total Students</h3>
            <p className="text-3xl font-bold">120</p>
          </div>
          <div className="p-6 bg-gradient-to-r from-green-400 to-green-600 rounded-lg shadow-lg text-white">
            <h3 className="text-lg font-semibold">Total Courses</h3>
            <p className="text-3xl font-bold">25</p>
          </div>
          <div className="p-6 bg-gradient-to-r from-yellow-400 to-yellow-600 rounded-lg shadow-lg text-white">
            <h3 className="text-lg font-semibold">Assignments Due</h3>
            <p className="text-3xl font-bold">5</p>
          </div>
          <div className="p-6 bg-gradient-to-r from-purple-400 to-purple-600 rounded-lg shadow-lg text-white">
            <h3 className="text-lg font-semibold">Reports Generated</h3>
            <p className="text-3xl font-bold">10</p>
          </div>
        </div>

        {/* Recent Activities */}
        <section className="mt-6">
  <h2 className="text-2xl font-bold text-gray-800">Recent Activities</h2>
  <div className="mt-4 bg-white rounded-lg shadow-lg p-4">
    <ul className="space-y-2">
      <li className="flex items-center p-3 bg-gray-50 rounded-lg hover:bg-blue-100 transition duration-200">
        <div className="flex-shrink-0">
          <HiUserGroup className="w-6 h-6 text-blue-600" />
        </div>
        <div className="ml-3">
          <span className="font-semibold">Student Annu </span> enrolled in Mathematics
        </div>
      </li>
      <li className="flex items-center p-3 bg-gray-50 rounded-lg hover:bg-blue-100 transition duration-200">
        <div className="flex-shrink-0">
          <HiClipboardList className="w-6 h-6 text-green-600" />
        </div>
        <div className="ml-3">
          <span className="font-semibold">Assignment 'Algebra Homework'</span> submitted by Jane Smith
        </div>
      </li>
      <li className="flex items-center p-3 bg-gray-50 rounded-lg hover:bg-blue-100 transition duration-200">
        <div className="flex-shrink-0">
          <HiAcademicCap className="w-6 h-6 text-yellow-600" />
        </div>
        <div className="ml-3">
          <span className="font-semibold">Course 'Physics'</span> added
        </div>
      </li>
      <li className="flex items-center p-3 bg-gray-50 rounded-lg hover:bg-blue-100 transition duration-200">
        <div className="flex-shrink-0">
          <HiDocumentText className="w-6 h-6 text-purple-600" />
        </div>
        <div className="ml-3">
          <span className="font-semibold">Report Review'</span> generated
        </div>
      </li>
      <li className="flex items-center p-3 bg-gray-50 rounded-lg hover:bg-blue-100 transition duration-200">
        <div className="flex-shrink-0">
          <HiUserGroup className="w-6 h-6 text-blue-600" />
        </div>
        <div className="ml-3">
          <span className="font-semibold">Student Saurabh Mishra</span> enrolled in Chemistry
        </div>
      </li>
    </ul>
  </div>
</section>

      </main>
    </div>
  );
};

export default Chart;
