import React from "react";
import { Row, Col } from 'antd';
import {NewsFeed} from "../../components/NewsFeed/NewsFeed";
import {Achievements} from "../../components/Achievements/Achievements";
import {GuildInfo} from "../../components/GuildInfo/GuildInfo";
import {GuildsRating} from "../../components/GuildsRating/GuildsRating";

export const GuildPage: React.FC = () => {
    return (
        <Row justify="space-between">
            <Col span={6}>
                <GuildInfo/>
            </Col>
            <Col span={12}>
                <Achievements/>
                <NewsFeed/>
            </Col>
            <Col span={6}>
                <GuildsRating/>
            </Col>
        </Row>
    );
}
