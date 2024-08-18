import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
import PersonalInformationForm from "@/components/FormElements/PersonalInformationForm";
import UploadImage from "@/components/ImageUpload/UploadImage";

// Dynamically import Client Components with React.lazy

const Settings = () => {
  return (
      <div className="mx-auto max-w-270">
        <Breadcrumb pageName="Settings" />

        <div className="grid grid-cols-5 gap-8">
          {/* Wrap with Suspense to handle loading states */}

            <PersonalInformationForm />

            <UploadImage />
        </div>
      </div>
  );
};

export default Settings;




















// import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
// import dynamic from "next/dynamic";
// import { Suspense, lazy } from "react";

// // Dynamically import Client Components with React.lazy
// const PersonalInformationForm = dynamic(() => import("@/components/FormElements/PersonalInformationForm"), { ssr: false });
// const UploadImage = dynamic(() => import("@/components/ImageUpload/UploadImage"), { ssr: false });

// const Settings = () => {
//   return (
//       <div className="mx-auto max-w-270">
//         <Breadcrumb pageName="Settings" />

//         <div className="grid grid-cols-5 gap-8">
//           {/* Wrap with Suspense to handle loading states */}
//           <Suspense fallback={<div>Loading Form...</div>}>
//             <PersonalInformationForm />
//           </Suspense>
//           <Suspense fallback={<div>Loading Image Upload...</div>}>
//             <UploadImage />
//           </Suspense>
//         </div>
//       </div>
//   );
// };

// export default Settings;

