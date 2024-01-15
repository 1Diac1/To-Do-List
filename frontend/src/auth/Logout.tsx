import { googleLogout } from "@react-oauth/google";

const Logout = () => {

    return (
        <button onClick={googleLogout}>
            Logout
        </button>
    )
}

export default Logout;