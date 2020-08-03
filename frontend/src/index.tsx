import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import {App} from './App';
import {Provider} from 'react-redux';
import {configureStore, getDefaultMiddleware} from "@reduxjs/toolkit";
import {BrowserRouter} from 'react-router-dom';
import {guildSlice} from "./redux/guild/guildReducer";

const middleware = getDefaultMiddleware({
    thunk: true,
});

const store = configureStore({
    reducer: {
        guild: guildSlice.reducer,
    },
    middleware,
    devTools: process.env.NODE_ENV !== 'production',
});

ReactDOM.render(
    <Provider store={store}>
        <BrowserRouter>
            <App/>
        </BrowserRouter>
    </Provider>,
    document.getElementById('root')
);
