"use client"

export type ErrorProps= {
  error: {
    message: string;
    statusCode: number;
  };
}

export default function LoginError({ error }: ErrorProps){
  return (
    <div className="text-red-500 text-sm mt-1">
      <h2 className="font-bold">Error {error.statusCode}</h2>
      <p>{error?.message}</p>
    </div>
  );
};