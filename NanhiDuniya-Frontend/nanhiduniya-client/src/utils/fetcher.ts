import { axiosPrivate } from "./AxiosInstances/api";
const fetcher = async (url:string) => {
       const res = await axiosPrivate.get(url);
     return res.data;
  };

  export default fetcher