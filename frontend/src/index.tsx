import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import { Provider } from 'react-redux';
import { configureStore } from "@reduxjs/toolkit";
import {rootReducer} from "./redux/reducers";
import { BrowserRouter as Router, Route } from 'react-router-dom';

const store = configureStore({
      reducer: rootReducer,
        devTools: process.env.NODE_ENV !== 'production',
});

ReactDOM.render(
    <Provider store={store}>
        <Router>
            <Route path="/" component={App} />
            <Route path="/guild" component={App} />
            <Route path="/profile" component={App} />
        </Router>
    </Provider>,
  document.getElementById('root')
);
