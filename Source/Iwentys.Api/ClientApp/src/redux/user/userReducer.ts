import {createSlice} from "@reduxjs/toolkit";
import {getUserById} from "./userThunk";

export const userSlice = createSlice({
    name: 'user',
    initialState: {},
    reducers: {},
    extraReducers: {
        [getUserById.rejected.type]: (state) => {
            return Object.assign({}, state, {requestStatus: 'rejected'});
        },
        [getUserById.pending.type]: (state) => {
            return Object.assign({}, state, {requestStatus: 'pending'});
        },
        [getUserById.fulfilled.type]: (state, action) => {
            return Object.assign({}, state, {requestStatus: 'fulfilled', ...action.payload});
        },
    }
});
