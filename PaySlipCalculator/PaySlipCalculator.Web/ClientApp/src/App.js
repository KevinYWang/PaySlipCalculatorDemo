import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Introduction } from './components/Introduction';
import { Calculator } from './components/Calculator';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Calculator} />
            <Route path='/Introduction' component={Introduction} />
      </Layout>
    );
  }
}
