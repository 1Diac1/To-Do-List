import './App.css'
import {BrowserRouter} from "react-router-dom";
import {useRoutes} from "./routes.tsx";
import {GoogleOAuthProvider} from '@react-oauth/google'

const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID

function App() {

  const isLogin: boolean = false
  const routes = useRoutes(isLogin)
    console.log(clientId)


  return (
      <GoogleOAuthProvider clientId={clientId}>
        <BrowserRouter>
          {routes}
        </BrowserRouter>
      </GoogleOAuthProvider>
  )
}

export default App
