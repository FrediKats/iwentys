import React from "react";
import {Card, Row} from "antd";
import {CoffeeOutlined} from '@ant-design/icons';
import './Achievements.css';
import {IAchievement} from "../../redux/typings";

const {Meta} = Card;

export interface IAchievementsProps {
    achievements: IAchievement[];
}

export const Achievements: React.FC<IAchievementsProps> = ({achievements = []}) => {
    return (
        <div>
            <h2>Achievements</h2>
            <Row>
                {achievements.slice(0, 2).map((achievement) => (
                    <Card cover={<CoffeeOutlined/>} key={achievement.name}>
                        <Meta title={achievement.name} description={achievement.description}/>
                    </Card>))
                }
            </Row>
        </div>
    );
};
