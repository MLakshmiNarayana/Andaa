var ServerIPAddress = {
    //ServerIP: '202.153.40.154:6565'
    ServerIP: 'localhost'
};


var GlobalClass = {
    CCode: 0,
    SId: 0,
    UserName: '',
    AccIds: '',
    ProdIds: '',
    ClaimTypes: '',
    AccJSON: '',
    ProdJSON: ''
};

var CallBackVariables = {
    AgeingArrRequest: []
}

var Focus8WAPI = {
    ENUMS: {
        MODULE_TYPE: {
            MASTER: 1,
            TRANSACTION: 2,
            UI: 3,
            GLOBAL: 4
        },

        REQUEST_TYPE: {
            GET: 1,
            SET: 2,
            CONTINUE: 3
        },

        REQUEST_TYPE_UI: {
            SET_POPUP_COORDINATE: 1,
            OPEN_POPUP: 2,
            CLOSE_POPUP: 3,
            GOTOHOMEPAGE: 4
        }
    },

    getFieldValue: function (sCallbackFn, Field, iModuleType, isFieldId, iRequestId) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: 0,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.GET,
                objData: { fieldid: Field },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, false) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getFieldValue " + err.message);
        }
    },

    setFieldValue: function (sCallbackFn, Field, Value, iModuleType, isFieldId, iRequestId) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: 0,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.SET,
                objData: { fieldid: Field, value: Value },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, false) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.setFieldValue " + err.message);
        }
    },

    getBodyFieldValue: function (sCallbackFn, Field, iModuleType, isFieldId, iRowIndex, iRequestId) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: iRowIndex,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.GET,
                objData: { fieldid: Field },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, true) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getBodyFieldValue " + err.message);
        }
    },

    setBodyFieldValue: function (sCallbackFn, Field, Value, iModuleType, isFieldId, iRowIndex, iRequestId) {
        var obj = null;

        try {
            obj = {
                moduleType: iModuleType,
                rowIndex: iRowIndex,
                isFieldId: isFieldId,
                requestType: Focus8WAPI.ENUMS.REQUEST_TYPE.SET,
                objData: { fieldid: Field, value: Value },
                iRequestId: iRequestId,
                sCallbackFn: sCallbackFn
            };

            if (Focus8WAPI.PRIVATE.isValidInput(obj, true) == true) {
                Focus8WAPI.PRIVATE.postMessage(obj);
            }
        }
        catch (err) {
            alert("Exception: Focus8WAPI.setBodyFieldValue " + err.message);
        }
    },

    continueModule: function (iModuleType, result) {
        var obj = null;
        try {
            obj = {};
            obj.moduleType = iModuleType;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE.CONTINUE;
            obj.result = result;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.continueModule " + err.message);
        }
    },

    openPopup: function (url, sCallback) {
        var obj = null;

        try {
            if (Focus8WAPI.PRIVATE.isNullOrEmpty(url, true) == true) {
                return (false);
            }

            obj = {};
            obj.URL = url;
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.OPEN_POPUP;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.openPopup " + err.message);
        }

        return (true);
    },

    closePopup: function () {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.CLOSE_POPUP;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.closePopup " + err.message);
        }
    },

    gotoHomePage: function () {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.GOTOHOMEPAGE;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.gotoHomePage " + err.message);
        }
    },

    setPopupCoordinates: function (scenter, sTop, sWidth, sHeight) {
        var obj = null;
        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.UI;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE_UI.SET_POPUP_COORDINATE;
            obj.center = scenter;
            obj.Top = sTop;
            obj.Width = sWidth;
            obj.Height = sHeight;
            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.openPopup " + err.message);
        }

        return (true);
    },

    getGlobalValue: function (sCallbackFn, sVariable, iRequestId) {
        var obj = null;

        try {
            obj = {};
            obj.moduleType = Focus8WAPI.ENUMS.MODULE_TYPE.GLOBAL;
            obj.requestType = Focus8WAPI.ENUMS.REQUEST_TYPE.GET;
            obj.Variable = sVariable;
            obj.iRequestId = iRequestId;
            obj.sCallbackFn = sCallbackFn;

            Focus8WAPI.PRIVATE.postMessage(obj);
        }
        catch (err) {
            alert("Exception: Focus8WAPI.getGlobalValue " + err.message);
        }
    },

    PRIVATE: {
        isValidInput: function (obj, bBodyField) {
            try {
                if (Focus8WAPI.PRIVATE.isValidObject(obj.moduleType) == false || obj.moduleType.toString() == "") {
                    alert("Validation Exception: Please pass Module Type parameter");

                    return (false);
                }

                if (Focus8WAPI.PRIVATE.isValidObject(obj.isFieldId) == false || obj.isFieldId.toString() == "") {
                    alert("Validation Exception: Please pass isFieldId parameter");

                    return (false);
                }

                if (Focus8WAPI.PRIVATE.isValidObject(obj.objData.fieldid) == false) {
                    alert("Validation Exception: Please pass Field parameter");

                    return (false);
                }
                else {
                    if (Focus8WAPI.PRIVATE.isValidObject(obj.objData.fieldid.length) == true) {
                        if (obj.objData.fieldid.length == 0) {
                            alert("Validation Exception: Please pass Field parameter");

                            return (false);
                        }
                    }
                    else if (obj.objData.fieldid.toString() == "") {
                        alert("Validation Exception: Please pass Field parameter");

                        return (false);
                    }
                }


                if (bBodyField == true) {
                    if (Focus8WAPI.PRIVATE.isValidObject(obj.rowIndex) == false || isNaN(obj.rowIndex)) {
                        alert("Validation Exception: Row Index should be number type");

                        return (false);
                    }

                    if (obj.rowIndex <= 0) {
                        alert("Validation Exception: Row Index should be greater than 0 for Body Fields");

                        return (false);
                    }
                }
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.isValidInput} " + err.message);
            }

            return (true);
        },

        postMessage: function (obj) {
            try {
                obj.FromClient = true;
                window.parent.postMessage(obj, "*");
            }
            catch (err) {
                alert("Exception: Focus8WAPI.PRIVATE.postMessage " + err.message);
            }
        },

        onReceiveMessage: function (evt) {
            var objReturnData = null;
            var obj = null;

            try {
                Focus8WAPI.PRIVATE.stopKeyProcess(evt);
                objReturnData = evt.data;

                // Client                
                if (Focus8WAPI.PRIVATE.isValidObject(objReturnData.FromClient) == true) {
                    return;
                }

                console.log('Focus8WAPI::Received Response: ', JSON.stringify(objReturnData));

                if (Focus8WAPI.PRIVATE.isNullOrEmpty(objReturnData.sCallbackFn, true) == false) {
                    obj = {};
                    obj.returnCode = objReturnData.response.lValue;
                    obj.message = objReturnData.response.sValue;
                    obj.data = objReturnData.response.data;
                    obj.fieldId = objReturnData.fieldId;
                    obj.requestType = objReturnData.requestType;
                    obj.moduleType = objReturnData.moduleType;
                    obj.iRequestId = objReturnData.iRequestId;

                    if (Focus8WAPI.PRIVATE.isValidObject(objReturnData.RowsInfo) == true) {
                        obj.RowsInfo = objReturnData.RowsInfo;
                    }

                    eval(objReturnData.sCallbackFn)(obj);
                }
            }
            catch (err) {
                alert("Exception: Focus8WAPI.PRIVATE.onReceiveMessage " + err.message);
            }
        },

        isValidObject: function (obj) {
            try {
                if (typeof obj == "undefined" || obj == null) {
                    return (false);
                }

                return (true);
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.isValidObject} " + err.message);
            }

            return (false);
        },

        isNullOrEmpty: function (sValue, bTrim) {
            var bResult = false;

            try {
                if (Focus8WAPI.PRIVATE.isValidObject(sValue) == false || (typeof sValue).toLowerCase() != "string" || sValue.length <= 0) {
                    return (true);
                }

                if (Focus8WAPI.PRIVATE.isValidObject(bTrim) == true && bTrim == true) {
                    if (sValue.trim().length == 0) {
                        return (true);
                    }
                }
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.isNullOrEmpty} " + err.message);
                bResult = true;
            }

            return (bResult);
        },

        stopKeyProcess: function (evt) {
            try {
                if (Focus8WAPI.PRIVATE.isValidObject(evt) == false) {
                    return;
                }

                if (evt.preventDefault) {
                    evt.preventDefault();
                }
                else {
                    evt.returnValue = false;
                }

                if (evt.bubbles == true) {
                    evt.stopPropagation();
                }
            }
            catch (err) {
                alert("Exception: {Focus8WAPI.PRIVATE.stopKeyProcess} " + err.message);
            }
        }
    }

}
window.addEventListener('message', Focus8WAPI.PRIVATE.onReceiveMessage);

