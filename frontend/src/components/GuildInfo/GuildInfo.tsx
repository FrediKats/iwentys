import React from "react";
import { Card } from 'antd';

const { Meta } = Card;
export const GuildInfo: React.FC = () => {
    return (
        <div>
            <Card
                cover={<img alt="guild img" src="https://wikipet.ru/wp-content/uploads/2018/01/486840.jpg" />}
            >
                <Meta title="My guild" description="www.woofwoof.ru" />
            </Card>
        </div>

    );
};
