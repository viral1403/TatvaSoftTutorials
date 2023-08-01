$(document).ready(function (){
    inittoastrOptions();
    loadComapnyDataTable();
});

function inittoastrOptions() {
    toastr.options = { positionClass: "toast-top-center", "preventDuplicates": true };
}

function loadComapnyDataTable() {
    dataTable = $('#tblCompany').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetCompanies",
        },
        columns: [
            {
                'data': 'id',
                'render': function (data) {
                    return `<div class="w-75 btn-group" role="group"><a class="btn btn-primary mx-2" href="/Admin/Company/Upsert?id=${data}"><i class="bi bi-pencil-fill mr-2"></i>Edit</a><a class="btn btn-danger mx-2" onclick="deleteCompany('/Admin/Company/Delete?id=${data}')"><i class="bi bi-trash"></i>Delete</a></div>`
                }
            },
            { 'data': 'name', 'width': '15%' },
            { 'data': 'streetAddress', 'width': '15%' },
            { 'data': 'city', 'width': '15%' },
            { 'data': 'state', 'width': '15%' },
            { 'data': 'phoneNumber', 'width': '15%' }
        ]
    })
}

function deleteCompany(url) {
    Swal.fire({
        title: "Confirmation",
        text: "Are you sure, you want to delete this Record?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}