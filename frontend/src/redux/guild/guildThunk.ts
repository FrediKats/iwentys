import * as fetchService from '../fetchService';
import { createAsyncThunk} from "@reduxjs/toolkit";

export const getGuildById = createAsyncThunk('guild/getGuildById', async (id: string | number) => {
    const data = await fetchService.get(`Guild/${id}`, {userId: 1});
    return data;
});
