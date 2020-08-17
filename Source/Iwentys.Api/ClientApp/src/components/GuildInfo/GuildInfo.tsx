import React from "react";
import { Card } from 'antd';

const { Meta } = Card;
interface IGuildInfoProps {
    title: string;
    bio: string;
    logoUrl?: string;
}
export const GuildInfo: React.FC<IGuildInfoProps> = (
    {title, bio, logoUrl = "https://wikipet.ru/wp-content/uploads/2018/01/486840.jpg"}) => {
    return (
        <section>
            <Card
                cover={<img alt="guild img" src={logoUrl} />}
            >
                <Meta title={title} description={bio} />
            </Card>
        </section>

    );
};
