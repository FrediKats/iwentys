import * as fetchService from '../fetchService';
import { createAsyncThunk} from "@reduxjs/toolkit";

export const getGuilds = createAsyncThunk('guild/getGuilds', async () => {
    const data = await fetchService.get(`Guild`);
    return data;
});
