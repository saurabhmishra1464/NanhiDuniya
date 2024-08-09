import LoginForm from '@/components/forms/LoginForm';

const LoginPage = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-cover bg-center" style={{ backgroundImage: 'url(/images/LoginBackground.webp)' }}>
      <div className="text-center mb-4">
        <h1 className="text-3xl font-bold text-blue-600">Nanhi Duniya</h1>
        <p>mobile mei hai performance optimize website pdf</p>
      </div>
      <div className="bg-white p-8 rounded-lg shadow-md w-full md:w-96 lg:w-1/2 xl:w-1/3 2xl:w-1/4">
        <h2 className="text-2xl font-semibold mb-4 text-center text-gray-600">Log in to your account</h2>
        <LoginForm />
      </div>
    </div>
  );
}

export default LoginPage