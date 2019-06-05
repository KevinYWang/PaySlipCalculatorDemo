import React, { Component } from 'react';

export class Introduction extends Component {
    static displayName = Introduction.name;

  render () {
    return (
      <div>
        <h5>Hi, there!</h5>
        <p>Welcome to this simple solution to MYOB Payslip Calculator. </p>
        <p>This demo is featured with the following tech stack:</p>
        <ul>
          <li>The back-end is powered by net core (2.2) MVC;</li>
          <li>The back-end has 100% unit testing coverage (using xunit, moq and Autofixture); </li>
          <li>The front-end uses react.js SPA; </li>          
          <li>Dependency Injection is enhanced by Autofac;</li>
          <li>Paring and exporting data spreadsheet is supported by Gembox.</li>
        </ul>
        
      </div>
    );
  }
}
