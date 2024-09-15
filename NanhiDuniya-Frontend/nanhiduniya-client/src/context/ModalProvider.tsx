// modalContext.tsx
import { createContext, useContext, useState, ReactNode } from 'react';
import { ModalTypes } from '../enums/modalTypes';
import { handleError } from '@/utils/ErrorHandelling/errorHandler';
import { toast } from 'react-toastify';
import { useRouter } from 'next/navigation';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '@/store/store';
import { useAppSelector } from '@/hooks/hooks';
import { useLogoutUserMutation } from '@/services/auth';
import { logOut } from '@/store/auth-slice';

interface ModalContextType {
  openModal: (type: ModalTypes) => void;
  closeModal: () => void;
  activeModal: ModalTypes | null;
  logout: () => void;
}

const ModalContext = createContext<ModalContextType | undefined>(undefined);

export const ModalProvider = ({ children }: { children: ReactNode }) => {
  const router = useRouter();
  const { user } = useAppSelector((state) => state.auth);
  const [
    logoutUser, 
  ] = useLogoutUserMutation();
  const [activeModal, setActiveModal] = useState<ModalTypes | null>(null);

  const dispatch = useDispatch<AppDispatch>();
  const openModal = (type: ModalTypes) => setActiveModal(type);
  const closeModal = () => setActiveModal(null);
  const logout = async () => {
    try{
    if (user?.id) {
      // const result = await dispatch(logoutUser({ userId: user.id }));
      const result = await logoutUser({ userId: user.id } );
      if (result.data?.success) {
        dispatch(logOut())
        closeModal();
        toast.success(result.data?.message)
        router.push("/auth/login");
      }
    }
  }catch(error:any){
    let message = handleError(error);
    toast.error(message || "Logout failed!");
  }
  }

  return (
    <ModalContext.Provider value={{ activeModal, openModal, closeModal, logout }}>
      {children}
    </ModalContext.Provider>
  );
};

export const useModal = () => {
  const context = useContext(ModalContext);
  if (!context) {
    throw new Error('useModal must be used within a ModalProvider');
  }
  return context;
};
