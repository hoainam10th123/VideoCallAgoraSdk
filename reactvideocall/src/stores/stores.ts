import { createContext, useContext } from "react";
import AudioStore from "./audioStore";
import CommonStore from "./commonStore";
import ModalMakeACallAnswerStore from "./makeACallAnswerStore";
import ModalStore from "./modalStore";
import UserStore from "./userStore";


interface Store {
    commonStore: CommonStore;
    userStore: UserStore;
    modalStore: ModalStore;
    audioStore: AudioStore;
    modalMakeACallAnswer: ModalMakeACallAnswerStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    audioStore: new AudioStore(),
    modalMakeACallAnswer: new ModalMakeACallAnswerStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}