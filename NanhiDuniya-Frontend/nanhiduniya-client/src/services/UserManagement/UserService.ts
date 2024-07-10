import ResetPassword from '@/model/User';
import {axiosInstanceUserManagement} from '../../utils/AxiosInstances/api';
import { ApiResponse } from '../Responses/ApiResponse';
import { AxiosResponse } from 'axios';

const resetPassword = async (resetPasswordData: ResetPassword) =>{
    try{
        const response  = await axiosInstanceUserManagement.post('/api/Account/ResetPassword', resetPasswordData);
        return response.data;
    }catch{

    }
}

const UserService = {
    resetPassword,
  };
  export default UserService;