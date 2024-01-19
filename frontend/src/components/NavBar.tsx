import {$userPicture, $userName} from "../store/model.ts";
import {useUnit} from "effector-react";
import Logout from '../auth/Logout.tsx';

const NavBar = () => {

    const [userName, userPicture] = useUnit([$userName, $userPicture]);

    return (
        <div className='w-full h-content flex items-center bg-white rounded-xl p-3'>
            <div className='flex items-center flex-1'>
                <div>
                <img className='rounded-full' src={userPicture} alt={'Avatar'}/>
                </div>
                <h3 className='ml-4 text-3xl font-thin'>{userName}</h3>
            </div>
            <div className='flex-1'>
                <h2 className='text-4xl font-bold mr-15'>TODO</h2>
            </div>
            <div className='flex-1'>
                <Logout/>
            </div>
        </div>
    );
};

export default NavBar;