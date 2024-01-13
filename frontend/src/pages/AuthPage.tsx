import React, {useState} from "react";
import axios from 'axios';
const AuthPage = () => {

    const [form, setForm] = useState({
        email: '',
        password: ''
    })

    const changeHandler = (event: React.ChangeEventHandler) => {
        setForm({...form, [event.target.name]: event.target.value})
    }

    const registerHandler = async () => {
        try {
            await axios.post('api/registration',{...form}, {
                headers:{
                    "Content-Type": 'application/json'
                }
            }).then(res => console.log(res))
        } catch (e) {
            console.log(e)
        }
    }

    const loginHandler = async () => {
        try {
            await axios.post('api/login',{...form}, {
                headers:{
                    "Content-Type": 'application/json'
                }
            }).then(res => console.log(res))
        } catch (e) {
            console.log(e)
        }
    }

    return (
        <div>

        </div>
    );
};

export default AuthPage;