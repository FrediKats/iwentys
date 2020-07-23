import React from "react";
import {Card, Row} from "antd";
import { MehOutlined, BugOutlined, CoffeeOutlined } from '@ant-design/icons';
import './Achievements.css';

const { Meta } = Card;
export const Achievements: React.FC = () => {
    return (
        <div>
            <h2>Achievements</h2>
            <Row>
                <Card
                    // @ts-ignore
                    cover={<MehOutlined />}

                >
                    <Meta title="achievement 1"/>
                </Card>
                <Card
                    // @ts-ignore
                    cover={<BugOutlined />}
                >
                    <Meta title="achievement 2"/>
                </Card>
                <Card
                    // @ts-ignore
                    cover={<CoffeeOutlined />}
                >
                    <Meta title="achievement 3"/>
                </Card>
            </Row>
        </div>
    );
}
