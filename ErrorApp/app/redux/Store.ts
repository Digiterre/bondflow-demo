/**
 * Created by simonh on 06/06/2016.
 */
import {Store, createStore, applyMiddleware} from 'redux';
import {globalInitialState} from "./initialState";
import thunk = require("redux-thunk");
import {rootReducer} from "./rootReducer";

export var store: Store = createStore(rootReducer, globalInitialState, applyMiddleware(thunk.default));