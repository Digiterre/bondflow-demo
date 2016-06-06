import {IApplicationState} from "./interfaces/IApplicationState";
import * as React from 'react';
import {connect} from 'react-redux';
import Dispatch = Redux.Dispatch;
import { refreshBonds } from './redux/BondActions';
import { bondService } from './services/BondService';
import { BondError } from './BondError';

class BondErrorList extends React.Component<any, any> {
    constructor(props:any) {
        super(props);
    }

    public static mapStateToProps(state : any) : any {
        var appState = state as IApplicationState;
        return {
            bonds: appState.bonds,
            bondService: bondService
        };
    }

    AsyncActionToRefreshBonds(view : BondErrorList) {
        return function (dispatch : Dispatch) {
            return view.props.bondService.getBondErrors().then(
                bondArray => dispatch(refreshBonds(bondArray.$values)),
                error => {
                });
        }
    }

    componentDidMount() {
        setInterval(() => this.props.dispatch(this.AsyncActionToRefreshBonds(this)), 3000);
    }
    
    render() {
        var renderedBonds = this.props.bonds.map((b) => {
            return <BondError key={b.Identifier} bond={b} dispatch={this.props.dispatch} />;
        });
        return <div>{renderedBonds}</div>;
    }
}

export default connect(BondErrorList.mapStateToProps)(BondErrorList);