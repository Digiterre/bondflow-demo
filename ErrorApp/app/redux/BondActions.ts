/**
 * Created by simonh on 06/06/2016.
 */

import {IBond} from '../interfaces/IBond';
import {createAction} from 'redux-actions';
import * as actionTypes from './ActionTypes';

const refreshBonds = createAction<IBond[]>(actionTypes.REFRESH_BONDS, (bonds : IBond[]) => bonds);

export {
    refreshBonds
}