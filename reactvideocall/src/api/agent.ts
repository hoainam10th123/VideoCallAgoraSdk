import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "..";
import { IUser, UserLogin } from "../models/user";
import { store } from "../stores/stores";

const sleep = (delay: number) => {
    return new Promise((resolve => {
        setTimeout(resolve, delay)
    }))
}

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.request.use(config => {
    const token = store.userStore.user?.token;
    if (token) config.headers!.Authorization = `Bearer ${token}`
    return config;
})

axios.interceptors.response.use(async respone => {
    if (process.env.NODE_ENV === 'development') await sleep(1000);
    return respone;
}, (error: AxiosError) => {
    const { data, status, config } = error.response!;
    switch (status) {
        case 400:
            const dulieu = data as any;
            if (dulieu.errors) {
                const modelStateErrors = [];
                for (const key in dulieu.errors) {
                    if (dulieu.errors[key]) {
                        modelStateErrors.push(dulieu.errors[key]);
                    }
                }
                throw modelStateErrors.flat();
            } else {
                toast.error(dulieu);
            }
            break;
        case 401:
            toast.error('unauthorised!');
            break;
        case 404:
            history.push('/not-found');
            break;
        case 500:
            toast.error('500 internal server!');
            store.commonStore.setServerError(data as any);
            history.push('/server-error');
            break;
        default:
            console.log(data);
            toast.error(status.toString() + ' see console log');
            break;
    }
    return Promise.reject(error);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Account = {
    current: () => requests.get<IUser>('account'),
    login: (user: UserLogin) => requests.post<IUser>('account/login', user),
}

const FirebaseAdminSDK = {
    setFinishCalling: (isCalling: boolean) => requests.get<void>('FirebaseAdmin/set-calling/'+isCalling),
    addToken: (token: string) => requests.post<void>('FirebaseAdmin/add-token/'+token, {}),
    sendNotificationSpecific: (username: string) => requests.post<void>('FirebaseAdmin/send-notification-specific/'+username, {}),
    userDisconnected: () => requests.get<void>('FirebaseAdmin/user-disconnected/'),
    timUserTongDai: (data: string) => requests.post<void>('FirebaseAdmin/tim-tong-dai-vien/'+data, {}),
    denyCall: (username: string) => requests.get<void>('FirebaseAdmin/get-tu-choi/'+username),
}

const Agora = {
    getRtcToken: (setting: any) => requests.post<string>('Agora/get-rtc-token', setting),
}

const agent = {
    Account,
    FirebaseAdminSDK,
    Agora
}

export default agent;