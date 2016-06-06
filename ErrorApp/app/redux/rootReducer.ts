/**
 * Created by simonh on 06/06/2016.
 */
import bonds from './bonds'
import { combineReducers } from 'redux'

const rootReducer = combineReducers({
    bonds : bonds
});

export { rootReducer }