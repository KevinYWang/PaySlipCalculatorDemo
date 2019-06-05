import React, { Component } from 'react';

export class Calculator extends Component {
    static displayName = Calculator.name;

    constructor(props) {
        super(props);
        this.state = {
            paySlips: [],
            loading: false,
            downloadFileFormat: 'csv',
            uploadFile: null
        };
    }

    uploadDataFile() {

        this.setState({ loading: true });
        let fileSelector = document.getElementById('fileinput');
        if (fileSelector === null || fileSelector.files === null || fileSelector.files.length < 1) {
            alert("Please select a file to upload!")
            return false;
        }

        let file = fileSelector.files[0]
        let formData = new FormData();
        formData.append('file', file);

        let hasHttpError = false; 
        fetch("Home/RawFileUpload",
                {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json'
                    },
                    body: formData
            })
            .then(function (response) {
                if (!response.ok) {
                    alert("Error in importing data, please check the data file.  ");
                    hasHttpError = true; 
                }
                return response;
            })
            .then(response => hasHttpError?response:response.json())
            .then(res => {
                if (!hasHttpError) {
                this.setState({ paySlips: res.data, uploadFile: null});
                    fileSelector.value = "";
                }
                this.setState({ loading: false});
            })
    }

    downloadPaySlipFile() {
        if (this.state.paySlips === null || this.state.paySlips.length === 0) {
            alert("No  data to export!");
            return false;
        }
        let fileFormat = this.state.downloadFileFormat;
        let baseUrl = "Home/GetPaySlipFile?fileFormat=" + fileFormat;
        window.location.href = baseUrl;
    }

    downloadFileFormatChanged(event) {
        this.setState({ downloadFileFormat: event.target.value });
    }

    static renderPaySlipTable(paySlips) {
        return (
                  <
            table
        className = 'table table-striped' >  < thead >  < tr >  < th > Name < /
        th >  < th > Payperiod < /
        th >  < th > Gross
        income < /
        th >  < th > Income
        tax < /
        th >  < th > Net
        income < /
        th >  < th > Super < /
        th >  < /
        tr >  < /
        thead >  < tbody >
            { paySlips.map((p, i) =>
        <
        tr
        key = { i } >  < td > { p.name } < /
        td >  < td > { p.payPeriod } < /
        td >  < td > { p.grossIncome } < /
        td >  < td > { p.incomeTax } < /
        td >  < td > { p.netIncome } < /
        td >  < td > { p.superAnnunation } < /
        td >  < /
        tr > 
    )
}

</
tbody >  < /
table > 
);
}

render()
{
    let contents;
    if (!this.state.loading && this.state.paySlips.length === 0)
        contents = "No data";
    else
        contents = this.state.loading
            ?  <
    p><
    em > Loading...</
    em></
    p > 
    :
    Calculator.renderPaySlipTable(this.state.paySlips);


    return (
              <
        div >
         <
        div
    className = "flex-row" >  < div
    className = "row" >  < div
    className = "mt-2 mr-1 ml-3" >  < label > Upload
    Salary
    File(csv / Excel):</
    label >  < /
    div >  < input
    type = "file"
    className = "border h-25 mt-1"
    id = "fileinput" /  >  < button
    className = "btn btn-primary btn-sm h-25 ml-2 mt-1"
    onClick = { () =>
    this.uploadDataFile()
}>
Upload < /
button >  < /
div >  < /
div >  < p
className = "border-top border-light">
</
p >  < div
className = "row" >  < div
className = "col-2 mb-0 pb-0">
<
h4 > Payslips < /
h4 >  < /
div >  < div
className = "col-10 d-flex flex-row-reverse w-100 float-right mb-1 pb-0" >  < button
className = "btn btn-primary btn-sm m-1"
onClick = { () =>
this.downloadPaySlipFile()} >
Export < /
button >  < select
className = "mt-1"
value = { this.state.downloadFileFormat }
onChange = { (event) =>
this.downloadFileFormatChanged(event)}>
<
option
value = "csv" > CSV < /
option >  < option
value = "xls" > Excel
97 - 2003 < /
option >  < option
value = "xlsx" > Excel
2007 +  < /
option >  < /
select >  < /
div >  < /
div >  < p
className = "border-top border-grey">
</
p >
    { contents } <
/
div > 
);
}
}