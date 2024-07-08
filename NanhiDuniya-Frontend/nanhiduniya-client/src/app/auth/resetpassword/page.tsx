"use client"
import { useRouter, useSearchParams } from 'next/navigation';
import { useState, useEffect } from 'react';
import Link from 'next/link';

export default function ResetPassword() {
  const searchParams = useSearchParams();
  const token = searchParams?.get('token') || '';
  const email = searchParams?.get('email') || '';
  const router = useRouter();
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    if (!token || !email) {
      setError('Invalid password reset link.');
    }
  }, [token, email]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (newPassword !== confirmPassword) {
      setError('Passwords do not match.');
      return;
    }
try {
    const response = await fetch('https://your-backend-url/api/reset-password', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ token, email, newPassword })
    });

    if (response.ok) {
      setSuccess('Password reset successful.');
      setError('');
    } else {
      const data = await response.json();
      setError(data.error || 'Error resetting password.');
      setSuccess('');
    }
}catch(error){
    setError('An unexpected error occurred.');
    setSuccess('');
}
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md p-8 space-y-6 bg-white shadow-md rounded-lg">
        <h2 className="text-2xl font-bold text-center">Reset Password</h2>

      </div>
    </div>
  );
}



// return (
//   <div className="flex items-center justify-center min-h-screen bg-gray-100">
//     <div className="w-full max-w-md p-8 space-y-6 bg-white shadow-md rounded-lg">
//       <h2 className="text-2xl font-bold text-center">Reset Password</h2>
//       {error && <p className="text-red-500">{error}</p>}
//       {success && <p className="text-green-500">{success}</p>}
//       <form className="space-y-6" onSubmit={handleSubmit}>
//         <div>
//           <label htmlFor="newPassword" className="block text-sm font-medium text-gray-700">
//             New Password
//           </label>
//           <input
//             id="newPassword"
//             name="newPassword"
//             type="password"
//             value={newPassword}
//             onChange={(e) => setNewPassword(e.target.value)}
//             required
//             className="w-full p-2 mt-1 border rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
//           />
//         </div>
//         <div>
//           <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700">
//             Confirm Password
//           </label>
//           <input
//             id="confirmPassword"
//             name="confirmPassword"
//             type="password"
//             value={confirmPassword}
//             onChange={(e) => setConfirmPassword(e.target.value)}
//             required
//             className="w-full p-2 mt-1 border rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
//           />
//         </div>
//         <button
//           type="submit"
//           className="w-full px-4 py-2 text-white bg-blue-600 rounded-md hover:bg-blue-700"
//         >
//           Reset Password
//         </button>
//       </form>
//       <p className="text-sm text-center text-gray-600">
//         {/* <Link href="/auth/login">
//           <a className="text-blue-500 hover:text-blue-700">Back to login</a>
//         </Link> */}
//       </p>
//     </div>
//   </div>
// );
