import React from "react";
import { Card } from 'antd';

const { Meta } = Card;
interface IGuildInfoProps {
    title: string;
    bio: string;
}
export const GuildInfo: React.FC<IGuildInfoProps> = ({title, bio}) => {
    return (
        <div>
            <Card
                cover={<img alt="guild img" src="https://wikipet.ru/wp-content/uploads/2018/01/486840.jpg" />}
            >
                <Meta title={title} description={bio} />
            </Card>
        </div>

    );
};
