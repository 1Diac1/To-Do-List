import {useState, useEffect, useCallback} from "react";



export const useAuth = () => {
    const [token, setToken] = useState('')
    const [userId, setUserId] = useState('')
    const [isReady, setIsReady] = useState(false)

    const login = (jwtToken: string, id: string) => {
        setToken(jwtToken)
        setUserId(id)

        localStorage.setItem('userData',JSON.stringify({
            userId,
            token,
        }))
    }
    const logout = () => {
        setToken('')
        setUserId('')
        localStorage.removeItem('userData')
    }

    useEffect(()=>{
        const data = JSON.parse(localStorage.getItem('userData')!)
        if(data && data.token)
            login(data.token, data.userId)
        setIsReady(true)
    },[login])

    return {login, logout, token, userId, isReady}
}