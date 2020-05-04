var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load_r').DataTable({
        "ajax": {
            "url": "/admin/getreports",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "writer", "width": "20%" },
            { "data": "date", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Admin/Show?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                            Show</a>
                        </div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        }, "width": "100%"
    });
}
