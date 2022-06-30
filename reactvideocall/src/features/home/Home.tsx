import { Button, Card, Col, Row } from "react-bootstrap";
import { useStore } from "../../stores/stores";
import { observer } from "mobx-react-lite";
import CallOneToOneModal from "../../common/components/CallOneToOne";

export default observer(function HomePage() {
    const { modalStore, userStore } = useStore();//<ModalContainer />
    return (
        <>
            <Row className='justify-content-center'>
                <Col md={8}>
                    <Card >
                        <Card.Img variant="top" src="/call-center.jpg" />
                        <Card.Body>
                            <Card.Title>Trung tâm chăm sóc khách hàng</Card.Title>
                            <Card.Text>
                                Cần hổ trợ, hãy bấm vào gọi tổng đâì
                            </Card.Text>
                            {userStore.user?.roles.includes('TongDaiVien') ? null : (
                                <Button variant="danger" onClick={() => modalStore.openModal("Call One-One", <CallOneToOneModal />)}>
                                    Gọi tổng đài
                                </Button>
                            )}
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </>
    );
})