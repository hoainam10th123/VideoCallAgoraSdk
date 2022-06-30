// Scripts for firebase and firebase messaging
importScripts('https://www.gstatic.com/firebasejs/9.2.0/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.2.0/firebase-messaging-compat.js');

// Initialize the Firebase app in the service worker by passing the generated config
const firebaseConfig = {
    apiKey: "AIzaSyAc3fshoOQlbRUdckjate3k1mKCGTcRtvI",
    authDomain: "chat-app-react-66942.firebaseapp.com",
    projectId: "chat-app-react-66942",
    storageBucket: "chat-app-react-66942.appspot.com",
    messagingSenderId: "674025401519",
    appId: "1:674025401519:web:d3bfa3d0bafb0041a1f85a",
    measurementId: "G-MN29NVRH75"
  };

firebase.initializeApp(firebaseConfig);

// Retrieve firebase messaging
const messaging = firebase.messaging();

messaging.onBackgroundMessage(function(payload) {
  console.log('[firebase-messaging-sw.js] Received background message ', payload);
  // Customize notification here
  const notificationTitle = payload.notification?.title;
  const notificationOptions = {
    body: payload.notification?.body,
    icon: '/firebase-logo.png'
  };

  self.registration.showNotification(notificationTitle, notificationOptions);
});

/* messaging.onMessage((payload) => {
  console.log('Message received. ', payload);
}); */