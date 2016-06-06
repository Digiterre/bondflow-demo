/**
 * Created by simonh on 06/06/2016.
 */
import {Promise} from 'es6-promise'
import * as $ from 'jquery';
import {IBond} from "../interfaces/IBond";
import {IBondService} from "../interfaces/IBondService";
import {IBondArray} from "../interfaces/IBondArray";

export class BondService implements IBondService {
    public getBondErrors() : Promise<IBond[]> {
        return new Promise<IBondArray>((success, error) => {
            $.ajax({
                async: true,
                method: "GET",
                dataType: "json",
                url: "http://localhost:9990/BondError",
                data: {},
                xhrFields: {withCredentials: true},
                crossDomain: true
            }).then(success, error);
        });
    }
    
    public handleBond(bond : IBond) : Promise<void> {
        return new Promise<void>((success, error) => {
            $.ajax({
                url: "http://localhost:9990/BondError",
                type: 'PUT',
                dataType: 'json',
                data: bond,
                xhrFields: { withCredentials: true },
                crossDomain: true
            }).then(success, error);
        });
    }
}

export var bondService : IBondService = new BondService();