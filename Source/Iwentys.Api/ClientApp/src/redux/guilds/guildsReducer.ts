import {createSlice} from "@reduxjs/toolkit";
import { getGuilds} from "./guildsThunk";
import {IState} from "../typings";

export const guildsSlice = createSlice({
    name: 'guild',
    initialState: {},
    reducers: {},
    // @ts-ignore
    extraReducers: {
        [getGuilds.rejected.type]: (state: IState) => {
            return Object.assign({}, state, {requestStatus: 'rejected'});
        },
        [getGuilds.pending.type]: (state: IState) => {
            return Object.assign({}, state, {requestStatus: 'pending'});
        },
        [getGuilds.fulfilled.type]: (state: IState, action: any) => {
            return Object.assign({}, state, {requestStatus: 'fulfilled', guildsList: action.payload});
        },
    }
});
