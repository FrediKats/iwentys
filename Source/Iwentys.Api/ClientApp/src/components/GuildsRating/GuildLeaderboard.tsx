import React from "react";
import {Table} from 'antd';
import {IMemberLeaderBoard} from "../../redux/typings";

export interface IGuildLeaderboardProps {
    totalRate: IMemberLeaderBoard['totalRate'];
    members: IMemberLeaderBoard['members'];
    contribution: IMemberLeaderBoard['membersImpact'];
}

export const GuildLeaderboard: React.FC<IGuildLeaderboardProps> = ({totalRate = 0, members, contribution}) => {
    if (!members) return null;
    const dataSource = members.map((member, idx) => ({
        key: idx,
        name: member.firstName,
        contribution: 0,
    }));

    const columns = [
        {
            title: 'Имя',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Вклад',
            dataIndex: 'contribution',
            key: 'contribution',
        },
    ];

    return (
        <section>
            <h2>Суммарный вклад: {totalRate}</h2>
            <Table dataSource={dataSource} columns={columns}/>
        </section>);
}
