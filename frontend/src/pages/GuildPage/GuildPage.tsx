import React from "react";
import {Row, Col} from 'antd';
import {NewsFeed} from "../../components/NewsFeed/NewsFeed";
import {Achievements} from "../../components/Achievements/Achievements";
import {GuildInfo} from "../../components/GuildInfo/GuildInfo";
import {GuildLeaderboard} from "../../components/GuildsRating/GuildLeaderboard";
import {useDispatch, useSelector} from "react-redux";
import {getGuildById} from "../../redux/guild/guildThunk";
import {IState} from "../../redux/typings";
import {PageLayout} from "../../components/PageLayout/PageLayout";
import {PinnedRepositories} from "../../components/PinnedRepositories/GuildInfo";

interface IGuildPageProps {
}

export const GuildPage: React.FC<IGuildPageProps> = () => {
    const dispatch = useDispatch();

    React.useEffect(() => {
        dispatch(getGuildById(1));
    }, [dispatch]);
    // TODO: add guild fetching state
    const guild = useSelector((state: IState) => state.guild);
    if(!guild.title) return null;

    return (
        <PageLayout>
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
                <PinnedRepositories pinnedRepositories={guild.pinnedRepositories}/>
            </Col>
        </Row>
        </PageLayout>
    );
};
