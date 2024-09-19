import { useState } from "react";


const forgotPassword = () => {
    const [email, setEmail] = useState('');
    const forgotPassword = (e: React.FormEvent) => {
        e.preventDefault();
        try {
const response 
        } catch (error) {

        }
    }
    return (
        <div className="">
            <h2>Forgot Password</h2>
            <form onSubmit={forgotPassword}>
                <input type="email" value="" onChange={(e) => setEmail(e.target.value)} placeholder="" required />
                <button type="submit">Send Reset Link</button>
            </form>
        </div>
    );
};

export default forgotPassword;