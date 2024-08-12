
import { SessionProvider } from "next-auth/react"
import SessionCheck from "./SessionCheck"

export default function AuthProvider({
children,
}:{children:React.ReactNode}){
    return (
        <SessionProvider >
          <SessionCheck>{children}</SessionCheck>
        </SessionProvider>
    )
}