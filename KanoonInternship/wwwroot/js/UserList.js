var dataTable;
var table = document.getElementById('table_name').innerHTML;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/admin/gettables?table=" + table,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "userName", "width": "10%" },
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "10%" },
            { "data": "activeState", "width": "10%" },
            { "data": "isAdmin", "width": "10%" },
            { "data": "isBanned", "width": "10%" },
            { "data": "banUntil", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    if (table.startsWith("All")) {
                        return `<div class="text-center">
                        <a href="/Admin/Edit?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:70px;">
                            Edit</a>
                        &nbsp;

                        <a href="/Admin/Ban?id=${data}" class="btn btn-danger text-white" style="cursor:pointer; width:70px;">
                            Ban</a>
                        &nbsp;

                        <a class="btn btn-danger text-white" style="cursor:pointer; width:70px;"
                            onclick=Delete('/Admin/Delete?id=${data}')>
                            Delete</a>
                        </div>`;
                    }
                    else if (table.startsWith("Waiting")) {
                        return `<div class="text-center">
                            <a class="btn btn-success text-white" style="cursor:pointer; width:100px;"
                                onclick=ChangeProperty('/Admin/ChangeProperty?id=${data}&which=${"Active"}')>
                                Accept</a>
                            &nbsp;

                            <a class="btn btn-danger text-white" style="cursor:pointer; width:100px;"
                                onclick=ChangeProperty('/Admin/ChangeProperty?id=${data}&which=${"Reject"}')>
                                Reject</a>
                            </div>`;
                    }
                    else {
                        return `<div class="text-center">
                            <a class="btn btn-success text-white" style="cursor:pointer; width:100px;"
                                onclick=ChangeProperty('/Admin/ChangeProperty?id=${data}&which=${"Unban"}')>
                                Unban</a>
                            </div>`;
                    }
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        }, "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "If you delete the user, you will not able to undo.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function ChangeProperty(url) {
    swal({
        title: "Are you sure?",
        text: "If you do it, you will not able to undo.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDo) => {
        if (willDo) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
