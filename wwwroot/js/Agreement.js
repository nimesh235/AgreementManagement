    const dateformat = 'mm/dd/yyyy';
$(function () {

    $("#newAgreementButton").click(function () {
        $.ajax({
            url: "/Agreements/Create",
            type: "GET", 
            headers: {
                RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (result) {
                bindDialogModel(result, true);
            },
            error: function (error) {
                console.error(error);
            }
        });
        
    });
   
});
$(document).ready(function () {
    $('#dataTable').DataTable({
        processing: true,
        serverSide: true,
        searching: true,
        ordering: true,
        ajax: {
            url: '/Agreements/Search',
            type: 'POST',
            data: function (d) {
                d.sortColumn = d.order[0].column;
                d.searchValue = d.search.value;
                d.sortDirection = d.order[0].dir;
            },
            dataSrc: 'data'
        },
        columns: [
            { data: 'user.userName' },
            {
                data: 'product.productGroup',
                render: function (data, type, row) {
                    return '<span title="' + data.groupDescription + '">' + data.groupCode + '</span>';
                }
            },
            {
                data: 'product',
                render: function (data, type, row) {
                    return '<span title="' + data.productDescription + '">' + data.productNumber + '</span>';
                }
            },
            {
                data: 'effectiveDate',
                render: function (data, type, row) {
                    var date = new Date(data);
                    return '<span>' + date.toLocaleDateString() + '</span>';
                }
            },
            {
                data: 'expirationDate',
                render: function (data, type, row) {
                    var date = new Date(data);
                    return '<span>' + date.toLocaleDateString() + '</span>';
                }            },
            { data:'product.price'},
            { data:'newPrice'},
            {
                data: 'id',
                render: function (data, type, row) {
                    return '<div class="row badge"><button class="m-1 btn btn-primary" onclick=openAgreementDialog(' + data + ')>Edit</button>' +
                        '<button class="m-1 btn btn-danger" onclick=DeleteAgreementDialog(' + data + ')>Delete</button></div>';

                }
               
            }, 
        ],
        "order": [[0, 'asc']], 
        "pageLength": 10, 
        "paging": true, 
        "info": true,

        initComplete: function () {
            var table = this.api();
            table.ajax.reload();
        }
    });
});

function openAgreementDialog(data) {
    $.ajax({
        url: "/Agreements/Create",
        type: "GET",
        headers: {
            RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        data: {
            id:data
        },
        success: function (result) {
            bindDialogModel(result, false);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function DeleteAgreementDialog(data) {
    $("#agreementDialog").html("<h4>Are You Want To Delete Record???</h4><input type='hidden' id='DeleteRecordId' value='" + data+"'>");
    $("#agreementDialog").dialog({
        modal: true,
        width: 650,
        title:  "Delete Agreement",
        buttons: {
            "Yes": function () {
                
                $.ajax({
                    url: "/Agreements/Delete",
                    type: "Post",
                    headers: {
                        RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                    },
                    data: {
                        Id: $("#DeleteRecordId").val(),
                        
                    },
                    success: function (result) {
                        $("#agreementDialog").html(result.status);
                        $("#agreementDialog").dialog({
                            modal: true,
                            width: 650,
                            title: "Delete Agreement",
                            buttons: {
                                "Okay": function () {
                                    $('#dataTable').DataTable().ajax.reload();
                                    $(this).dialog("close");
                                }
                            }
                        });
                        $('#dataTable').DataTable().ajax.reload();
                    },
                    error: function (error) {
                        $("#agreementDialog").html(error);
                    }
                });
            },
            "No": function () {
                $('#dataTable').DataTable().ajax.reload();
                $(this).dialog("close");
            }
        }
    });
}

function bindDialogModel(result,isCreate) {
    $("#agreementDialog").html(result);
    DatePickerForate();
    bindProductGroupCange();
    $("#agreementDialog").dialog({
        modal: true,
        width: 650,
        title: isCreate == true ? "New Agreement" :"Edit Agreement",
        buttons: {
            "Save": function () {
                $.ajax({
                    url: "/Agreements/Create",
                    type: "Post",
                    headers: {
                        RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                    },
                    data: {
                        Id: $("#Id").val(),
                        UserId: $("#UserId").val(),
                        ProductGroupId: $("#ProductGroupId").val(),
                        ProductId: $("#ProductId").val(),
                        EffectiveDate: convertDateFormat($("#EffectiveDate").val()),
                        ExpirationDate: convertDateFormat($("#ExpirationDate").val()),
                        NewPrice: $("#NewPrice").val(),
                        Active: $("#Active").val()
                    },
                    success: function (result) {
                        $("#agreementDialog").html(result);
                        DatePickerForate();
                        bindProductGroupCange();
                        $('#dataTable').DataTable().ajax.reload();
                    },
                    error: function (error) {
                        $("#agreementDialog").html(error);
                    }
                });
            },
            "Cancel": function () {
                $('#dataTable').DataTable().ajax.reload();
                $(this).dialog("close");
            }
        }
    });
}

function convertDateFormat(inputDate) {
    var datePattern = /^(\d{2})\/(\d{2})\/(\d{4})$/;

    if (datePattern.test(inputDate)) {
        var formattedDate = inputDate.replace(datePattern, '$2/$1/$3');
        return formattedDate;
    } else {
        return inputDate;
    }
}
function DatePickerForate(){
    $('.Datepicker').datepicker({
        format: dateformat,
        autoclose: true,
        changeMonth: true,
        changeYear: true
    });
}
function bindProductGroupCange() {
    $("#ProductGroupId").change(function () {
        var selectedValue = $(this).val();
        $.ajax({
            url: "/Agreements/GetProduct",
            type: "GET",
            headers: {
                RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            data: { ProductGroupId: selectedValue },
            success: function (data) {
                $("#ProductId").empty();

                $.each(data, function (key, value) {
                    $("#ProductId").append('<option value="' + key + '">' + value + '</option>');
                });
            },
            error: function (error) {
                console.error(error);
            }
        });

    });
}
