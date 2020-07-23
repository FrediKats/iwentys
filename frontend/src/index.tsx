import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import { Provider } from 'react-redux';
import { configureStore } from "@reduxjs/toolkit";
import {rootReducer} from "./redux/reducers";
import { BrowserRouter } from 'react-router-dom';

const store = configureStore({
      reducer: rootReducer,
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
