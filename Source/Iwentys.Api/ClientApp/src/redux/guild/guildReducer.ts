import {createSlice} from "@reduxjs/toolkit";
import {getGuildById} from "./guildThunk";
import {IState} from "../typings";

export const guildSlice = createSlice({
    name: 'guild',
    initialState: {},
    reducers: {},
    // @ts-ignore
    extraReducers: {
        [getGuildById.rejected.type]: (state: IState) => {
            return Object.assign({}, state, {requestStatus: 'rejected'});
        },
        [getGuildById.pending.type]: (state: IState) => {
            return Object.assign({}, state, {requestStatus: 'pending'});
        },
        [getGuildById.fulfilled.type]: (state: IState, action: any) => {
            return Object.assign({}, state, {requestStatus: 'fulfilled', ...action.payload});
        },
    }
});
