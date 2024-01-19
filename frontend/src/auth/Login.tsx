import {CredentialResponse, GoogleLogin} from "@react-oauth/google";
import {jwtDecode, JwtPayload} from "jwt-decode";
import {watchedLogin, watchedName, watchedPicture} from "../store/model.ts";
import {useUnit} from "effector-react";



interface googleRes extends JwtPayload{
    picture: string
    name: string
}

const Login = () => {

    const [setLogin, setPicture, setName] = useUnit([watchedLogin, watchedPicture, watchedName])

    const onSuccess = (creadentialResponse: CredentialResponse) => {
        const decodedCredential: googleRes = jwtDecode(creadentialResponse.credential!)
        if(decodedCredential.name) {
            setLogin(true)
            setName(decodedCredential.name)
            setPicture(decodedCredential.picture)
        }
    }

    const onFailure = () => {
        console.log('LOGIN ACHIEVED FAILURE!');
    }

    return (
        <GoogleLogin
            onSuccess={onSuccess}
            onError={onFailure}
            theme={'filled_black'}
            width={500}
        />
    )
}

export default Login;