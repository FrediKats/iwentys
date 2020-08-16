import React from "react";
import {Row, Col, Spin} from "antd";
import {PageLayout } from "../../components/PageLayout/PageLayout";
import {NewsFeed} from "../../components/NewsFeed/NewsFeed";
import {useDispatch, useSelector} from "react-redux";
import {IState} from "../../redux/typings";
import {StudyDashboard} from "../../components/StudyDashboard/StudyDashboard";
import {CodingDashboard} from "../../components/CodingDashboard/CodingDashboard";
import {UserInfo} from "../../components/UserInfo/UserInfo";
import {getUserById} from "../../redux/user/userThunk";
import {Achievements} from "../../components/Achievements/Achievements";

export const UserPage: React.FC = () => {

    const dispatch = useDispatch();

    React.useEffect(() => {
        dispatch(getUserById(289140));
    }, [dispatch]);

    const user = useSelector((state: IState) => state.user);
    let userPageContent: JSX.Element;

    switch (user.requestStatus) {
        case 'fulfilled':
            userPageContent = (
                <Row>
                    <Col span={12}>
                        <UserInfo
                            firstName={user.firstName}
                            middleName={user.middleName}
                            secondName={user.secondName}
                            group={user.group}
                            githubUsername={user.githubUsername}
                            additionalLink={user.additionalLink}
                        />
                        <NewsFeed/>
                    </Col>
                    <Col span={12}>
                        <Achievements achievements={user.achievements}/>
                        <CodingDashboard codingActivityInfo={user.codingActivityInfo}/>
                        <StudyDashboard subjectActivityInfo={user.subjectActivityInfo}/>
                    </Col>
                </Row>
            );
            break;
        case 'rejected':
            userPageContent = <h2>Не удалось загрузить страницу. Попробуйте ещё раз позже</h2>;
            break;
        default:
            userPageContent = <Spin size='large'/>;
            break;
    }

    return (
        <PageLayout>
            {userPageContent}
        </PageLayout>
    );
};
