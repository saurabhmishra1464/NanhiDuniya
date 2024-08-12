"use client"

export type ErrorProps= {
    message: string;
}

export default function LoginError({ message }: ErrorProps){
  return (
    <div className="text-red text-md mt-1">
      <h2>{message}</h2>
    </div>
  );
};