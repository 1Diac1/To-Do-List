import {createStore, createEvent} from "effector";

export const $isLogin = createStore<boolean>(false);
export const watchedLogin = createEvent<boolean>();

export const $userName = createStore<string>('');
export const watchedName = createEvent<string>();

export const $userPicture = createStore<string>('');
export const watchedPicture = createEvent<string>();

$isLogin.on(watchedLogin, (_, isLog) => isLog);
$userName.on(watchedName, (_, name) => name);
$userPicture.on(watchedPicture, (_, pic) => pic);