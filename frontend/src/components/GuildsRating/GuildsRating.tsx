import React from "react";
import {Table, Tag} from 'antd';

export const GuildsRating: React.FC = () => {
    const dataSource = [
        {
            key: '1',
            name: 'Fredi',
            contribution: 100500,
            tags: ['nice', 'developer'],
        },
        {
            key: '2',
            name: 'Cthulhu',
            contribution: 1050,
            tags: ['ya', 'pyos', 'woof'],
        },
        {
            key: '3',
            name: 'im2string',
            contribution: 42,
            tags: ['loser'],
        },
    ];

    const columns = [
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Tags',
            dataIndex: 'tags',
            key: 'tags',
            render: (tags: any) => (
                <>
                    {tags.map((tag:any) => {
                        let color = tag.length > 5 ? 'geekblue' : 'green';
                        if (tag === 'loser') {
                            color = 'volcano';
                        }
                        return (
                            <Tag color={color} key={tag}>
                                {tag.toUpperCase()}
                            </Tag>
                        );
                    })}
                </>
            ),
        },
        {
            title: 'Contribution',
            dataIndex: 'contribution',
            key: 'contribution',
        },
    ];

    return <Table dataSource={dataSource} columns={columns} />;
}
