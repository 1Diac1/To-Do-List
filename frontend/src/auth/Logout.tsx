import { googleLogout } from "@react-oauth/google";
import {watchedLogin, watchedName, watchedPicture} from "../store/model.ts";
import {useUnit} from "effector-react";

const Logout = () => {

    const [setLogin, setName, setPicture] = useUnit([watchedLogin, watchedPicture, watchedName])

    const LogOut = () => {
        googleLogout();
        setLogin(false);
        setName('');
        setPicture('');
    }

    return (
        <button onClick={LogOut} className='rounded-xl p-4 font-black transition ease-in-out delay-50 hover:border-black hover:border-2'>
            LOG OUT
        </button>
    )
}

export default Logout;