import React from 'react';
import {NavLink} from "react-router-dom";
import {Menu} from 'antd';
import {Layout} from 'antd';
import 'antd/dist/antd.css';
import './PageLayout.css';
import {useRouteMatch} from "react-router";

const {Header, Content} = Layout;

export const PageLayout: React.FC = (props) => {
    const match = useRouteMatch();
    const routes = [{
        name: 'profile',
        title: 'Профиль'
    }, {
        name: 'guild',
        title: 'Моя гильдия'
    }, {
        name: 'guilds-rating',
        title: 'Рейтинг гильдий'
    }];
    const currPage = routes.find(route => match.url === '/'+ route.name);

    return (
        <Layout className="layout">
            <Header>
                <div className="logo"/>
                <Menu theme="dark" mode="horizontal" defaultSelectedKeys={['profile']} selectedKeys={currPage && [currPage.name]}>
                    {routes.map(route => (
                        <Menu.Item key={route.name}>
                            <NavLink to={'/' + route.name} key={'link' + route.name}>
                                {route.title}
                            </NavLink>
                        </Menu.Item>)
                    )}
                </Menu>
            </Header>
            <Content>
                {props.children}
            </Content>
        </Layout>)
};
