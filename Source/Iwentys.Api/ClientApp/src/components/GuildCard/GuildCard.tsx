import React from "react";
import Avatar from "antd/es/avatar";
import './GuildCard.css';
import {IGuildState} from "../../redux/typings";

interface IGuildCardProps extends IGuildState {
}

export const GuildCard: React.FC<IGuildCardProps> = (
    {title, bio, logoUrl = "https://wikipet.ru/wp-content/uploads/2018/01/486840.jpg", id}) => {
    return (
        <li className={'GuildCard-Wrapper'}>
            <a href={`guild/${id}`} className={'GuildCard'}>
                <Avatar size={100} src={logoUrl}/>
                <div className={'GuildCard-Description'}>
                    <h3>{title}</h3>
                    <span>{bio}</span>
                </div>
                {/*тут что-то будет но пока не понятно что именно*/}
                <div className={'GuildCard-Stats'}>
                    <span/>
                </div>
            </a>
        </li>
    );
};
