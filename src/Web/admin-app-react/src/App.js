import { Routes, Route, Outlet } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';

import { useLocation } from 'react-router-dom';
import Login from './components/Login/Login';
import Header from "./components/Header/Header";
import ManageUsers from "./components/ManageUsers/ManageUsers";
import DisplayAdminInfo from "./components/DisplayAdminInfo/DisplayAdminInfo";
import ListCategories from "./components/ListCategories/ListCategories";
import ManageCourses from "./components/ManageCourses/ManageCourses";
import Units from "./components/UnapprovedCourse/Units/Units";
import Exam from "./components/UnapprovedCourse/Exam/Exam";
import Material from "./components/UnapprovedCourse/Material/Material";
import styles from "./App.module.css";
import UnapprovedCourse from "./components/UnapprovedCourse/UnapprovedCourse/UnapprovedCourse";

const setToken = (adminToken) => {
    localStorage.setItem('accessToken', adminToken);
}

function App() {

    let logged = false;

    localStorage.getItem("accessToken") ? logged=true : logged=false;

    const path = useLocation().pathname;
    const location = path.split('/')[1];

    return (
        <div>
            {!logged && <>
                <Routes>
                    <Route index element={<Login setAccessToken={setToken}/>} />
                    <Route path="login" element={<Login setAccessToken={setToken}/>} />
                </Routes>
            </>}
            {logged && 
            <div className={styles[`${location}`]}>
                <Header />
                <Outlet />
                <Routes>
                    <Route exact path="/" element={<ManageUsers />} />
                    <Route exact path="/profile" element={<DisplayAdminInfo/>} />
                    <Route exact path="/categories" element={<ListCategories />} />
                    <Route exact path="/courses" element={<ManageCourses />} />
                    <Route exact path="/courses/:id" element={<UnapprovedCourse />} >
                        <Route index element={<Units />} />
                        <Route path="material/:materialId" element={<Material />} />
                        <Route path="exam/:examId" element={<Exam />} />
                    </Route>
                    <Route path="*" element={<p>Path not resolved</p>} />
                </Routes>
            </div>}
        </div>
    );
}
export default App;