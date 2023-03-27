// import { Table, TableBody, TableCell, TableRow } from '@mui/material';
import classNames from 'classnames/bind';

import React, { useEffect, useState } from 'react';
import {
    Checkbox,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableFooter,
    TableHead,
    TableRow,
} from '@mui/material';

// import { makeStyles } from '@mui/styles';

import styles from './PublisherCourses.module.scss';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import config from '~/config';

// const useStyles = makeStyles({
//     table: {
//         minWidth: 650,
//     },
//     footer: {
//         // display: 'flex',
//     },
//     backBtn: {
//         marginRight: '10',
//     },
//     nextBtn: {},
// });

const cx = classNames.bind(styles);

function PublisherCourses() {
    const [isChecked, setIsChecked] = useState(false);
    const [data, setData] = useState([]);
    const [selected, setSelected] = useState([]);
    const [page, setPage] = useState(0);
    const [pageSize, setPageSize] = useState(5);

    let params = useParams();
    const navigate = useNavigate();
    const accessToken = localStorage.getItem('accessToken');

    const fetchCourses = async () => {
        try {
            const [approvedCoursesResponse, notApprovedCoursesResponse] = await Promise.all([
                axios.get(`${config.baseUrl}/api/courses`, {
                    params: {
                        PublisherUserId: params.PublisherUserId,
                        IsApproved: true,
                        PageSize: 9999,
                    },
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                }),
                axios.get(`${config.baseUrl}/api/courses`, {
                    params: {
                        PublisherUserId: params.PublisherUserId,
                        IsApproved: false,
                        PageSize: 9999,
                    },
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                }),
            ]);

            const combinedData = [...approvedCoursesResponse.data.queried, ...notApprovedCoursesResponse.data.queried];
            const sortedData = combinedData.sort((a, b) => a.courseId - b.courseId);
            setData(sortedData);
        } catch (error) {
            console.error(error);
        }
    };
    useEffect(() => {
        fetchCourses();
    }, []);

    const handleSelectAll = (event) => {
        if (event.target.checked) {
            setSelected(data.map((item) => item.courseId));
            return;
        }
        setSelected([]);
    };

    const handleSelect = (courseId) => {
        const selectedIndex = selected.indexOf(courseId);
        let newSelected = [];

        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selected, courseId);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selected.slice(1));
        } else if (selectedIndex === selected.length - 1) {
            newSelected = newSelected.concat(selected.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(selected.slice(0, selectedIndex), selected.slice(selectedIndex + 1));
        }

        setSelected(newSelected);
    };

    const isSelected = (courseId) => selected.indexOf(courseId) !== -1;

    const handleNextPage = () => {
        setPage(page + 1);
    };

    const handleBackPage = () => {
        setPage(page - 1);
    };

    const handleEditCourse = (courseId) => {
        navigate(`/publisher/${params.PublisherUserId}/edit-course/${courseId}`);
    };

    const handleDeleteCourse = async (courseId) => {
        await axios.delete(`${config.baseUrl}/api/courses/${courseId}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`,
            },
        });
        fetchCourses();
    };

    const handleChange = (event) => {
        setIsChecked(event.target.checked);
    };

    // const classes = useStyles();
    const currentData = data.slice(page * pageSize, (page + 1) * pageSize);

    return (
        <div className={cx('wrapper')}>
            <p className={cx('title')}>Courses</p>
            <TableContainer component={Paper}>
                <Table className={cx('table')} aria-label="custom table">
                    <TableHead>
                        <TableRow>
                            <TableCell padding="checkbox">
                                <Checkbox
                                    indeterminate={selected.length > 0 && selected.length < data.length}
                                    checked={data.length > 0 && selected.length === data.length}
                                    onChange={handleSelectAll}
                                />
                            </TableCell>
                            <TableCell>Title</TableCell>
                            <TableCell align="right">Description</TableCell>
                            <TableCell align="right">Status</TableCell>
                            <TableCell align="right">Topic</TableCell>
                            <TableCell align="right">Course Tier</TableCell>
                            <TableCell align="right">Action</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {currentData.map((row) => (
                            <TableRow key={row.courseId}>
                                <TableCell>
                                    <input
                                        type="checkbox"
                                        checked={selected.includes(row.courseId)}
                                        onChange={handleChange}
                                        onClick={() => handleSelect(row.courseId)}
                                    />
                                </TableCell>
                                <TableCell component="th" scope="row">
                                    {row.title}
                                </TableCell>
                                <TableCell align="right">{row.description}</TableCell>
                                <TableCell align="right">{row.isApproved ? 'Approved' : 'Pending'}</TableCell>
                                <TableCell align="right">{row.topicTitle}</TableCell>
                                <TableCell align="right">{row.tier === 0 ? 'Free' : 'Premium'}</TableCell>
                                <TableCell align="right">
                                    <button
                                        style={{ marginRight: '6px' }}
                                        onClick={() => handleEditCourse(row.courseId)}
                                    >
                                        Edit
                                    </button>
                                    <button style={{ color: 'red' }} onClick={() => handleDeleteCourse(row.courseId)}>
                                        Delete
                                    </button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                    <TableFooter>
                        <TableRow className={cx('footer')}>
                            <TableCell align="left">*{selected.length} data selected</TableCell>

                            <TableCell className={cx('nextBtn')}>
                                {page > 0 && <button onClick={handleBackPage}>Prev</button>}
                                {(page + 1) * pageSize < data.length && <button onClick={handleNextPage}>Next</button>}
                            </TableCell>
                        </TableRow>
                    </TableFooter>
                </Table>
            </TableContainer>
        </div>
    );
}

export default PublisherCourses;
