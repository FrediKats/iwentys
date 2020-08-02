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

export interface IState {
    guild: IGuildState;
}
