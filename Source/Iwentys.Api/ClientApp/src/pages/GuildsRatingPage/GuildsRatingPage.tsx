import React from "react";
import {useDispatch, useSelector} from "react-redux";
import {IState} from "../../redux/typings";
import {PageLayout} from "../../components/PageLayout/PageLayout";
import {getGuilds} from "../../redux/guilds/guildsThunk";
import Spin from "antd/lib/spin";
import {GuildCard} from "../../components/GuildCard/GuildCard";
import './GuildsRatingPage.css';

export const GuildsRatingPage: React.FC = () => {
    const dispatch = useDispatch();

    React.useEffect(() => {
        dispatch(getGuilds());
    }, [dispatch]);

    const guilds = useSelector((state: IState) => state.guilds);
    let pageContent: JSX.Element;

    switch (guilds.requestStatus) {
        case 'fulfilled':
            pageContent = (
                <div className={'GuildsRatingPage'}>
                    <h2>Рейтинг гильдий</h2>
                    <ul>
                        {guilds.guildsList.map(guild => <GuildCard key={guild.id} {...guild}/>)}
                    </ul>
                </div>
            );
            break;
        case 'rejected':
            pageContent = <h2>Не удалось загрузить страницу. Попробуйте ещё раз позже</h2>;
            break;
        default:
            pageContent = <Spin size='large'/>;
            break;
    }

    return (
        <PageLayout>
            {pageContent}
        </PageLayout>
    );
};
