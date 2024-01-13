import {Routes, Route, Navigate} from "react-router-dom";
import MainPage from "./pages/MainPage.tsx";
import AuthPage from "./pages/AuthPage.tsx";

export const useRoutes = (isLogin: boolean) => {
    if(isLogin){
        return (
            <Routes>
                <Route path='/' element={<MainPage/>} />
                <Navigate to='/' />
            </Routes>
        )
    }
    return (
        <Routes>
            <Route path='/login' element={<AuthPage/>} />
            <Navigate to='/login' />
        </Routes>
    )
}