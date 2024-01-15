import {Routes, Route, Navigate} from "react-router-dom";
import MainPage from "./pages/MainPage.tsx";
import AuthPage from "./pages/AuthPage.tsx";

export const useRoutes = (isLogin: boolean) => {
    if(isLogin){
        return (
            <>
                <Navigate to='/' />
                <Routes>
                    <Route path='/' element={<MainPage/>}/>
                </Routes>
            </>
        )
    }
    return (
        <>
            <Navigate to='/login' />
            <Routes>
                <Route path='/login' element={<AuthPage/>} />
            </Routes>
        </>
    )
}