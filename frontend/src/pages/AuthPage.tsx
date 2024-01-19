import Login from '../auth/Login.tsx';

const AuthPage = () => {
    return (
        <div className='bg-black m-0 p-4 w-full h-full flex flex-col items-center justify-center'>
            <h1 className='font-mono text-8xl text-slate-200'>Login page</h1>
                <div className='mt-28'>
                    <Login/>
                </div>
        </div>
    );
};

export default AuthPage;