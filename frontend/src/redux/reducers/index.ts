import {productReleasesFetched, productReleasesFetching} from "../actions";
import {createReducer} from "@reduxjs/toolkit";
// какой-то код из примера

const initialState = {
    productReleases: [],
    loadedProductRelease: null,
    fetchingState: 'none',
    creatingState: 'none',
    loadingState: 'none',
    error: null,
};

export const rootReducer = createReducer(initialState, {
    [productReleasesFetching.type]: (state, action) => {
        state.fetchingState = 'requesting'
    },
    [productReleasesFetched.type]: (state, action) => {
        state.productReleases = action.payload.productReleases;
        state.fetchingState = 'success';
    },
})
