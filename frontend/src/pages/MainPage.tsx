import NavBar from "../components/NavBar.tsx";
import TodoForm from "../components/TodoForm.tsx";

const MainPage = () => {
    return (
        <div className='bg-black m-0 p-4 w-full h-full flex flex-col items-center text-center'>
            <NavBar/>
            <TodoForm/>
        </div>
    );
};

export default MainPage;