import NavDropdown from "react-bootstrap/NavDropdown";
import { NavLink } from "react-router-dom";
import { useLocation } from "react-router-dom";
import instance from "../../api/request";

import styles from "./Header.module.css";


export default function Header() {
    const pathname = useLocation().pathname;

    const handleSignOut = () => {
        localStorage.clear();
        instance
            .post(`authenticate/logout`)
            .then((res) => {
            })
            .catch((err) => {
                console.log(err);
            })
    }
    return (
        <div className={styles.container}>
            <div className={styles.navLeft}>
                <NavLink to='/' className={`${styles.navLink} ${pathname === '/' ? styles.active : ''}`} >
                    Users
                </NavLink>
                <NavLink to="/categories" className={`${styles.navLink} ${pathname === '/categories' ? styles.active : ''}`}>
                    Categories
                </NavLink>
                <NavLink to="/courses" className={`${styles.navLink} ${pathname === '/courses' ? styles.active : ''}`}>
                    Courses
                </NavLink>
            </div>
            <div className={styles.navRight}>
                <NavDropdown title="Admin">
                    <NavDropdown.Item href="/profile">
                        My Profile
                    </NavDropdown.Item>
                    <NavDropdown.Divider />
                    <NavDropdown.Item href="/" onClick={() => handleSignOut()}>
                        Sign Out
                    </NavDropdown.Item>
                </NavDropdown>
            </div>
        </div>
    );
}
