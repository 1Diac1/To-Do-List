import {useState} from "react";
import Icon from '@mdi/react';
import { mdiCheck, mdiDeleteVariant } from '@mdi/js';

const TodoForm = () => {
    const [inputTodo, setInputTodo] = useState('')

    return (
        <div className='w-full flex justify-center'>
        <div className='bg-white p-5 font-mono flex flex-col justify-items-stretch mt-10 w-96 h-72 flex flex-col rounded justify-center'>
            <h2 className='font-mono text-xl'>Create new TODO</h2>
            <input type='text' className='border-2 border-black p-2 mt-10 rounded-xl'
                   value={inputTodo}
                   onInput={event   => {
                       setInputTodo(event.target.value)
                   }}
            />
            <div className='mt-10 flex justify-around'>
                <button
                    className='bg-red-100 rounded-xl p-4 transition ease-in-out delay-50 hover:border-black hover:border-2'
                    onClick={()=>{setInputTodo('')}}
                >
                    <Icon path={mdiDeleteVariant} size={1}/>
                </button>
                <button
                    className=' bg-lime-100 rounded-xl p-4 transition ease-in-out delay-50 hover:border-black hover:border-2'
                >
                    <Icon path={mdiCheck} size={1} />
                </button>
            </div>
        </div>
        </div>
    );
};

export default TodoForm;