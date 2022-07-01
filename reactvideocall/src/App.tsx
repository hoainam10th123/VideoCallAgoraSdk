import './App.css';
import { toast, ToastContainer } from 'react-toastify';
import ModalContainer from './common/modals/ModalContainer';
import { Button, Container } from 'react-bootstrap';
import MenuBar from './common/components/MenuBar';
import { Route, Routes } from 'react-router-dom';
import NotFound from './common/components/NotFound';
import Login from './features/user/Login';
import PrivateWrapper from './common/components/PrivateRoute';
import TestErrors from './features/errors/TestErrors';
import ModalMakeACallAnswer from './common/modals/ModalMakeACallAnswer';
import { useStore } from './stores/stores';
import { observer } from 'mobx-react-lite';
import { useEffect, useRef, useState } from 'react';
import { openCallingAnswerService } from './common/services/openCallingAnswer';
import CallOneToOneModal from './common/components/CallOneToOne';
import HomePage from './features/home/Home';
import ServerErrorView from './features/errors/ServerError';

import { initializeApp } from "firebase/app";
import { getMessaging, getToken, onMessage } from "firebase/messaging";
import { getAnalytics } from "firebase/analytics";
import agent from './api/agent';
import HopTHoaiNgheCuocGoi from './common/components/HopTHoaiNgheCuocGoi';

const firebaseConfig = {
  apiKey: "AIzaSyAc3fshoOQlbRUdckjate3k1mKCGTcRtvI",
  authDomain: "chat-app-react-66942.firebaseapp.com",
  projectId: "chat-app-react-66942",
  storageBucket: "chat-app-react-66942.appspot.com",
  messagingSenderId: "674025401519",
  appId: "1:674025401519:web:d3bfa3d0bafb0041a1f85a",
  measurementId: "G-MN29NVRH75"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);

//const auth = getAuth(app);
//const provider = new GoogleAuthProvider();

const messaging = getMessaging(app);

function App() {
  const { userStore, modalMakeACallAnswer, modalStore } = useStore();  
  const buttonCallOneToOneRef = useRef<any>(null);
  const buttonNgheCuocGoiRef = useRef<any>(null);
  const [token, setToken] = useState('');

  // Things to do before unloading/closing the tab
  const disconnectedBeforeUnload = async () => {
    await agent.FirebaseAdminSDK.userDisconnected();
  }

  // Setup the `beforeunload` event listener
  const setupBeforeUnloadListener = () => {
    window.addEventListener("beforeunload", (ev) => {
      ev.preventDefault();
      return disconnectedBeforeUnload();
    });
  };

  useEffect(() => {
    if (userStore.user) {
      // user thoat trinh duyet thi remove user o backend, de ngan khong nhan cuoc goi ao
      setupBeforeUnloadListener();
      //token cua firebase
      getToken(messaging, { vapidKey: 'BIRizgVLNDLwllmYcH6NbY1kSyaXkS29uGUOaHKa1PyJ3a3W-BaXiYtRS1y9I425xDfsrTtvYzRQkZpbNzSrlwE' }).then((currentToken) => {
        if (currentToken) {
          setToken(currentToken);
          agent.FirebaseAdminSDK.addToken(currentToken).then(() => toast.success('add token FCM thanh cong'))
        } else {
          // Show permission request UI
          console.log('No registration token available. Request permission to generate one.');
        }
      }).catch((err) => {
        console.error('An error occurred while retrieving token. ', err);
        toast.error('error while get token');
      });
    }

    // open modal call 1-1
    openCallingAnswerService.getMessage().subscribe(data => {
      if (data) buttonNgheCuocGoiRef.current.click();
    })
    //Foreground
    onMessage(messaging, (payload) => {
      console.log('Message received. ', payload);
      if(payload.data!.usernameTo === userStore.user?.username){
        if(payload.data!.channel){
          userStore.setChannel(payload.data!.channel);
          modalMakeACallAnswer.openModal(payload.notification?.body!); 
        }else{
          toast.info(payload.notification?.body!);
        }
      }
    });

  }, [userStore.user])

  return (
    <>
      <Button style={{ display: 'none' }} onClick={() => modalStore.openModal(`Call from ${userStore.user?.username}`, <CallOneToOneModal />)} ref={buttonCallOneToOneRef}>Call 1-1</Button>
      <Button style={{ display: 'none' }} onClick={() => modalStore.openModal(`Call from ${userStore.user?.username}`, <HopTHoaiNgheCuocGoi />)} ref={buttonNgheCuocGoiRef}>Call 1-1</Button>

      <ToastContainer position='bottom-right' hideProgressBar theme='colored' />
      <ModalMakeACallAnswer />
      <ModalContainer />

      <MenuBar />
      <Container>
        <div>{token}</div>
        <Routes>
          <Route index element={
            (<PrivateWrapper><HomePage /></PrivateWrapper>)
          } />

          <Route path='/login' element={<Login />} />

          <Route path='/errors' element={<TestErrors />} />

          <Route path='/server-error' element={<ServerErrorView />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </Container>
    </>
  );
}

export default observer(App);
