import React from "react";
import {Row, Col, Spin} from 'antd';
import {NewsFeed} from "../../components/NewsFeed/NewsFeed";
import {Achievements} from "../../components/Achievements/Achievements";
import {GuildInfo} from "../../components/GuildInfo/GuildInfo";
import {GuildLeaderboard} from "../../components/GuildsRating/GuildLeaderboard";
import {useDispatch, useSelector} from "react-redux";
import {getGuildById} from "../../redux/guild/guildThunk";
import {IState} from "../../redux/typings";
import {PageLayout} from "../../components/PageLayout/PageLayout";
import {PinnedRepositories} from "../../components/PinnedRepositories/PinnedRepositories";
import {useParams} from "react-router";


export const GuildPage: React.FC = () => {
    const dispatch = useDispatch();
    const { guildId } = useParams();
    React.useEffect(() => {
        dispatch(getGuildById(guildId || 1)); // временная заглушка
    }, [dispatch, guildId]);

    const guild = useSelector((state: IState) => state.guild);
    let guildPageContent: JSX.Element;

    switch (guild.requestStatus) {
        case 'fulfilled':
            guildPageContent = (
                <Row justify="space-between">
                    <Col span={6}>
                        <GuildInfo title={guild.title} bio={guild.bio} logoUrl={guild.logoUrl}/>
                    </Col>
                    <Col span={12}>
                        <Achievements achievements={guild.achievements}/>
                        <NewsFeed/>
                    </Col>
                    <Col span={6}>
                        <GuildLeaderboard
                            totalRate={guild.memberLeaderBoard.totalRate}
                            members={guild.memberLeaderBoard.members}
                            contribution={guild.memberLeaderBoard.membersImpact}
                        />
                        <PinnedRepositories pinnedRepositories={guild.pinnedRepositories.filter(Boolean)}/>
                    </Col>
                </Row>
            );
            break;
        case 'rejected':
            guildPageContent = <h2>Не удалось загрузить страницу. Попробуйте ещё раз позже</h2>;
            break;
        default:
            guildPageContent = <Spin size='large'/>;
            break;
    }

    return (
        <PageLayout>
            {guildPageContent}
        </PageLayout>
    );
};
