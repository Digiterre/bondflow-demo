/**
 * Created by simonh on 06/06/2016.
 */
import {IBond} from './IBond'
import {Promise} from 'es6-promise'
import {IBondArray} from "./IBondArray";

export interface IBondService {
    getBondErrors(): Promise<IBondArray>
    handleBond(bond : IBond) : Promise<void>
}