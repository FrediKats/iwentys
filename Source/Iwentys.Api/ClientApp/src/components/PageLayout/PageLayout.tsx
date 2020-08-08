import React from 'react';
import { NavLink } from "react-router-dom";
import {Menu} from 'antd';
import {Layout} from 'antd';
import 'antd/dist/antd.css';
import './PageLayout.css';

const {Header, Content} = Layout;

export const PageLayout:React.FC = (props) =>  (
        <Layout className="layout">
            <Header>
                <div className="logo"/>
                <Menu theme="dark" mode="horizontal" defaultSelectedKeys={['1']}>
                    <Menu.Item key="1">
                        <NavLink to={'guild'} key={'guild'}>
                            Гильдия
                        </NavLink>
                    </Menu.Item>
                    <Menu.Item key="2">
                        <NavLink to={'profile'} key={'profile'}>
                            Профиль
                        </NavLink>
                    </Menu.Item>
                </Menu>
            </Header>
            <Content>
                {props.children}
            </Content>
        </Layout>
    );
