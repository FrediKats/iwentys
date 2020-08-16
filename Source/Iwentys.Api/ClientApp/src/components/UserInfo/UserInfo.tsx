import React from 'react';
import { Card } from 'antd';

const { Meta } = Card;

export interface IUserInfoProps {
    firstName: string;
    middleName: string,
    secondName: string,
    group: string;
    githubUsername: string;
    additionalLink: string | null;
}
export const UserInfo: React.FC<IUserInfoProps> = ({
        firstName,
        middleName,
        secondName,
        group,
        githubUsername,
        additionalLink
}
) => {
    return (
        <div>
            <Card
                cover={
                    <img
                        alt="user avatar"
                        src={'https://wikipet.ru/wp-content/uploads/2018/01/486840.jpg'}
                    />
                }
            >
                <Meta
                    title={firstName + ' ' + (middleName ? middleName + ' ' : '') + secondName}
                    description={group}
                />
            </Card>
        </div>
    );
}
