import {GoogleLogin} from "@react-oauth/google";
import {jwtDecode} from "jwt-decode";

const Login = () => {

    const onSuccess = (creadentialResponse: any) => {
        const decodedCredential = jwtDecode(creadentialResponse.credential)
        console.log(decodedCredential);
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