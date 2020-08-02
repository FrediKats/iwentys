import React from "react";
import {IRepository} from "../../redux/typings";
import { List, Avatar } from 'antd';

interface IPinnedRepositoriesProps {
    pinnedRepositories: IRepository[];
}
export const PinnedRepositories: React.FC<IPinnedRepositoriesProps> = ({pinnedRepositories}) => {
    // todo: star count
    return (
        <section>
            <h2>Pinned Repositories</h2>
            <List
                itemLayout="horizontal"
                dataSource={pinnedRepositories}
                renderItem={item => (
                    <List.Item>
                        <List.Item.Meta
                            avatar={<Avatar src={item.url} />}
                            title={<a href={item.url}>{item.name}</a>}
                            description={item.description}
                        />
                    </List.Item>
                )}
            />
        </section>

    );
};
