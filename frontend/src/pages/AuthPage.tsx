import Login from '../auth/Login.tsx';
import Logout from '../auth/Logout.tsx';

const AuthPage = () => {
    return (
        <div>
            <h1>Login page</h1>
            <Login/>
            <Logout/>
        </div>
    );
};

export default AuthPage;