function getSetPropertyForMastersField() {
    Focus8WAPI.getGlobalValue("fnGetValueCallBack", "", 1);
}

function fnGetValueCallBack(objWrapperResult) {
    debugger;
    var responseData = objWrapperResult.data;
    if (objWrapperResult.requestType == 1) {
        if (objWrapperResult.iRequestId == 1) {
            GlobalClass.SId = responseData.SessionId;
            GlobalClass.CCode = responseData.CompanyId;
            GlobalClass.UserName = responseData.UserName;
        }
    }




}


//$(document).ready(function () {

//});

function Pct(value, min, max) {
    if (parseInt(value) < 0 || isNaN(value))
        return "";
    else if (parseInt(value) > 100)
        return "";
    else return value;
}


function SaveScheme() {
    debugger;
    var IsError = false;
    var txtTargetAmt = 0;
    var txtSchemePct = 0;
    var txtSchemeName = '';
    var SchemeFlag = 0;
    var FromDate = '';
    var ToDate = '';
    var FDate = '';
    var TDate = '';
    var SelAccIds = '';
    var SelProdIds = '';

    var txtDocDt = $('#txtDocDt').val();
    var DocDate = $('#txtDocDt').val();
    var Doc_Date = DocDate.split('-');
    DocDate = new Date();
    DocDate.setFullYear(Doc_Date[2], Doc_Date[1] - 1, Doc_Date[0]);


    var txtCreatedBy = $('#txtCreatedBy').val();

    SelAccIds = GlobalClass.AccIds.substring(0, GlobalClass.AccIds.length - 1);
    SelProdIds = GlobalClass.ProdIds.substring(0, GlobalClass.ProdIds.length - 1);


    if ($("#txtTargetAmt").val() == '' || parseInt($("#txtTargetAmt").val()) == 0) {
        IsError = true;
        $("#txtTargetAmt").focus();
        alert("Please Enter Target Amount");
    }
    else {
        txtTargetAmt = $('#txtTargetAmt').val();
        txtTargetAmt = txtTargetAmt.replace(/,/g, "");  //Remove commas
    }

    if ($("#txtSchemePct").val() == '' || parseFloat($("#txtSchemePct").val()) == 0) {
        IsError = true;
        $("#txtSchemePct").focus();
        alert("Please Enter Percentage");
    }
    else {
        txtSchemePct = $('#txtSchemePct').val();
    }

    if ($("#txtSchemeName").val() == '') {
        IsError = true;
        $("#txtSchemeName").focus();
        alert("Please Enter Scheme Name");
    }
    else {
        txtSchemeName = $('#txtSchemeName').val();
    }

    if ($("#FromDate").val() == '') {
        IsError = true;
        $("#FromDate").focus();
        alert("Please Select Effective From Date");
    }
    else {
        FDate = $('#FromDate').val();
        FromDate = $('#FromDate').val();
        var From_Date = FromDate.split('-');
        FromDate = new Date();
        FromDate.setFullYear(From_Date[2], From_Date[1] - 1, From_Date[0]);
    }

    if ($("#ToDate").val() == '') {
        IsError = true;
        $("#ToDate").focus();
        alert("Please Select Effective To Date");
    }
    else {
        TDate = $('#ToDate').val();
        ToDate = $('#ToDate').val();
        var To_Date = ToDate.split('-');
        ToDate = new Date();
        ToDate.setFullYear(To_Date[2], To_Date[1] - 1, To_Date[0]);
    }

    if (SelAccIds == '') {
        IsError = true;
        alert("Please Select Accounts");
    }

    if (SelProdIds == '') {
        IsError = true;
        alert("Please Select Products");
    }

    if ($('#SchemeFlag').is(":checked")) {
        SchemeFlag = 1;
    }

    //if (ToDate < FromDate) {
    //    IsError = true;
    //    alert("Effective to date cannot be less than effective from date.");
    //}

    //if (FromDate < DocDate) {
    //    IsError = true;
    //    alert("Effective from Date cannot be prior to Current Date");
    //}

    var dataToSend = { CompId: GlobalClass.CCode, SchemeDt: txtDocDt, AccountIds: SelAccIds, CreatedBy: txtCreatedBy, ProductIds: SelProdIds, TargetAmt: txtTargetAmt, Pct: txtSchemePct, SchemeName: txtSchemeName, Active: SchemeFlag, EffFromDt: FDate, EffToDt: TDate }
    //alert(JSON.stringify(dataToSend));
    if (IsError == false) {
        $.ajax({//Save Scheme
            url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/ConSchemePref/SaveScheme",
            cache: false,
            data: dataToSend,
            datatype: "JSON",
            type: "POST",
            contenttype: 'application/json; charset=utf-8',
            async: true,
            success: function (data) {
                alert(data.SaveConf);
                if (data.SaveConf == 'Scheme Saved Successfully') {
                    Flush();
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (Err) {
                console.log(Err);
            }
        });
    }
}
var claimsData = {};
function RunClaimsRpt() {
    debugger;
    var RptFlag = $("#RptOption option:selected").val();
    var FDt = $("#FromDate").val();
    var TDt = $("#ToDate").val();
    var opt2;
    var Flag = false;

    if (RptFlag === 0 || FDt === '' || TDt === '') {
        Flag = true;
        alert("Invalid Report Parameters");
    }

    var dataToSend = { CompId: GlobalClass.CCode, EffFromDt: FDt, EffToDt: TDt, Flag: RptFlag }
    if (Flag == false) {
        $.ajax({
            url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/ClaimsRpt/GetClaimsRpt",
            cache: false,
            type: "POST",
            datatype: 'JSON',
            contenttype: 'application/json; charset=utf-8',
            async: true,
            data: dataToSend,
            beforeSend: function () {
                //$("#ClaimsRptTbl tbody").empty();
                //$("#preloader").show();
            },
            success: function (data) {
                console.log({ data });
                claimsData = {};
                for(let claim of data.claims) {
                    claimsData[`${claim.CustomerId}`] = claim;
                }
                $("#ClaimsRptTbl tbody").empty();
                appendData(data.claims);
            },
            complete: function () {
                //$('#preloader').hide(); 
                //$('#preloader').delay(200).fadeOut('slow');
                //$('body').delay(350).css({ 'overflow': 'visible' });
            },
            error: function () {
                //alert("Error occured while fetching data!");
            }
        });
    }

}
function Loaddetails() {
    debugger;
    $("#preloader").show();
    var RptFlag = $("#Paymenttype option:selected").val();
    var FDt = $("#FromDate").val();
    var TDt = $("#ToDate").val();
    var opt2;
    var Flag = false;
    if (RptFlag === 0 || FDt === "" || TDt === "") {
        Flag = true;
        alert("Invalid Report Parameters");
        $("#Defprepaidtbl").hide();
        $("#DefRecuring").hide();
        $("#DefReconciledtbl").hide();
        $("#preloader").hide();
    }
    $("#Defprepaidtbl").hide();
    $("#DefRecuring").hide();
    $("#DefReconciledtbl").hide();

    if (RptFlag == 0 && FDt !="" && TDt !="") {
        $("#Defprepaidtbl").show();
        $("#Defprepaidtbl tbody").empty();
    }
    if (RptFlag == 1 && FDt != "" && TDt != "") {
        $("#DefRecuring").show();
        $("#DefRecuring tbody").empty();
    }
    if (RptFlag == 2 && FDt != "" && TDt != "") {
        $("#DefReconciledtbl").show();
        $("#DefReconciledtbl tbody").empty();
    }

    var dataToSend = { CompId: GlobalClass.CCode, EffFromDt: FDt, EffToDt: TDt, Flag: RptFlag }
    if (Flag == false) {
        $.ajax({
            url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/Definepayment/GetPayRpt",
            cache: false,
            type: "POST",
            datatype: "JSON",
            contenttype: "application/json; charset=utf-8",
            async: true,
            data: dataToSend,
            beforeSend: function () {
            },
            success: function (data) {
                console.log({ data });
                claimsData = {};

                dataappend(data);
            },
            complete: function () {
                //$('#preloader').hide(); 
                //$('#preloader').delay(200).fadeOut('slow');
                //$('body').delay(350).css({ 'overflow': 'visible' });
            },
            error: function () {
                //alert("Error occured while fetching data!");
            }
        });
    }
    debugger;
   
   
}
function dataappend(DefReconciled) {
    debugger;
    $("#preloader").show();
    console.log(JSON.stringify(DefReconciled));
    var Count = 1;
    var disabled = '';
    var Stat = '';
    var CType = 1;
    var AppStatus = '';
    var lgt = DefReconciled.length;
    var RptFlag = $("#Paymenttype option:selected").val();
    if (RptFlag == 0) {
        dataprepaid(DefReconciled);
    }
    if (RptFlag == 1) {
        dataRecurring(DefReconciled);
    }
    if (RptFlag == 2) {
        datareconciled(DefReconciled);
    }

}
function dataprepaid(DefReconciled) {
    debugger;
    if (DefReconciled.length == 0) {
        opt2 = "<tr><td colspan='13' style='color:red;height:300px !important;text-align: center;vertical-align: middle;'>Data Not Found!</td></tr>";
        $("#Defprepaidtbl tbody").append(opt2);
        $("#preloader").hide();
        $("#tblheaders:input").removeAttr('disabled');
        $("#FromDate").removeAttr('disabled');
        $("#ToDate").removeAttr('disabled');
        $("#Paymenttype").removeAttr('disabled');
        $("#load").removeAttr('disabled');
        $("#FromDate").css('cursor', 'pointer');
        $("#ToDate").css('cursor', 'pointer');
        $("#Paymenttype").css('cursor', 'pointer');
        $("#load").css('cursor', 'pointer');
    }
    else {

        for (var k = 0; k < DefReconciled.length; k++) {
            debugger;
            opt2 = "<tr>" +
                 "<td class='TblBody' style='text-align:center;' id='rownumber'>" + DefReconciled[k].Rownumber + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='vendrname'>" + DefReconciled[k].VendorName + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='expnse'>" + DefReconciled[k].Expense + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='Recvno'>" + DefReconciled[k].VoucherNo + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='lstupdt'>" + DefReconciled[k].Lastupdate + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='lstpmt'>" + DefReconciled[k].Lastpayment.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='rentrm'>" + DefReconciled[k].RentTerm + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='duedt'>" + DefReconciled[k].Duedate + "</td>" +
                 "<td class='TblBody' style='text-align:center;'id='dueamt' class='dueamt'>" + DefReconciled[k].Dueamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;'><input type='checkbox' id='chkbx'onchange='checkfunction()'></td>" +
                "<td class='TblBody' style='text-align:left;'><input type='number' class='amount' style='width:65px' min='0' max='100'  id='amt' size='4' readonly='true' value='" + DefReconciled[k].Topay.toFixed(2) + "' ></td>" +
                "<td class='TblBody' style='text-align:center;'id='topayamt' class='topayamt'>" + DefReconciled[k].Topayamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='Benifnme'>" + DefReconciled[k].BenificeryName + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='Benifacc'>" + DefReconciled[k].BenificeryAccount + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='bank'>" + DefReconciled[k].Bank + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none'id='dpmamt'>" + DefReconciled[k].DPMamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none' id='icurid'>" + DefReconciled[k].IcurrencyId + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none'id='iaccid'>" + DefReconciled[k].AccountId + "</td>" +
             "</tr>";
            var check = $("#chkbx").val();
            if (check == 1) {
                $("#chkbx").prop('checked', true);
            }
            else {
                $("#chkbx").prop('checked', false);
            }
            $("#Defprepaidtbl tbody").append(opt2);
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
        }
    }
}
function dataRecurring(DefReconciled) {
    debugger;
    if (DefReconciled.length == 0) {
        opt2 = "<tr><td colspan='13' style='color:red;height:300px !important;text-align: center;vertical-align: middle;'>Data Not Found!</td></tr>";
        $("#DefRecuring tbody").append(opt2);
        $("#preloader").hide();
        $("#tblheaders:input").removeAttr('disabled');
        $("#FromDate").removeAttr('disabled');
        $("#ToDate").removeAttr('disabled');
        $("#Paymenttype").removeAttr('disabled');
        $("#load").removeAttr('disabled');
        $("#FromDate").css('cursor', 'pointer');
        $("#ToDate").css('cursor', 'pointer');
        $("#Paymenttype").css('cursor', 'pointer');
        $("#load").css('cursor', 'pointer');
    }
    else {

        for (var k = 0; k < DefReconciled.length; k++) {
            debugger;
            opt2 = "<tr>" +
                "<td class='TblBody' style='text-align:center;' id='rownumber'>" + DefReconciled[k].Rownumber + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='vendrname'>" + DefReconciled[k].Account + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='expnse'>" + DefReconciled[k].Expense + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='vchno'>" + DefReconciled[k].VoucherNo + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='lstupdt'>" + DefReconciled[k].Lastupdate + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='duelamt'>" + DefReconciled[k].Lastpayment.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='bldt'>" + DefReconciled[k].BillDate + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='duedt'>" + DefReconciled[k].Duedate + "</td>" +
                 "<td class='TblBody' style='text-align:center;'id='dueamt' class='dueamt'>" + DefReconciled[k].Dueamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;'><input type='checkbox' id='chkbx' onchange='checkfunction()'></td>" +
                "<td class='TblBody' style='text-align:left;'><input type='number' class='amount' style='width:65px' min='0' max='100' id='amt' value='" + DefReconciled[k].Topay.toFixed(2) + "' ></td>" +
                "<td class='TblBody' style='text-align:center;'id='topayamt'class='topayamt'>" + DefReconciled[k].Topayamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='Benifnme'>" + DefReconciled[k].BenificeryName + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='Benifacc'>" + DefReconciled[k].BenificeryAccount + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='bank'>" + DefReconciled[k].Bank + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none'id='dpmamt'>" + DefReconciled[k].DPMamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none' id='icurid'>" + DefReconciled[k].IcurrencyId + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none'id='iaccid'>" + DefReconciled[k].AccountId + "</td>" +
             "</tr>";
            var check = $("#chkbx").val();
            if (check == 1) {
                $("#chkbx").prop('checked', true);
            }
            else {
                $("#chkbx").prop('checked', false);
            }
            $("#DefRecuring tbody").append(opt2);
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
        }
    }
}

function datareconciled(DefReconciled) {
    debugger;
    if (DefReconciled.length == 0) {
        opt2 = "<tr><td colspan='13' style='color:red;height:300px !important;text-align: center;vertical-align: middle;'>Data Not Found!</td></tr>";
        $("#DefReconciledtbl tbody").append(opt2);
        $("#preloader").hide();
        $("#tblheaders:input").removeAttr('disabled');
        $("#FromDate").removeAttr('disabled');
        $("#ToDate").removeAttr('disabled');
        $("#Paymenttype").removeAttr('disabled');
        $("#load").removeAttr('disabled');
        $("#FromDate").css('cursor', 'pointer');
        $("#ToDate").css('cursor', 'pointer');
        $("#Paymenttype").css('cursor', 'pointer');
        $("#load").css('cursor', 'pointer');
    }
    else {

        for (var k = 0; k < DefReconciled.length; k++) {
            debugger;
            //var row = `
            //    <tr>
            //     <td>
            //        <input type='file' value='${DefReconciled[k]}'/>
            //     </td>
            //    </tr>
            //    `;
       
            opt2 = "<tr>" +
                "<td class='TblBody' style='text-align:center;' id='rownumber'>" + DefReconciled[k].Rownumber + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='vendrname'>" + DefReconciled[k].VendorName + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='purchsinv'>" + DefReconciled[k].PurchseInv + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='Recvdt'>" + DefReconciled[k].Receiveddate + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='stckbal'>" + DefReconciled[k].Stockbalance + "</td>" +
                //"<td class='TblBody'><input type='text' value='' style='text-align:center;width:100px;margin-top:15px;' id='img' size='25' readonly  ondblclick='uploadimgfunction()'><input id='imgfile' type='file' style='visibility:hidden;width:10px' onchange='ChangeimgText()'>" + DefReconciled[k].image + "</td>" +
                "<td class='TblBody'><input type='text' value='' style='text-align:center;width:100px;margin-top:15px;' id='img' size='25' readonly class='upload'><input class='imgfile' id='imgfile' type='file' style='visibility:hidden;width:0px' >" + DefReconciled[k].image + "</td>" +
               "<td class='TblBody' style='text-align:center;' id='duedt'>" + DefReconciled[k].Duedate + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='dueamt' class='dueamt'>" + DefReconciled[k].Dueamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;'><input type='checkbox' class='chkbx' onchange='checkfunction()'></td>" +
                 "<td class='TblBody' style='text-align:left;'><input type='number' class='amount' style='width:65px' min='0' max='100'  id='amt' size='4' value='" + DefReconciled[k].Topay.toFixed(2) + "' ></td>" +
                "<td class='TblBody' style='text-align:center;'id='topayamt' class='topayamt'>" + DefReconciled[k].Topayamount.toFixed(2) + "</td>" +
                 "<td class='TblBody' style='text-align:center;'><select class='ddlpaymode' id='ddlpaymode'><option>" + DefReconciled[k].Accpaymode + "<option></td>" +
                "<td class='TblBody' style='text-align:center;' id='cur'>" + DefReconciled[k].Currency + "</td>" +
                "<td class='TblBody'><input type='text' value='' style='text-align:center;width:100px;margin-top: 15px;' id='vendrimg' size='25' readonly class='vendupload'><input id='vendfile' class='vendfile' type='file' style='visibility:hidden;width:0px'>" + DefReconciled[k].Vendorattachment + "</td>" +
                "<td class='TblBody' style='text-align:center;' id='lstupdt'>" + DefReconciled[k].Lastupdate + "</td>" +              
                "<td class='TblBody' style='text-align:center;'id='Benifnme'>" + DefReconciled[k].BenificeryName + "</td>" +
                "<td class='TblBody' style='text-align:center;'id='Benifacc'>" + DefReconciled[k].BenificeryAccount + "</td>" +
                "<td class='TblBody' style='text-align:center;width:auto'id='bank'>" + DefReconciled[k].Bank + "</td>" +
                //"<td class='TblBody' style='text-align:right;'id='idt'>" + DefReconciled[k].iDate + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none'id='dpmamt'>" + DefReconciled[k].DPMamount.toFixed(2) + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none' id='icurid'>" + DefReconciled[k].IcurrencyId + "</td>" +
                "<td class='TblBody' style='text-align:center;display:none'id='iaccid'>" + DefReconciled[k].AccountId + "</td>" +
                "<td  style='text-align:center;display:none'id='s64img' class='s64img'>" + DefReconciled[k].s64image + "</td>" +
                "<td  style='text-align:center;display:none'id='s64vendimg' class='s64vendimg'>" + DefReconciled[k].s64vendorattch + "</td>" +
                //"<td class='TblBody' style='text-align:center;display:none'><select class='ddlallpaymode' id='ddlallpaymode'><option><option></td>"
            "</tr>";
            $("#DefReconciledtbl tbody").append(opt2);
            allpamentmodes();
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
        }
    }
}

function allpamentmodes() {
    debugger;
    let dropdown = $('.ddlpaymode');
   var ddlvalue= $('.ddlpaymode :selected').val();
    $.ajax({
        url: "Accpaymentmodes",
        cache: false,
        type: "Post",
        data: { CompId: GlobalClass.CCode,ddlitem:ddlvalue },
        datatype: 'JSON',
        success: function (response) {
            debugger;
            dropdown.empty();
            $('.ddlpaymode').append('<option selected="selected" value="'+ddlvalue+'">'+ddlvalue+'</option>');
            $.each(response, function (i, data) {

                $(".ddlpaymode").append('<option value="' + data.AllAccpaymentmodes + '">' +
                     data.AllAccpaymentmodes + '</option>');
            });
        }
    });
}
function cleardata() {
    debugger;
    $("#Defprepaidtbl").hide();
    $("#DefRecuring").hide();
    $("#DefReconciledtbl").hide();
    $('#myModal').modal('hide');
    $('#postmodel').modal('hide');
    $("#preloader").hide();
    $("#tblheaders:input").removeAttr('disabled');
    $("#FromDate").removeAttr('disabled');
    $("#ToDate").removeAttr('disabled');
    $("#Paymenttype").removeAttr('disabled');
    $("#load").removeAttr('disabled');
    $("#FromDate").css('cursor', 'pointer');
    $("#ToDate").css('cursor', 'pointer');
    $("#Paymenttype").css('cursor', 'pointer');
    $("#load").css('cursor', 'pointer');
}


$('#DefReconciledtbl tbody').on('dblclick', '.upload', function () {
    var $tr = $(this).parents('tr');
    var $upload = $tr.find('.imgfile').click();
});

$('#DefReconciledtbl tbody').on('change', '.imgfile', function () {
    var files = $(this)[0].files;
    var fileName = files[0].name;
    console.log(files)
    var $tr = $(this).parents('tr');
    var $upload = $tr.find('.upload').val(fileName)
    uploadimgData();
});
$('#DefReconciledtbl tbody').on('dblclick', '.vendupload', function () {
    var $tr = $(this).parents('tr');
    var $upload = $tr.find('.vendfile').click();
});

$('#DefReconciledtbl tbody').on('change', '.vendfile', function () {
    var files = $(this)[0].files;
    var fileName = files[0].name;
    console.log(files)
    var $tr = $(this).parents('tr');
    var $upload = $tr.find('.vendupload').val(fileName)
    uploadvendData();
});
function uploadimgData() {
    var files = $('.imgfile')[0].files;
    var row = $(this).parents('tr');
    var formData = new FormData();
    formData.append(files[0].name, files[0]);
    console.log(formData);
    row.find('.s64img').html(0);
    var s64bytes = row.find('.s64img').html();
    row.find('.s64img').text('');
    //var request = new XMLHttpRequest();
    //request.open("POST", configuration.baseUrl + "/Definepayment/getpath");
    //request.send(formData);

    $.ajax({
        url: configuration.baseUrl + "/PrjAndaa/Definepayment/getpath",
        type: "POST",
        contentType: false, 
        processData: false, 
        data: formData,
        success: function (result) {
            debugger;
            $(".s64img").text(result.bytes);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
function uploadvendData() {
    var files = $('.vendfile')[0].files;
    var row = $(this).parents('tr');
    var formData = new FormData();
    formData.append(files[0].name, files[0]);
    console.log(formData);
    row.find('.s64vendimg').html(0);
    var s64bytes = row.find('.s64vendimg').html();
    row.find('.s64vendimg').text('');
    //var request = new XMLHttpRequest();
    //request.open("POST", configuration.baseUrl + "/Definepayment/getpath");
    //request.send(formData);

    $.ajax({
        url: configuration.baseUrl + "/PrjAndaa/Definepayment/getpath",
        type: "POST",
        contentType: false, 
        processData: false, 
        data: formData,
        success: function (result) {
            debugger;
            $(".s64vendimg").text(result.bytes);
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}

function uploadvendfunction() {
    debugger;
    var $trs = $('#DefReconciledtbl tbody tr')
    var rowcount = $trs.length
    $("#DefReconciledtbl tbody input[type=checkbox]:checked").each(function () {
        debugger;
        var row = $(this).closest("tr")[0];
        $(row.cells[10]).find(".vendfile").click();
    });
}

function ChangevendText() {
    debugger;
    var $trs = $('#DefReconciledtbl tbody tr')
    var rowcount = $trs.length
    $("#DefReconciledtbl tbody input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        var file = $(row.cells[10]).find('#vendfile').val();
        var filename = file;
        $(row.cells[10]).find("#vendrimg").attr('value', '');
        $(row.cells[10]).find("#vendrimg").attr('value', filename);
        $(row.cells[5]).find("#img").text().replace('C:\fakepath', '');
        debugger;
    });
}
function fileupload() {
    debugger;
    let nBytes = 0,
       oFiles = document.getElementById("img").files,
       nFiles = oFiles.length;
    for (let nFileId = 0; nFileId < nFiles; nFileId++) {
        nBytes += oFiles[nFileId].size;
        var filename = oFiles[nFileId].name;
    }
    let sOutput = nBytes
}
function appendData(SalesData) {
    console.log(JSON.stringify(SalesData));
    var Count = 1;
    var disabled = '';
    var Stat = '';
    var CType = -1;
    var AppStatus = '';
    if (SalesData.length == 0) {
        opt2 = "<tr><td colspan='13' style='color:red;height:300px !important;text-align: center;vertical-align: middle;'>Data Not Found!</td></tr>";
        $("#ClaimsRptTbl tbody").append(opt2);
    } else {
        //$("#ClaimsRptTbl").append("<tbody>");
        for (var k = 0; k < SalesData.length; k++) {

            disabled = parseInt(SalesData[k].ClaimStatus) === 1 ? "disabled" : "";
            Stat = parseInt(SalesData[k].ClaimStatus) === 1 ? "Yes" : "No";
            if (parseInt(SalesData[k].ClaimStatus) == 1) {
                CType = SalesData[k].ClaimType;
            }
            else {
                CType = -1;
            }

            if (parseInt(SalesData[k].NetSales) >= parseInt(SalesData[k].TargetAmt)) {
                AppStatus = 'Yes';
            }
            else {
                AppStatus = 'No';
            }

            opt2 = "<tr>" +
            "<td class='TblBody'>" + Count + "</td>" +
            "<td class='TblBody' style='text-align:center;'>" + SalesData[k].SchemeName + "</td>" +
            "<td class='TblBody' style='text-align:center;width:200px;'>" + SalesData[k].CustName + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + SalesData[k].TargetAmt + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + SalesData[k].TaxableSL + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + SalesData[k].TaxableSR + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + SalesData[k].NetSales + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + AppStatus + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + SalesData[k].ClaimPct + "</td>" +
            "<td class='TblBody' style='text-align:right;'>" + parseFloat(SalesData[k].ClaimAmt).toFixed(2) + "</td>" +
            "<td class='TblBody' style='text-align:right;width:150px;'><select style='width:150px;border:none;' onchange='GetValue(this)'  data-claimtype=" + CType + " data-body-id=" + SalesData[k].BodyId + " data-customer-id='" + SalesData[k].CustomerId + "' class='claims' id=" + Count + "></select></td>" +
            "<td class='TblBody Stat' style='text-align:center;' >" + Stat + "</td>" +
            "<td class='TblBody' style='text-align:center;'><input type='checkbox' class='Chk' data-customer-id='" + SalesData[k].CustomerId + "' data-body-id='" + SalesData[k].BodyId + "' " + disabled + " /></td>" +
            "<td class='TblBody' style='text-align:center;display:none;'>" + SalesData[k].BodyId + "</td>" +
            "</tr>";
            $("#ClaimsRptTbl tbody").append(opt2);
            Count++;
        }
        //$("#ClaimsRptTbl").append("</tbody>");
        BindClaimTypes();
    }
}

$('#ClaimsRptTbl tbody').on('change', ".Chk", function () {
    debugger;
    var id = $(this).data('customer-id');
    var select = $(this).parents("tr").find(".claims").val()
    var status = $(this).parents("tr").find(".Stat");
    //console.log("Status: "+{status});
    if ($(this).is(":checked")) {
        //$(status).html("Yes")
    } else {
        // $(status).html("No")
    }

    if (select > 0) {
        //claimsData[`${id}`].ClaimStatus = 'Yes'; 
        claimsData[`${id}`].Flag = 1;
        console.log({ claimsData });
    }
    else {
        $(this).attr("checked", false);
        //claimsData[`${id}`].ClaimStatus = 'No'; 
        claimsData[`${id}`].Flag = 0;
        alert("Please select claim type");
    }
})


function GetValue(e) {
    var id = $(e).data('customer-id');

    claimsData[`${id}`].ClaimType = $(e).val();

    if (parseInt($(e).val()) === -1) {
        $(e).parents("tr").find(".Chk").attr("checked", false)
        //claimsData[`${id}`].ClaimStatus = 'No';
        //$(e).parents("tr").find(".Stat").html("No")
    }
}
function checkfunction() {
    debugger;
    var $trs = $('#Defprepaidtbl tbody tr')
    var rowcount = $trs.length
    $("#Defprepaidtbl tbody input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        $(row.cells[8]).prop('readonly', false);
        $("#ddlpaymode").prop('disabled', false);
    });
    var $trs = $('#DefRecuring tbody tr')
    var rowcount = $trs.length
    $("#DefRecuring tbody input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        $(row.cells[8]).prop('readonly', false);
        $("#ddlpaymode").prop('disabled', false);
    });
    var $trs = $('#DefReconciledtbl tbody tr')
    var rowcount = $trs.length
    $("#DefReconciledtbl tbody input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        $(row.cells[12]).prop('readonly', false);
        $("#ddlpaymode").prop('disabled', false);
    });
}

$('#DefReconciledtbl tbody').on('change', '.amount', function () {
    debugger;
    var row = $(this).parents("tr");
    var tpay =row.find('#amt').val();
    var dueamt = row.find('#dueamt').html();
    var topayamt =row.find('#topayamt').html();
    debugger;
    if (dueamt > 0 && tpay > 0) {
        debugger;
        $(row).find('#topayamt').html(0);
        $(row).find('#topayamt').html(dueamt * tpay / 100);

    }
});
$('#DefRecuring tbody').on('change', '.amount', function () {
    debugger;
    var row = $(this).parents("tr");
    var tpay = row.find('#amt').val();
    var dueamt = row.find('#dueamt').html();
    var topayamt = row.find('#topayamt').html();
    debugger;
    if (dueamt > 0 && tpay > 0) {
        debugger;
        $(row).find('#topayamt').html(0);
        $(row).find('#topayamt').html(dueamt * tpay / 100);

    }
});
    function BindClaimTypes() {
        debugger;
        var data = JSON.parse(GlobalClass.ClaimTypes);
        $('#ClaimsRptTbl .claims').empty();
        $('#ClaimsRptTbl .claims').append($("<option></option>")
         .attr("value", -1)
         .attr("selected", true)
         .text(''))

        for(let claim of data) {
            $('#ClaimsRptTbl .claims').append($("<option></option>")
           .attr("value", claim.id)
           .text(claim.name))
        }

        var ClaimT = $('.claims');
        ClaimT.each(function (i, e) {
            $(e).val($(e).data("claimtype"));
        })
    }

    function postcheck() {
        debugger;
        if ($("#Defprepaidtbl").is(":visible"))
        {
            var $trs = $('#Defprepaidtbl tbody tr')
            var rowcount = $trs.length
            var checkeddetailsarray = [];
            $("#Defprepaidtbl tbody input[type=checkbox]:checked").each(function () {
                debugger;
                var row = $(this).parents("tr");
                var checkeddetails = {
                    VendorName: $(row).find('#vendrname').html(),
                    Expense: $(row).find('#expnse').html(),
                    VoucherNo: $(row).find('#vchno').html(),
                    Lastupdate: $(row).find('#lstupdt').html(),
                    Lastpayment: $(row).find('#lstpmt').html(),
                    RentTerm: $(row).find('#rentrm').html(),
                    Duedate: $(row).find('#duedt').html(),
                    Dueamount: $(row).find('#dueamt').html(),
                    Topay: $(row).find('#amt').val(),
                    Topayamount: $(row).find('#topayamt').html(),
                    BenificeryName: $(row).find('#Benifnme').html(),
                    BenificeryAccount: $(row).find('#Benifacc').html(),
                    Bank: $(row).find('#bank').html(),
                    DPMamount: $(row).find('#dpmamt').html(),
                    IcurrencyId: $(row).find('#icurid').html(),
                    AccountId: $(row).find('#iaccid').html()
                }
                checkeddetailsarray.push(checkeddetails);
            });
            if (!checkeddetailsarray) {

                alert('Please select at least one record')
                $("#preloader").hide();
                $("#tblheaders:input").attr('disabled', 'disabled');
                $("#FromDate").attr('disabled', 'disabled');
                $("#ToDate").attr('disabled', 'disabled');
                $("#Paymenttype").attr('disabled', 'disabled');
                $("#load").attr('disabled', 'disabled');
                $("#FromDate").css('cursor', 'not-allowed');
                $("#ToDate").css('cursor', 'not-allowed');
                $("#Paymenttype").css('cursor', 'not-allowed');
                $("#load").css('cursor', 'not-allowed');
                return
            }
            if (checkeddetailsarray.length === 0) {
                alert('Please select at least one record')
                $("#preloader").hide();
                $("#tblheaders:input").attr('disabled', 'disabled');
                $("#FromDate").attr('disabled', 'disabled');
                $("#ToDate").attr('disabled', 'disabled');
                $("#Paymenttype").attr('disabled', 'disabled');
                $("#load").attr('disabled', 'disabled');
                $("#FromDate").css('cursor', 'not-allowed');
                $("#ToDate").css('cursor', 'not-allowed');
                $("#Paymenttype").css('cursor', 'not-allowed');
                $("#load").css('cursor', 'not-allowed');
                return
            }
            if (checkeddetailsarray.length != "")
            {

                $('#postmodel').modal('show');
            }
        }
        if ($("#DefRecuring").is(":visible"))
        {
            var $trs = $('#DefRecuring tbody tr')
            var rowcount = $trs.length
            var checkeddetailsarray = [];
            var compId = 504;
            $("#DefRecuring tbody input[type=checkbox]:checked").each(function () {
                debugger;
                var row = $(this).parents("tr");
                var checkeddetails = {
                    Account: $(row).find('#vendrname').html(),
                    Expense: $(row).find('#expnse').html(),
                    VoucherNo: $(row).find('#vchno').html(),
                    Lastupdate: $(row).find('#lstupdt').html(),
                    Lastpayment: $(row).find('#duelamt').html(),
                    Billdate: $(row).find('#bldt').html(),
                    Duedate: $(row).find('#duedt').html(),
                    Dueamount: $(row).find('#dueamt').html(),
                    Topay: $(row).find('#amt').val(),
                    Topayamount: $(row).find('#topayamt').html(),
                    BenificeryName: $(row).find('#Benifnme').html(),
                    BenificeryAccount: $(row).find('#Benifacc').html(),
                    Bank: $(row).find('#bank').html(),
                    DPMamount: $(row).find('#dpmamt').html(),
                    IcurrencyId: $(row).find('#icurid').html(),
                    AccountId: $(row).find('#iaccid').html(),
                }
                checkeddetailsarray.push(checkeddetails);
            });
            if (!checkeddetailsarray) {

                alert('Please select at least one record')
                $("#preloader").hide();
                $("#tblheaders:input").attr('disabled', 'disabled');
                $("#FromDate").attr('disabled', 'disabled');
                $("#ToDate").attr('disabled', 'disabled');
                $("#Paymenttype").attr('disabled', 'disabled');
                $("#load").attr('disabled', 'disabled');
                $("#FromDate").css('cursor', 'not-allowed');
                $("#ToDate").css('cursor', 'not-allowed');
                $("#Paymenttype").css('cursor', 'not-allowed');
                $("#load").css('cursor', 'not-allowed');
                return

            }
            if (checkeddetailsarray.length === 0) {
                alert('Please select at least one record')
                $("#preloader").hide();
                $("#tblheaders:input").attr('disabled', 'disabled');
                $("#FromDate").attr('disabled', 'disabled');
                $("#ToDate").attr('disabled', 'disabled');
                $("#Paymenttype").attr('disabled', 'disabled');
                $("#load").attr('disabled', 'disabled');
                $("#FromDate").css('cursor', 'not-allowed');
                $("#ToDate").css('cursor', 'not-allowed');
                $("#Paymenttype").css('cursor', 'not-allowed');
                $("#load").css('cursor', 'not-allowed');
                return

            }
            if (checkeddetailsarray.length != "") {

                $('#postmodel').modal('show');
            }
        }
        if ($("#DefReconciledtbl").is(":visible"))
        {
            var $trs = $('#DefReconciledtbl tbody tr')
            var rowcount = $trs.length
            var checkeddetailsarray = [];
            var accpaymode = "";
            var formData = new FormData();
            $("#DefReconciledtbl tbody input[type=checkbox]:checked").each(function (i) {
                debugger;
                var row = $(this).parents("tr");
                var files = $(row).find('.imgfile')[0].files;
                var checkeddetails = {
                    Index: i,
                    VendorName: $(row).find('#vendrname').html(),
                    PurchseInv: $(row).find('#purchsinv').html(),
                    Receiveddate: $(row).find('#Recvdt').html(),
                    Stockbalance: $(row).find('#stckbal').html(),
                    image: $(row).find('#img').val(),
                    Duedate: $(row).find('#duedt').html(),
                    Dueamount: $(row).find('#dueamt').html(),
                    Currency: $(row).find('#cur').html(),
                    Vendorattachment: $(row).find('#vendrimg').val(),
                    Lastupdate: $(row).find('#lstupdt').html(),
                    Topay: $(row).find('#amt').val(),
                    Topayamount: $(row).find('#topayamt').html(),
                    BenificeryName: $(row).find('#Benifnme').html(),
                    BenificeryAccount: $(row).find('#Benifacc').html(),
                    Bank: $(row).find('#bank').html(),
                    DPMamount: $(row).find('#dpmamt').html(),
                    IcurrencyId: $(row).find('#icurid').html(),
                    AccountId: $(row).find('#iaccid').html(),
                    s64image: $(row).find("#s64img").html(),
                    s64vendorattch: $(row).find("#s64vendimg").html(),
                    Accpaymode: $(row).find('#ddlpaymode option:selected').val()//accpaymode
                    //$(row.cells[22]).find('#ddlpaymode option:selected').val()
                }
                checkeddetailsarray.push(checkeddetails);
            });
            if (!checkeddetailsarray) {
                alert('Please select at least one record')
                $("#preloader").hide();
                $("#tblheaders:input").attr('disabled', 'disabled');
                $("#FromDate").attr('disabled', 'disabled');
                $("#ToDate").attr('disabled', 'disabled');
                $("#Paymenttype").attr('disabled', 'disabled');
                $("#load").attr('disabled', 'disabled');
                $("#FromDate").css('cursor', 'not-allowed');
                $("#ToDate").css('cursor', 'not-allowed');
                $("#Paymenttype").css('cursor', 'not-allowed');
                $("#load").css('cursor', 'not-allowed');
                return

            }
            if (checkeddetailsarray.length === 0) {
                alert('Please select at least one record')
                $("#preloader").hide();
                $("#tblheaders:input").attr('disabled', 'disabled');
                $("#FromDate").attr('disabled', 'disabled');
                $("#ToDate").attr('disabled', 'disabled');
                $("#Paymenttype").attr('disabled', 'disabled');
                $("#load").attr('disabled', 'disabled');
                $("#FromDate").css('cursor', 'not-allowed');
                $("#ToDate").css('cursor', 'not-allowed');
                $("#Paymenttype").css('cursor', 'not-allowed');
                $("#load").css('cursor', 'not-allowed');
                return
            }
            if (checkeddetailsarray.length != "") {
                $('#postmodel').modal('show');
            }
        }
    }
    function PostDefinepayment() {
        debugger;
        $("#preloader").show();
        if ($("#Defprepaidtbl").is(":visible")) {
            postPrepaid();
            //cleardata();
        }
        if ($("#DefRecuring").is(":visible")) {
            postRecurring();
            //cleardata();
        }
        if ($("#DefReconciledtbl").is(":visible")) {
            postReconciled();
            //cleardata();
        }
    }

    function postPrepaid() {
        var $trs = $('#Defprepaidtbl tbody tr')
        var rowcount = $trs.length
        var checkeddetailsarray = [];
        $("#Defprepaidtbl tbody input[type=checkbox]:checked").each(function () {
            debugger;
            var row = $(this).parents("tr");
            var checkeddetails = {
                VendorName: $(row).find('#vendrname').html(),
                Expense: $(row).find('#expnse').html(),
                VoucherNo: $(row).find('#vchno').html(),
                Lastupdate: $(row).find('#lstupdt').html(),
                Lastpayment: $(row).find('#lstpmt').html(),
                RentTerm: $(row).find('#rentrm').html(),
                Duedate: $(row).find('#duedt').html(),
                Dueamount: $(row).find('#dueamt').html(),
                Topay: $(row).find('#amt').val(),
                Topayamount: $(row).find('#topayamt').html(),
                BenificeryName: $(row).find('#Benifnme').html(),
                BenificeryAccount: $(row).find('#Benifacc').html(),
                Bank: $(row).find('#bank').html(),
                DPMamount: $(row).find('#dpmamt').html(),
                IcurrencyId: $(row).find('#icurid').html(),
                AccountId: $(row).find('#iaccid').html()
            }
            checkeddetailsarray.push(checkeddetails);
        });
        if (!checkeddetailsarray) {

            alert('Please select at least one record')
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
            return
        }
        if (checkeddetailsarray.length === 0) {
            alert('Please select at least one record')
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
            return        
        }
        if (checkeddetailsarray.length != "")
        {
            //$('#postmodel').modal('show');
            debugger;
            var dataToSend = { CompId: GlobalClass.CCode, SessionId: GlobalClass.SId, checkeddetailsarray: checkeddetailsarray }
            $.ajax({
                //url: '@Url.Action("methodname", "cintrollername")',
                url: configuration.baseUrl + "/PrjAndaa/Definepayment/Saveprepaid",
                cache: false,
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                async: true,
                data: JSON.stringify(dataToSend),
                success: function (response) {
                    debugger;
                    cleardata();
                    if (response.status) {
                        console.log(response)
                        alert(response)
                        location.reload(true);
                    } else {
                        alert(response)
                        cleardata();
                    }
                }, error: function (e) {
                    console.log(e)
                    cleardata();
                }

            });
            $('#postmodel').modal('hide');
            $("#preloader").show();
            
        }
       
    }
    function postRecurring() {
        var $trs = $('#DefRecuring tbody tr')
        var rowcount = $trs.length
        var checkeddetailsarray = [];
        var compId = 504;
        $("#DefRecuring tbody input[type=checkbox]:checked").each(function () {
            debugger;
            var row = $(this).parents("tr");
            var checkeddetails = {
                Account: $(row).find('#vendrname').html(),
                Expense: $(row).find('#expnse').html(),
                VoucherNo: $(row).find('#vchno').html(),
                Lastupdate: $(row).find('#lstupdt').html(),
                Lastpayment: $(row).find('#duelamt').html(),
                Billdate: $(row).find('#bldt').html(),
                Duedate: $(row).find('#duedt').html(),
                Dueamount: $(row).find('#dueamt').html(),
                Topay: $(row).find('#amt').val(),
                Topayamount: $(row).find('#topayamt').html(),
                BenificeryName: $(row).find('#Benifnme').html(),
                BenificeryAccount: $(row).find('#Benifacc').html(),
                Bank: $(row).find('#bank').html(),
                DPMamount: $(row).find('#dpmamt').html(),
                IcurrencyId: $(row).find('#icurid').html(),
                AccountId: $(row).find('#iaccid').html(),
            }
            checkeddetailsarray.push(checkeddetails);
        });
        if (!checkeddetailsarray) {

            alert('Please select at least one record')
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
            return
           
        }
        if (checkeddetailsarray.length === 0) {
            alert('Please select at least one record')
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
            return
         
        }
        if (checkeddetailsarray.length != "")
        {
            //$('#postmodel').modal('show');
            debugger;
            var dataToSend = { CompId: GlobalClass.CCode, SessionId: GlobalClass.SId, checkeddetailsarray: checkeddetailsarray }
            $.ajax({
                //url: '@Url.Action("methodname", "cintrollername")',
                url: configuration.baseUrl + "/PrjAndaa/Definepayment/saveRecurring",
                cache: false,
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                async: true,
                data: JSON.stringify(dataToSend),
                success: function (response) {
                    debugger;
                    cleardata();
                    if (response.status) {
                        console.log(response)
                        alert(response)
                        location.reload(true);
                    } else {
                        alert(response)
                        cleardata();
                    }
                }, error: function (e) {
                    console.log(e)
                    cleardata();
                }

            });
            $('#postmodel').modal('hide');
            $("#preloader").show();
        }
      
    }
    function postReconciled() {
        var $trs = $('#DefReconciledtbl tbody tr')
        var rowcount = $trs.length
        var checkeddetailsarray = [];
        var accpaymode = "";
        var formData = new FormData();
        $("#DefReconciledtbl tbody input[type=checkbox]:checked").each(function (i) {
            debugger;
            var row = $(this).parents("tr");
            var files = $(row).find('.imgfile')[0].files;
            var checkeddetails = {
                Index: i,
                VendorName: $(row).find('#vendrname').html(),
                PurchseInv: $(row).find('#purchsinv').html(),
                Receiveddate: $(row).find('#Recvdt').html(),
                Stockbalance: $(row).find('#stckbal').html(),
                image: $(row).find('#img').val(),
                Duedate: $(row).find('#duedt').html(),
                Dueamount: $(row).find('#dueamt').html(),
                Currency: $(row).find('#cur').html(),
                Vendorattachment: $(row).find('#vendrimg').val(),
                Lastupdate: $(row).find('#lstupdt').html(),
                Topay: $(row).find('#amt').val(),
                Topayamount: $(row).find('#topayamt').html(),
                BenificeryName: $(row).find('#Benifnme').html(),
                BenificeryAccount: $(row).find('#Benifacc').html(),
                Bank: $(row).find('#bank').html(),
                DPMamount: $(row).find('#dpmamt').html(),
                IcurrencyId: $(row).find('#icurid').html(),
                AccountId: $(row).find('#iaccid').html(),
                s64image: $(row).find("#s64img").html(),
                s64vendorattch: $(row).find("#s64vendimg").html(),
                Accpaymode: $(row).find('#ddlpaymode option:selected').val()//accpaymode
                //$(row.cells[22]).find('#ddlpaymode option:selected').val()
            }
            checkeddetailsarray.push(checkeddetails);
        });
        if (!checkeddetailsarray) {

            alert('Please select at least one record')
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
            return
        
        }
        if (checkeddetailsarray.length === 0) {
            alert('Please select at least one record')
            $("#preloader").hide();
            $("#tblheaders:input").attr('disabled', 'disabled');
            $("#FromDate").attr('disabled', 'disabled');
            $("#ToDate").attr('disabled', 'disabled');
            $("#Paymenttype").attr('disabled', 'disabled');
            $("#load").attr('disabled', 'disabled');
            $("#FromDate").css('cursor', 'not-allowed');
            $("#ToDate").css('cursor', 'not-allowed');
            $("#Paymenttype").css('cursor', 'not-allowed');
            $("#load").css('cursor', 'not-allowed');
            return
         
        }
        if (checkeddetailsarray.length != "")
        {
            //$('#postmodel').modal('show');
            debugger;
            //formData.append(`CompId`, GlobalClass.CCode);
            //formData.append(`SessionId`, GlobalClass.SId);
            //formData.append(`checkeddetailsarray`,checkeddetailsarray);
            var dataToSend = { CompId: GlobalClass.CCode, SessionId: GlobalClass.SId, checkeddetailsarray: checkeddetailsarray }
            $.ajax({
                //url: '@Url.Action("methodname", "cintrollername")',
                url: configuration.baseUrl + "/PrjAndaa/Definepayment/SaveReconciled",
                cache: false,
                type: "POST",
                // processData: false,
                // contentType: false,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(dataToSend),
                success: function (response) {
                    debugger;
                    cleardata();
                    if (response.status) {
                        console.log(response)
                        alert(response)
                        location.reload(true);
                    } else {
                        alert(response)
                        cleardata();
                    }
                }, error: function (e) {
                    console.log(e)
                    cleardata();
                }
            });
            $('#postmodel').modal('hide');
            $("#preloader").show();
        }      
    }
    function GetAllClaims() {
        debugger;
        var filtered = Object.values(claimsData).filter(c => c.Flag === 1);
        console.log(filtered.length);
        if (filtered.length > 0) {
            var dataToSend = { CompId: GlobalClass.CCode, SessionId: GlobalClass.SId, model: filtered }
            console.log({ dataToSend });
            $.ajax({//Save Scheme
                url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/ClaimsRpt/PostVoucher",
                cache: false,
                data: JSON.stringify(dataToSend),
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                async: true,
                success: function (data) {
                    if (getBoolean(data.Msg) == true) {
                        //var len=Object.keys(claimsData).length;
                        //for (var i=0; i<=len; i++) {
                        //    claimsData['Flag' + i] = 0;
                        //}
                        location.reload(true);
                        alert("Voucher(s) Posted Successfully");
                        $("#ClaimsRptTbl tbody").empty();
                        appendData(Object.values(claimsData));
                    }
                    else {
                        console.log("Data not available/Vouchers are not posted due to internal error " + data.Msg);
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                }
            });
        }
    }


    //$('#selectAll').click(function (e) {
    //    $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
    //});

    function GetDocNo() {
        var dataToSend = { CompId: GlobalClass.CCode }
        $.ajax({    //Get Docuemnt No
            url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/ConSchemePref/GetDocNo",
            cache: false,
            data: dataToSend,
            datatype: "JSON",
            type: "POST",
            contenttype: 'application/json; charset=utf-8',
            async: true,
            success: function (data) {
                $("#txtDocNo").val(data.GetDocSeq[0].DocNo);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (Err) {
                console.log(Err);
            }
        });
    }

    function GetAllMasters(SchName) {
        $("#AccTree").empty();
        $("#ProdTree").empty();
        var dataToSend = { CompId: GlobalClass.CCode, SchemeName: SchName }
        $.ajax({                                                            //Get Masters(Accounts & Products)
            url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/ConSchemePref/GetMasters",
            cache: false,
            data: dataToSend,
            datatype: "JSON",
            type: "POST",
            async: true,
            success: function (item1) {

                var TAcc = item1.Accounts.map(c=> c.IsChecked ? { c, 'checked': "checked" } : { c });
                GlobalClass.AccIds = TAcc.filter(a=>a.checked).map(abc=>abc.id).join(',');

                var TProd = item1.Products.map(c=> c.IsChecked ? { c, 'checked': "checked" } : { c });
                GlobalClass.ProdIds = TProd.filter(a=>a.checked).map(abc=>abc.id).join(',');

                var tree = simTree({
                    el: '#AccTree',
                    data: TAcc,
                    check: true,
                    linkParent: true,
                    onChange: function (item) {
                        GlobalClass.AccIds = '';
                        for (var i = 0; i < item.length; i++) {
                            GlobalClass.AccIds = GlobalClass.AccIds + item[i]['id'] + ",";
                        }
                    }
                });


                var tree = simTree({
                    el: '#ProdTree',
                    data: TProd,
                    check: true,
                    linkParent: true,
                    onChange: function (item) {
                        //alert("Products: "+JSON.stringify(item));
                        GlobalClass.ProdIds = '';
                        for (var i = 0; i < item.length; i++) {
                            GlobalClass.ProdIds = GlobalClass.ProdIds + item[i]['id'] + ",";
                        }
                    }
                });

                updateSchemaDetails(SchName, item1.SchemeDetails);

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (Err) {
                console.log(Err);
            }
        });
    }


    function GetSchemeExistFlag() {
        var ScName = $('#txtSchemeName').val();
        var dataToSend = { CompId: GlobalClass.CCode, SchemeName: ScName }
        $.ajax({    //Get Docuemnt No
            url: "http://" + ServerIPAddress.ServerIP + "/PrjAndaa/ConSchemePref/GetSchemeFlag",
            cache: false,
            data: dataToSend,
            datatype: "JSON",
            type: "POST",
            contenttype: 'application/json; charset=utf-8',
            async: true,
            success: function (data) {
                if (data.GetFlag[0].Flag == "1") {
                    alert("Scheme already exists by this name");
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (Err) {
                console.log(Err);
            }
        });
    }



    function updateSchemaDetails(SName, schema) {
        if (SName == "") {
            GetDocNo();

            var now = new Date();
            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = (day) + "-" + (month) + "-" + now.getFullYear()
            $('#txtDocDt').val(today);

            $("#txtCreatedBy").val(GlobalClass.UserName);
        }
        else {
            $("#txtDocNo").val(schema.DocNo);
            $("#txtDocDt").val(schema.DocDate);
            $("#txtCreatedBy").val(schema.CreatedBy);
            $("#txtTargetAmt").val(schema.TargetAmt);
            $("#txtSchemePct").val(schema.SchemePct);
            $("#txtSchemeName").val(schema.SchemeName);
            $('#SchemeFlag').prop('checked', getBoolean(schema.SchemeActive));
            $("#FromDate").val(schema.EffFromDt);
            $("#ToDate").val(schema.EffToDt);
        }
    }

    function getBoolean(value) {
        debugger;
        switch (value) {
            case true:
            case "true":
            case "True":
            case "TRUE":
            case 1:
            case "1":
            case "on":
            case "yes":
                return true;
            default:
                return false;
        }
    }

    function SearchScheme() {
        debugger;
        var SchName = $('#SchemeNames').find(":selected").text();
        if (SchName != null || SchName != "") {
            GetAllMasters(SchName);
        }
    }