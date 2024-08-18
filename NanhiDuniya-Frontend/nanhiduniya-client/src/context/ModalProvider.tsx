// modalContext.tsx
import { createContext, useContext, useState, ReactNode } from 'react';
import { ModalTypes } from '../enums/modalTypes';

interface ModalContextType {
  openModal: (type: ModalTypes) => void;
  closeModal: () => void;
  activeModal: ModalTypes | null;
}

const ModalContext = createContext<ModalContextType | undefined>(undefined);

export const ModalProvider = ({ children }: { children: ReactNode }) => {
  const [activeModal, setActiveModal] = useState<ModalTypes | null>(null);

  const openModal = (type: ModalTypes) => setActiveModal(type);
  const closeModal = () => setActiveModal(null);

  return (
    <ModalContext.Provider value={{ activeModal, openModal, closeModal }}>
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
