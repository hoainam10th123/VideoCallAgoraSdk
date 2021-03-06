import { observer } from "mobx-react-lite";
import { Container, Dropdown, Nav, Navbar } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useStore } from "../../stores/stores";

export default observer(function MenuBar() {
    const { userStore } = useStore();
    const { isLoggedIn, user, logout } = userStore;
    return (
        <Navbar bg="primary" variant="dark">
            <Container>
                <Navbar.Brand href="/">Agora video call and firebase (FCM)</Navbar.Brand>

                <Nav className="me-auto">
                    <Nav.Link as={Link} to='/login'>Login</Nav.Link>
                    <Nav.Link as={Link} to='/errors'>Errors</Nav.Link>
                </Nav>

                {isLoggedIn ? (
                    <div className="d-none d-sm-block">
                        <Dropdown>
                            <Dropdown.Toggle id="dropdown-button-dark-example1" variant="Primary">
                                <img src='/user.png' height="25" alt='img user' className="rounded" />
                                <span style={{color: 'white'}}>{user?.displayName}</span>
                            </Dropdown.Toggle>
                            <Dropdown.Menu variant="dark">
                                <Dropdown.Item as={Link} to={`/profile/${user?.username}`}>
                                    My profile
                                </Dropdown.Item>
                                <Dropdown.Divider />
                                <Dropdown.Item onClick={logout}>Log out</Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                ) : null}

            </Container>
        </Navbar>
    )
})