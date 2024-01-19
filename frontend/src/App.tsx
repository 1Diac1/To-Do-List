import './App.css'
import {BrowserRouter} from "react-router-dom";
import {useRoutes} from "./routes.tsx";
import {GoogleOAuthProvider} from '@react-oauth/google'
import {$isLogin} from "./store/model.ts";
import {useEffect} from "react";
import {useUnit} from "effector-react";


const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID

function App() {

    const [isLogin] = useUnit([$isLogin])

  let routes = useRoutes(isLogin)

    useEffect(() => {
        routes = useRoutes(isLogin)
    }, [$isLogin]);

  return (
      <GoogleOAuthProvider clientId={clientId}>
        <BrowserRouter>
          {routes}
        </BrowserRouter>
      </GoogleOAuthProvider>
  )
}

export default App;
