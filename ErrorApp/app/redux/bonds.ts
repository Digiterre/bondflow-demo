/**
 * Created by simonh on 06/06/2016.
 */
/// <reference path="../../typings/tsd.d.ts" />
import {IBond} from "../interfaces/IBond";
import {initialBonds} from "./initialState";
import {handleActions, Action } from 'redux-actions';
import * as actionTypes from './ActionTypes';

export default handleActions<IBond[], IBond[]>({
    [actionTypes.REFRESH_BONDS]: (state : IBond[], action : Action<IBond[]>) : IBond[] => {
        return action.payload;
    }
}, initialBonds);