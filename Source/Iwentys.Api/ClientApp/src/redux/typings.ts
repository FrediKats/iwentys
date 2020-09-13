export interface IAchievement{
    name: string;
    description: string;
    url: string;
}

export interface IStudent{
    barsPoints: number;
    creationTime: Date;
    firstName: string;
    githubUsername: string;
    group: string;
    id: number;
    lastOnlineTime: Date;
    middleName: string;
    role: string;
    secondName: string;
}

export interface IMemberLeaderBoard {
    members: IStudent[];
    membersImpact: Record<string, object> // ???
    totalRate: string;
}

export interface IRepository {
    id: number;
    name: string;
    description: string;
    starCount: number;
    url: string;
}

export interface IGuildState {
    id: number;
    requestStatus: string;
    title: string;
    bio: string;
    logoUrl: string;
    tribute: number | null;
    achievements: IAchievement[];
    leader: IStudent;
    totem: IStudent;
    memberLeaderBoard: IMemberLeaderBoard;
    pinnedRepositories: IRepository[];
}

export interface IGuildsState {
    requestStatus: string;
    guildsList: IGuildState[]
}

export interface IState {
    guild: IGuildState;
    guilds: IGuildsState;
}
