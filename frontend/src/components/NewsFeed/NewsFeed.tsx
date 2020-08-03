import React from "react";
import {Comment, Tooltip, List} from 'antd';
import './NewsFeed.css';

export const NewsFeed: React.FC = () => {

    const data = [
        {
            actions: [<span key="comment-list-reply-to-0">Reply to</span>],
            author: 'Han Solo',
            avatar: 'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
            content: (
                <p>
                    We supply a series of design principles, practical patterns and high quality design
                    resources (Sketch and Axure), to help people create their product prototypes beautifully and
                    efficiently.
                </p>
            ),
            datetime: (
                <Tooltip
                    title={'date-1'}
                >
        <span>
          {(new Date()).toDateString()}
        </span>
                </Tooltip>
            ),
        },
        {
            actions: [<span key="comment-list-reply-to-0">Reply to</span>],
            author: 'Han Solo',
            avatar: 'https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png',
            content: (
                <p>
                    We supply a series of design principles, practical patterns and high quality design
                    resources (Sketch and Axure), to help people create their product prototypes beautifully and
                    efficiently.
                </p>
            ),
            datetime: (
                <Tooltip
                    title={'date-2'}
                >
        <span>
          {(new Date()).toDateString()}
        </span>
                </Tooltip>
            ),
        },
    ];
    return (
         <section className={'NewsFeed'}>
        <h2>Новостная лента</h2>
        <List
            className="comment-list"
            itemLayout="horizontal"
            dataSource={data}
            renderItem={item => (
                <li>
                    <Comment
                        actions={item.actions}
                        author={item.author}
                        avatar={item.avatar}
                        content={item.content}
                        datetime={item.datetime}
                    />
                </li>
            )}
        />
        </section>
    );
}
