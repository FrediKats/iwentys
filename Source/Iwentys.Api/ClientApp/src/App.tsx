import React from 'react';
import {Route} from "react-router-dom";
import SwaggerUI from "swagger-ui-react";
import "swagger-ui-react/swagger-ui.css";
import {GuildPage} from "./pages/GuildPage/GuildPage";
import {GuildsRatingPage} from "./pages/GuildsRatingPage/GuildsRatingPage";

export class App extends React.PureComponent {
    state = {
        error: false,
    };

    componentDidCatch(error: object) {
        this.setState({error: true})
        console.log(error);
    }
    render(){
        if(this.state.error){
            return <h2>Кажется что-то сломалось</h2>;
        }

        return (
            <>
                <Route exact path="/" component={GuildPage}/>
                <Route path="/guild" component={GuildPage}/>
                <Route path="/guild/:guildId" component={GuildPage}/>
                <Route path="/profile" component={GuildPage}/>
                <Route path="/guilds-rating" component={GuildsRatingPage}/>
                <Route exact path="/swagger" component={() =>
                    <SwaggerUI url="https://iwentys.azurewebsites.net/swagger/v1/swagger.json"/>}/>
            </>
        );
    }
}

