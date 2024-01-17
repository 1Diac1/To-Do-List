import {GoogleLogin} from "@react-oauth/google";
import {jwtDecode, JwtPayload} from "jwt-decode";

interface googleRes extends JwtPayload{
    picture: string
    name: string
}

const Login = () => {

    const onSuccess = (creadentialResponse: any) => {
        const decodedCredential: googleRes = jwtDecode(creadentialResponse.credential)
        console.log(decodedCredential.name, decodedCredential.picture);
    }

    const onFailure = () => {
        console.log('LOGIN ACHIEVED FAILURE!');
    }

    return (
        <GoogleLogin
            onSuccess={onSuccess}
            onError={onFailure}
            theme={'filled_black'}
        />
    )
}

export default Login;