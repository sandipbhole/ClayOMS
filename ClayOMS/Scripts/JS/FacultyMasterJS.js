var applicationUrl;
$(document).ready(function (event) {
    var value = $('#facultyID').val();
    if (value == 0)
        $('#btnSaveClient').val('SAVE')
    else
        $('#btnSaveClient').val('UPDATE')

   applicationUrl = $("#applicationPath").val();
    var txt = $('#txtSearch').data('placeholder');

    if (event.handled != true) {
        LoadGrid();
        event.handled = true;
    }


    $("#txtSearch").keyup(function () {

       ReloadGrid();
    });
   
    $("#txtDean").keyup(function () {

        ReloadGrid();
    });
    $("#ddtActivated").keyup(function () {

        //ReloadGrid();
    });
    $("#btnSearch").click(function () {

        ReloadGrid();
    });

    //    $("#btnSaveClient").click(function () {

    //if ($("#frmClient").valid()) {
    //            $("#loading").show();
    //            ////////////////
    //            var TxtAgentName = $.trim($('#TxtAgentName').val());
    //            var TxtMobile1 = $.trim($('#TxtMobile1').val());
    //            var TxtContactPersonName1 = $.trim($('#TxtContactPersonName1').val());

    //            if (TxtAgentName === '') {
    //                alert('Client field cannot be empty.');
    //                return false;
    //            }

    //            if (TxtMobile1 === '') {
    //                alert('Contact Person Name field cannot be empty.');
    //                return false;
    //            }
    //            if (TxtContactPersonName1 === '') {
    //                alert('Mobile No field cannot be empty.');
    //                return false;
    //            }
    //            //////////////
    //            $('#btnSaveClient').attr('disabled',false);
    //            $.ajax({

    //               // url: "Agent_Master/AddEditAgent",
    //                url: applicationUrl + ' Agent_Master/AddEditAgent',
    //                type: 'POST',
    //                data: $("#frmClient").serialize(),
    //                success: function (result) {
    //                    $("#loading").hide();
    //                    result.refresh;
    //                    var res = result.Agentexist;
    //                    var res1 = result.result;

    //                    if (res == 'AlreadyExist' && res1 == false) {
    //                        alert('Entered Client and Mobile no Already Exists');
    //                        $('#TxtAgentName').focus();
    //                        $('#btnSaveClient').attr('disabled', true);
    //                        return false;
    //                    }


    //                    else {
    //                        $('#btnSaveClient').attr('disabled', true);
    //                        alert("Record Saved Successfully.");
    //                        window.location.href = applicationUrl + 'Agent_Master/AgentMaster';
    //                        // $.post("/ManageUsers/ManageUsersList/", function (data) { }); does not work IE11
    //                        // window.location = '/ManageUsers/ManageUsersList/';
    //                        return true;
    //                    }
    //                }
    //            });
    //        }
    //        else {
    //    $('#btnSaveClient').attr('disabled', false);
    //        }
    //    });

    $("#btnSaveClient").click(function () {

        if ($("#frmClient").valid()) {
            $("#loading").show();
            ////////////////
            var TxtAgentName = $.trim($('#TxtAgentName').val());
            var TxtMobile1 = $.trim($('#TxtMobile1').val());

            if (TxtAgentName === '') {
                alert('Category field cannot be empty.');
                return false;
            }

            if (TxtMobile1 === '') {
                alert('Type field cannot be empty.');
                return false;
            }
            //////////////
            $('#btnSaveClient').attr('disabled', true);
            $.ajax({
                url: applicationUrl + "Agent_Master/AddEditAgent",
                type: 'POST',
                data: $("#frmClient").serialize(),
                success: function (result) {
                    $("#loading").hide();
                    result.refresh;
                    var res = result.Categoryexist;
                    var res1 = result.result;

                    if (res == 'AlreadyExist' && res1 == false) {
                        alert('Entered Category and Type Already Exists');
                        $('#TxtCategory').focus();
                        $('#btnSaveClient').attr('disabled', false);
                        return false;
                    }


                    else {
                        $('#btnSaveClient').attr('disabled', true);
                        alert("Record Saved Successfully.");
                        window.location.href = applicationUrl + 'Agent_Master/AgentMaster';
                        // $.post("/ManageUsers/ManageUsersList/", function (data) { }); does not work IE11
                        // window.location = '/ManageUsers/ManageUsersList/';
                        return true;
                    }
                }
            });
        }
        else {
            $('#btnSaveClient').attr('disabled', false);
        }
    });
    $('body').on('click', '.clsDelete', function (e) {

        if (e.handled != true) {
            var ID = $(this).attr('data-id');
            if (confirm("Are you sure you want to delete this Client?")) {
                $.ajax({

                    url: applicationUrl + "Agent_Master/DeleteAgent",
                    data: { id: ID },
                    type: 'GET',
                    success: function (result) {


                        result.refresh
                        var Alert = result.resultmessage
                        var ResultStatus = result.result

                        if (Alert == "Save") {
                            alert("Data Deleted Sucessfully");
                            ReloadGrid();

                        }
                        else if (Alert == "Error") {
                            alert("The specified Client cannot be deleted as it has a mapping with Client Details.");
                        }

                    }
                });
            }
            e.handled = true;
        }
        return false;
    });
});
function LoadGrid() {

    $("#grid").jqGrid({
        url: applicationUrl + "FacultyMaster/GetFacultyList",

        datatype: "json",
        mtype: 'POST',
        postData: {
            facultyName: function () { return $("#txtSearch").val(); },
            Dean: function () { return $("#txtDean").val(); },
            
            _search: function () { return "false"; }
        },

        colNames: ['Faculty Code', 'Faculty Name', 'Dean', 'Year Of Establishment','Activated','Update User','Update Date','Actions'],
        //colNames: ['ClientId', 'ClientName', 'ContactPersonName', 'MobileNo', 'EmailId', 'IsConsignee', 'CreatedBy', 'CreatedDate', 'UpdatedBy', 'UpdatedDate', 'Actions'],
        colModel: [
            { key: false, hidden: true, name: 'facultyID', index: 'facultyID', align: "left" },
            { key: false, name: 'facultyName', index: 'facultyName', align: "left", resizable: false },
            { key: true,  name: 'dean', index: 'dean', align: "left", resizable: false },
            { key: true, name: 'yearOfEstablishment', index: 'yearOfEstablishment', align: "left", resizable: false },
            { key: true,  name: 'activated', index: 'activated', align: "left", resizable: false },
            { key: true, name: 'updateUser', index: 'dean', align: "left", resizable: false },
            { key: true, hidden: false, name: 'updateDate', index: 'dean', align: "left", resizable: false },
            
           

            {
                name: 'LinkButton',
                formatter: function (cellvalue, options, rowObject) {
                    var userid;
                    var editLink = '<a id="btnEdit" title="Edit" href="../FacultyMaster/EditFaculty?facultyID=' + rowObject["facultyID"] + '"><img src="../Content/Images/delete2.png" height="20" alt="Edit" /></a>';
                    var deleteLink = '<a href="#" id="btnDelete" title="Delete" class="clsDelete" data-id="' + rowObject["AgentId"] + '"><img src="../Content/Images/delete2.png" height="20" alt="Delete" /></a>';
                    return editLink + deleteLink;
                }, sortable: false, search: false, align: 'center', width: 80, resizable: false
            }

        ],
        pager: jQuery('#pager'),
        rowNum: 10,
        rownumbers: false,
        rowList: [10, 20, 30, 40, 50, 100],
        height: '100%',
        viewrecords: true,
        caption: 'Faculty List',
        sortname: 'facultyID',
        sortorder: "desc",
        emptyrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false,
    }).navGrid('#pager', { edit: false, add: false, del: false, search: false, refresh: true })
    ///Export Functionality 
    //}).navGrid('#pager', { edit: false, add: false, del: false, search: false, refresh: true }).navButtonAdd('#pager', {
    //    caption: "Export to Excel",
    //    buttonicon: "ui-icon-disk",
    //    onClickButton: function (e) {
    //        if (e.handled != true) {
    //            ExportDataToExcel();
    //            e.handled = true;
    //        }
    //    },
    //    position: "last"
    //});
}

function ReloadGrid() {
    $("#grid").jqGrid('setGridParam', { search: false, postData: { "filters": "" } }).trigger("reloadGrid", [{ current: true, page: 1 }]);
}