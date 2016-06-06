import * as React from 'react';
import {Panel, Grid, Row, Col, Button, Input} from 'react-bootstrap'
import { bondService } from './services/BondService'
import Dispatch = Redux.Dispatch;
import { refreshBonds } from './redux/BondActions';


export class BondError extends React.Component<any, any> {

    constructor(props:any) {
        super(props);
        this.state = {
          midPrice: this.props.bond.MidPrice
        };
    }

    updateState(view:BondError, transform:(state:any) => any) {
        view.setState(transform(this.state));
    }

    handleChange(event) {
        this.updateState(this, (st) => {
            switch (event.target.id.toString()) {
                case "price":
                    st.midPrice = event.target.value;
                    break;
                default :
                    break;
            }
            return st;
        });
    }

    getClassName(value : string) : string {
        if (value === null || value === undefined || value === "") {
            return "large-bottom-padding";
        }
        else {
            return "small-bottom-padding";
        }

    }

    ActionToResendBond(props:any, view:BondError) {
        return function (dispatch:Dispatch) {
            return bondService.handleBond(props.bond)
                .then(() => setTimeout(() => {
                    bondService.getBondErrors()
                        .then(bondArray => {
                                dispatch(refreshBonds(bondArray.$values));
                            },
                            error => {
                            })
                }, 500), error => {
                });
        }
    }
    resend() {
        this.props.bond.MidPrice = this.state.midPrice;
        this.props.dispatch(this.ActionToResendBond(this.props, this));
    }
    render() {
        return <Panel>
            <Grid fluid></Grid>
            <Row className="row">
                <Col className="no-cell" lg={3} md={3} sm={3}>
                    <div className="no-cell"><b>Identifier:</b></div>
                    <div className="no-cell small-left-padded">{this.props.bond.Identifier}</div>
                </Col>
                <Col className="no-cell" lg={3} md={3} sm={3}>
                    <div className="no-cell"><b>Name:</b></div>
                    <div className="no-cell small-left-padded">{this.props.bond.Name}</div>
                </Col>
                <Col className="no-cell" lg={3} md={3} sm={3}>
                    <div className="no-cell"><b>Type:</b></div>
                    <div className="no-cell small-left-padded">{this.props.bond.Type}</div>
                </Col>
                <Col className="cell" lg={2} md={2} sm={2}>
                    <Input bsSize="small" id="price"
                           type="number"
                           label="Mid Price" value={this.state.midPrice}
                           onChange={this.handleChange.bind(this)}/>
                </Col>
                <Col className="cell" lg={1} md={1} sm={1}>
                    <div className="button-padding">
                        <Button bsSize="small" onClick={this.resend.bind(this)} >ReSend</Button>
                    </div>
                </Col>
            </Row>
            </Panel>;
    }
}