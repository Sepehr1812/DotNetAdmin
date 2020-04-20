var dataTable;
var table = window._table;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/admin/gettables?table=" + document.getElementById('table_name').innerHTML,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "userName", "width": "10%"},
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "10%" },
            { "data": "activeState", "width": "10%" },
            { "data": "isAdmin", "width": "10%" },
            { "data": "isBanned", "width": "10%" },
            { "data": "banUntil", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Admin/Edit?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:70px;">
                            Edit</a>
                        &nbsp;

                        <a href="/Admin/Ban?id=${data}" class="btn btn-danger text-white" style="cursor:pointer; width:70px;">
                            Ban</a>
                        &nbsp;

                        <a class="btn btn-danger text-white" style="cursor:pointer; width:70px;"
                            onclick=Delete('/Admin/Delete?id='+${data})>
                            Delete</a>
                        </div>`;
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
