import { makeAutoObservable, reaction } from "mobx";
import { history } from "..";
import agent from "../api/agent";
import { IUser, UserLogin } from "../models/user";

export default class UserStore {
    user: IUser | null = JSON.parse(localStorage.getItem('user')!);
    channelName: string | null = null;

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.user,
            user => {
                if (user) {
                    user.roles = [];
                    const roles = this.getDecodedToken(user.token).role;
                    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
                    localStorage.setItem('user', JSON.stringify(user))
                } else {
                    localStorage.removeItem('user')
                }
            }
        )
    }

    get isLoggedIn() {
        return !!this.user;
    }

    login = async (creds: UserLogin) => {
        try {
            const user = await agent.Account.login(creds);
            this.setUser(user);
            history.push('/'); //redict to home page
        } catch (error) {
            throw error;
        }
    }

    logout = async () => {
        await agent.FirebaseAdminSDK.userDisconnected()
        this.setUser(null);
        history.push('/login');// redict to login page
    }

    getUser = async () => {
        try {
            const user = JSON.parse(window.localStorage.getItem('user')!);
            if (!user) await agent.Account.current();
            this.setUser(user);
        } catch (error) {
            console.log(error);
        }
    }

    setUser = (user: IUser | null) => {
        this.user = user;
    }

    getDecodedToken(token: string) {
        return JSON.parse(atob(token.split('.')[1]));
    }

    setChannel = (channel: string | null) => {
        this.channelName = channel;
    }
}