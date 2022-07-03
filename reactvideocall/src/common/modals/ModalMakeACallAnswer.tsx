import { observer } from "mobx-react-lite";
import { useEffect, useRef } from "react";
import { Button, Modal } from "react-bootstrap";
import agent from "../../api/agent";
import { useStore } from "../../stores/stores";
import { openCallingAnswerService } from "../services/openCallingAnswer";

export default observer(function ModalMakeACallAnswer() {
    const { modalMakeACallAnswer, audioStore, userStore } = useStore();
    const buttonPlayAudioRef = useRef<any>(null);

    function traLoiCuocGoi() {
        agent.FirebaseAdminSDK.setFinishCalling(true);
        audioStore.setPlaying(false);
        modalMakeACallAnswer.closeModal();
        // mo hop thoai call 1-1
        openCallingAnswerService.sendMessage(true);//app.tsx        
    }

    useEffect(() => {
        //tuong tac dom 1 cach gian tiep, tuong tu nhu user click vao button play
        //su dung buoc trung gian nay de ngan ngua loi: play() failed because the user didn't interact with the document first.        
        //buttonPlayAudioRef.current.click();//play sound
    }, [])

    return (
        <>
            <Button style={{ display: 'none' }} onClick={audioStore.toogle} ref={buttonPlayAudioRef}>Play</Button>
            <Modal
                show={modalMakeACallAnswer.open}
                onHide={modalMakeACallAnswer.closeModal}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{modalMakeACallAnswer.title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    Would you like to answer this call?
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="danger" onClick={() => {
                        agent.FirebaseAdminSDK.denyCall(userStore.username!).then(()=> {
                            userStore.setChannel(null, null);
                        });
                                                
                        agent.FirebaseAdminSDK.setFinishCalling(false);
                        //audioStore.setPlaying(false);
                        modalMakeACallAnswer.closeModal();
                    }}>
                        Cancel
                    </Button>
                    <Button variant="success" onClick={traLoiCuocGoi}>Answer</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
})