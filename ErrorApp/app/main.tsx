/// <reference path="../typings/tsd.d.ts" />
import * as React from 'react'
import * as ReactDom from 'react-dom'
import {store} from "./redux/Store";
import {Provider } from 'react-redux';
import { PageHeader } from 'react-bootstrap';
import BondErrorList from './BondErrorList';


class Main extends React.Component<{}, {}> {
    render() {
        return <Provider store={store}>
            <div>
                <PageHeader className="center-align" >Bond Errors</PageHeader>
                <BondErrorList />
            </div>
            </Provider>
    }
}

ReactDom.render(<Main />, document.getElementById("reactTarget"));