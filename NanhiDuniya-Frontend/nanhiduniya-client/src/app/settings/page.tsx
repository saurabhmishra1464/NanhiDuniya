import Breadcrumb from "@/components/Breadcrumbs/Breadcrumb";
import PersonalInformationForm from "@/components/FormElements/PersonalInformationForm";
import UploadImage from "@/components/ImageUpload/UploadImage";
import DefaultLayout from "@/components/Layouts/DefaultLayout";


const Settings = () => {
  return (
    <DefaultLayout>
      <div className="mx-auto max-w-270">
        <Breadcrumb pageName="Settings" />

        <div className="grid grid-cols-5 gap-8">
          <PersonalInformationForm />
          <UploadImage />
        </div>
      </div>
    </DefaultLayout>
  );
};

export default Settings;
