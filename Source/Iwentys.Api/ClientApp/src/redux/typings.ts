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
    user: IUserState;
}

export interface IUserState {
    requestStatus: string;
    group: string;
    achievements: IAchievement[];
    subjectActivityInfo: ISubjectActivityInfo[];
    codingActivityInfo: ICodingActivityInfo[];
    id: 0;
    firstName: string;
    middleName: string;
    secondName: string;
    role: string;
    githubUsername: string;
    creationTime: Date;
    lastOnlineTime: Date;
    barsPoints: 0;
    guildName: string;
    studyLeaderBoardPlace: number;
    codingLeaderBoardPlace: number;
    socialStatus: string;
    additionalLink: string;
}

export interface ISubjectActivityInfo {
    subjectTitle: string;
    points: number;
}

export interface ICodingActivityInfo {
    month: string;
    activity: number;
}
