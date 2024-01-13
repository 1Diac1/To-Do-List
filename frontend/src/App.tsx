import './App.css'
import {BrowserRouter} from "react-router-dom";
import {useRoutes} from "./routes.tsx";
import {AuthContext} from "./context/authcontext.ts";
import {useAuth} from "./hooks/useAuth.ts";
import {authType} from "./types.ts";

function App() {

  const {login, logout, token, userId} = useAuth()



  const isLogin: boolean = !!token
  const routes = useRoutes(isLogin)

  const userData: authType = {
    login,
    logout,
    token,
    userId,
    isLogin,
  }

  return (
      <AuthContext.Provider value={userData}>
    <BrowserRouter>
      {routes}
    </BrowserRouter>
      </AuthContext.Provider>
  )
}

export default App
