import {createSlice} from "@reduxjs/toolkit";
import {getGuildById} from "./guildThunk";

export const guildSlice = createSlice({
    name: 'guild',
    initialState: {},
    reducers: {},
    extraReducers: {
        [getGuildById.fulfilled.type]: (state, action) => {
            return Object.assign({}, state, action.payload);
        },
    }
});
