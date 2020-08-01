import React from 'react';
import './App.css';
import {Route} from "react-router-dom";
import SwaggerUI from "swagger-ui-react";
import "swagger-ui-react/swagger-ui.css";
import {GuildPage} from "./pages/GuildPage/GuildPage";

function App() {
    return (
        <>
            <Route path="/guild" component={GuildPage}/>
            <Route path="/profile" component={GuildPage}/>
            <Route exact path="/swagger" component={() =>
                <SwaggerUI url="https://iwentys.azurewebsites.net/swagger/v1/swagger.json"/>}/>
        </>
    );
}

export default App;
