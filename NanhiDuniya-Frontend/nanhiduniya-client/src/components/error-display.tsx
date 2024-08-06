type ErrorDisplayProps = {
    message: string
    reset: () => void
  }
export  const ErrorDisplay = ({message, reset}: ErrorDisplayProps)=>{
      return (
    <div>
      <h2>Something went wrong!</h2>
      <button onClick={() => reset()}>
        Try again
      </button>
    </div>
  )
}