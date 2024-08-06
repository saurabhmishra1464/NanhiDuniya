"use client";

import { downloadCourse } from "../../app/actions/course";

export const DownloadCourse = () => {
  return (
    <form action={downloadCourse}>
      <button type="submit" className="bg-black text-white p-1 rounded">
        Download
      </button>
    </form>
  );
};
