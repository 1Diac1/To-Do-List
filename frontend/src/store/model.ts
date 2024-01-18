import {createStore, createEvent} from "effector";

export const $isLogin = createStore<boolean>(false);
export const watchedLogin = createEvent<boolean>();

export const $userName = createStore<null | string>(null);
export const watchedName = createEvent<null | string>();

export const $userPicture = createStore<null | string>(null);
export const watchedPicture = createEvent<null | string>();

$isLogin.on(watchedLogin, (isLog) => !isLog);
$userName.on(watchedName, (name) => name);
$userPicture.on(watchedPicture, (pic) => pic);