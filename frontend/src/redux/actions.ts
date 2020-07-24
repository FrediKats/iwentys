import {createAction} from "@reduxjs/toolkit";
// какой-то код из примера
// ещё примеры редакса https://redux.js.org/advanced/example-reddit-apihttps://redux.js.org/advanced/example-reddit-api

export const productReleasesFetching =
    createAction('PRODUCT_RELEASES_FETCHING');
export const productReleasesFetched =
    createAction('PRODUCT_RELEASES_FETCHED');
export const productReleasesFetchingError =
    createAction('PRODUCT_RELEASES_FETCHING_ERROR');

export function fetchProductReleases() {
    // @ts-ignore
    return dispatch => {
        dispatch(productReleasesFetching());
        return dispatch(productReleasesFetched());
    }
}
