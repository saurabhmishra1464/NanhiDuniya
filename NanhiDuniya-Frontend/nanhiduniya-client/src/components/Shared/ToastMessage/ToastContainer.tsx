import { ToastContainer as BaseToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const ToastContainer: React.FC = () => {
    return (
      <BaseToastContainer
        position="bottom-right"
        autoClose={3000} // Close after 3 seconds
        hideProgressBar
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
    );
  };
  
  export { toast, ToastContainer };