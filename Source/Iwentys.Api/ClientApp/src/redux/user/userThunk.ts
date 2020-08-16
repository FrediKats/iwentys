import * as fetchService from '../fetchService';
import {createAsyncThunk} from "@reduxjs/toolkit";

export const getUserById = createAsyncThunk('user/getUserProfileById', async (id: string | number) => {
    const data = await fetchService.get(`Student/${id}`, {userId: id});
    return data;
});
