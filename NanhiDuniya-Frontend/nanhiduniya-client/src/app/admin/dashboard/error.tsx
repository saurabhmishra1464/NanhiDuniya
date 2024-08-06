'use client'

import { useEffect, useState } from 'react'
import {ErrorDisplay} from '../../../components/error-display'
type ErrorProps ={
  error: Error
  reset: () => void
}

export default function Error({ error, reset }: ErrorProps) {
  const [message,setMessage] =useState("");
  useEffect(() => {
    setMessage(error.message);
    console.error(error);
  }, [error])
  return(
<>
  <ErrorDisplay message={error.message} reset={reset}/>
  </>
  )
}