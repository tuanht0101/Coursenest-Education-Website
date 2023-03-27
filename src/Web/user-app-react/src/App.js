import { Routes, Route } from 'react-router-dom';
import './App.css';
import { useState } from 'react';

import SignIn from '~/pages/SignIn';
import SignUp from '~/pages/SignUp';
import Forgot from '~/pages/Forgot';
import Landing from './pages/Landing Page/Landing';
import Home from './pages/Home/Home';
import Profile from './pages/Profile/Profile';
import Layout from './pages/Layout/Layout';
import Instructor from '~/pages/Instructor';
import Following from '~/pages/Instructor/Following';
import Pending from '~/pages/Instructor/Pending';
import History from '~/pages/Instructor/History';
import Publisher from '~/pages/Publisher/Publisher';
import PublisherCourses from './pages/Publisher/PublisherCourses';
import AddCourses from './pages/Publisher/AddCourses';
import Topic from './pages/Topic/Topic';
import Course from './pages/Course/Course/Course';
import Material from './pages/Course/Material/Material';
import Exam from './pages/Course/Exam/Exam';
import MyCourses from './pages/MyCourses/MyCourses';
import EnrolledCourse from './pages/Course/EnrolledCourse/EnrolledCourse';

import axios from 'axios';
import config from './config';
import EditLesson from './pages/Publisher/AddCourses/EditLesson';
import EditMaterial from './pages/Publisher/AddCourses/EditLesson/EditMaterial';
import getNumberOfDays from './helper/getNumberOfDays';

function App() {
    let logged = false;
    const accessToken = localStorage.getItem('accessToken');
    const userId = localStorage.getItem('userId');

    const [isInstructor, setIsInstructor] = useState(false);
    const [isPublisher, setIsPublisher] = useState(false);

    const checkInstructor = (role) => {
        if (role.type === 1 && getNumberOfDays(role.expiry) > 0) {
            return true;
        }
        return false;
    };

    const checkPublisher = (role) => {
        if (role.type === 2 && getNumberOfDays(role.expiry) > 0) {
            return true;
        }
        return false;
    };

    const getRoleMe = () => {
        axios
            .get(`${config.baseUrl}/api/roles/me`, {
                headers: { Authorization: `Bearer ${accessToken}` },
            })
            .then((res) => {
                const roles = res.data;
                if (roles.find(role => checkInstructor(role))) {
                    setIsInstructor(true);
                }
                if (roles.find(role => checkPublisher(role))) {
                    setIsPublisher(true);
                }
            })
            .catch((err) => {
                console.log(err);
            });
    };

    if (accessToken && userId) {
        logged = true;
        getRoleMe();
    }

    return (
        <div className="App">
            <Routes>
                <Route path="landing" element={<Landing />} />
                <Route path="sign-in" element={<SignIn />} />
                <Route path="sign-up" element={<SignUp />} />
                <Route path="forgot-password" element={<Forgot />} />
                <Route
                    path="/"
                    element={<Layout logged={logged} isInstructor={isInstructor} isPublisher={isPublisher} />}
                >
                    <Route index element={<Home logged={logged} />} />
                    <Route path="courses/:id" element={<Course logged={logged} />}></Route>
                    <Route path="topics/:id" element={<Topic />} />
                    {logged && (
                        <>
                            <Route path="profile" element={<Profile />} />
                            <Route path="my-courses" element={<MyCourses />} />
                            <Route path="enrolled-course/:enrollementId" element={<EnrolledCourse />}>
                                <Route path="material/:materialId" element={<Material />} />
                                <Route path="exam/:examId" element={<Exam />} />
                            </Route>
                            {isInstructor && (
                                <Route path="instructor" element={<Instructor />}>
                                    <Route index element={<Following />}></Route>
                                    <Route path="following" element={<Following />}></Route>
                                    <Route path="pending" element={<Pending />}></Route>
                                    <Route path="history" element={<History />}></Route>
                                </Route>
                            )}
                            {isPublisher && (
                                <Route path="publisher" element={<Publisher />}>
                                    {/* <Route index element={<PublisherCourses />}></Route> */}
                                    <Route path=":PublisherUserId" element={<PublisherCourses />}></Route>
                                    {/* <Route path="courses" element={<PublisherCourses />}></Route> */}
                                    <Route
                                        path=":PublisherUserId/add-course"
                                        element={<AddCourses isEditCourse={false} />}
                                    ></Route>
                                    <Route
                                        path=":PublisherUserId/add-course/add-lesson"
                                        element={<EditLesson />}
                                    ></Route>
                                    <Route
                                        path=":PublisherUserId/add-course/add-lesson/edit-material"
                                        element={<EditMaterial />}
                                    ></Route>
                                    <Route
                                        path=":PublisherUserId/edit-course/:courseId"
                                        element={<AddCourses isEditCourse={true} />}
                                    ></Route>
                                </Route>
                            )}
                        </>
                    )}
                </Route>
            </Routes>
        </div>
    );
}

export default App;
