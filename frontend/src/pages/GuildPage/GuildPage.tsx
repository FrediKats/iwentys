import React from "react";
import {Row, Col} from 'antd';
import {NewsFeed} from "../../components/NewsFeed/NewsFeed";
import {Achievements} from "../../components/Achievements/Achievements";
import {GuildInfo} from "../../components/GuildInfo/GuildInfo";
import {GuildsRating} from "../../components/GuildsRating/GuildsRating";
import {useDispatch, useSelector} from "react-redux";
import {getGuildById} from "../../redux/thunks/guildThunk";
import {IState} from "../../redux/typings";

interface IGuildPageProps {
}

export const GuildPage: React.FC<IGuildPageProps> = () => {
    const dispatch = useDispatch();

    React.useEffect(() => {
        dispatch(getGuildById(1));
    }, [dispatch]);

    const guildData = useSelector((state: IState) => state.guild);

    return (
        <Row justify="space-between">
            <Col span={6}>
                <GuildInfo title={guildData.title} bio={guildData.bio}/>
            </Col>
            <Col span={12}>
                <Achievements/>
                <NewsFeed/>
            </Col>
            <Col span={6}>
                <GuildsRating/>
            </Col>
        </Row>
    );
};
