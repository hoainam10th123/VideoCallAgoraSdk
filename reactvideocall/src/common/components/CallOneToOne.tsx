import AgoraRTC from "agora-rtc-sdk-ng";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Col, Row } from "react-bootstrap";
import useAgora from "../../hooks/useAgora";
import MediaPlayer from "./MediaPlayer";
import { useStore } from "../../stores/stores";
import agent from "../../api/agent";
import { toast } from "react-toastify";

const client = AgoraRTC.createClient({ codec: 'h264', mode: 'rtc' });

export default observer(function CallOneToOneModal() {
    const {
        localAudioTrack, localVideoTrack, leave, join, joinState, remoteUsers
    } = useAgora(client);

    const { userStore } = useStore();

    useEffect(() => {
        const chanelName = userStore.channelName ?? userStore.user?.username;
        const model = {channel: chanelName, username: userStore.user?.username};

        agent.Agora.getRtcToken({ uid: userStore.user?.username, channelName: chanelName }).then(token => {
            try {
                join('9c29102f9b5749988c092d4d9bab52e9', chanelName, token, userStore.user?.username);                
                agent.FirebaseAdminSDK.timUserTongDai(JSON.stringify(model));//notification toi tong dai vien
            } catch (error) {
                console.error(error);
                toast.error('FirebaseAdminSDK.timUserTongDai error');
            }
        }).catch((err) => {
            console.error('An error occurred while retrieving token. ', err);
            toast.error('error while get token agora');
        });

        return () => { 
            leave();
            userStore.setChannel(null, null);
            agent.FirebaseAdminSDK.setFinishCalling(false);
        }

    }, [])

    return (
        <Row>
            <Col>
                {joinState && localVideoTrack ? (
                    <MediaPlayer label={`Local ${client.uid?.toString()!}`} videoTrack={localVideoTrack} audioTrack={localAudioTrack}></MediaPlayer>
                ) : null}
            </Col>
            <Col>
                {remoteUsers.map(user => (
                    <MediaPlayer key={user.uid} label={`Remote ${user.uid.toString()}`} videoTrack={user.videoTrack} audioTrack={user.audioTrack}></MediaPlayer>
                ))}
            </Col>
        </Row>
    )
})