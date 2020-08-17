import React from 'react';
import {Col, Row} from "antd";
import './UserInfo.scss';
import {Badge} from "../Badge/Badge";
import Avatar from "antd/es/avatar";

export interface IUserInfoProps {
    firstName: string;
    middleName: string,
    secondName: string,
    group: string;
    guildName: string | null;
    githubUsername: string | null;
    additionalLink: string | null;
}
export const UserInfo: React.FC<IUserInfoProps> = ({
        firstName,
        middleName,
        secondName,
        group,
        githubUsername,
        guildName,
        additionalLink
}) => {

    const badgesContent = ['👿top-5', '\u2721top-50'];

    return (
        <section className={'UserInfo'}>
            <Row>
                <Col span={8}>
                    <Avatar size={200} src={'https://wikipet.ru/wp-content/uploads/2018/01/486840.jpg'}/>
                </Col>
                <Col span={12}>
                    <Row className={'user-info title'}>
                        {githubUsername}
                    </Row>
                    <Row className={'user-info separator'}>
                        {firstName + ' ' + (middleName ? middleName + ' ' : '') + secondName}
                    </Row>
                    <Row className={'user-info'}>
                        Группа: {group}
                    </Row>
                    <Row className={'user-info'}>
                        Клан: {guildName}
                    </Row>
                    <Row className={'user-info'}>
                        Дополнительная ссылка: {additionalLink}
                    </Row>
                    <Row className={'badges-container'}>
                        <Badge content={badgesContent}/>
                    </Row>
                </Col>
            </Row>
        </section>
    );
}
