import {createSlice} from "@reduxjs/toolkit";
import {getGuildById} from "./guildThunk";

export const guildSlice = createSlice({
    name: 'guild',
    initialState: {},
    reducers: {},
    extraReducers: {
        [getGuildById.rejected.type]: (state) => {
            return Object.assign({}, state, {requestStatus: 'rejected'});
        },
        [getGuildById.pending.type]: (state) => {
            return Object.assign({}, state, {requestStatus: 'pending'});
        },
        [getGuildById.fulfilled.type]: (state, action) => {
            return Object.assign({}, state, {requestStatus: 'fulfilled', ...action.payload});
        },
    }
});
