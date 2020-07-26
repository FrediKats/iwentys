import React from 'react';
import './App.css';
import {BrowserRouter as Router, NavLink, Route} from "react-router-dom";
import {GuildPage} from "./pages/GuildPage/GuildPage";
import {Menu} from 'antd';
import {Layout} from 'antd';
import 'antd/dist/antd.css';

const {Header, Content} = Layout;

function App() {
    // TODO: поправить баг с линками: не открывается нужная страница при нажатии на кнопку
    return (
        <Layout className="layout">
            <Header>
                <div className="logo"/>
                <Menu theme="dark" mode="horizontal" defaultSelectedKeys={['1']}>
                    <Menu.Item key="1">
                        <NavLink to={'guild'} key={'guild'}>
                            Guild
                        </NavLink>
                    </Menu.Item>
                    <Menu.Item key="2">
                        <NavLink to={'profile'} key={'profile'}>
                            Profile
                        </NavLink>
                    </Menu.Item>
                </Menu>
            </Header>
            <Content>
                <Router>
                    <Route path="/guild" component={GuildPage}/>
                    <Route path="/profile" component={GuildPage}/>
                </Router>
            </Content>
        </Layout>
    );
}

export default App;
