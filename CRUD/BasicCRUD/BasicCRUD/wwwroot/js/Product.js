$(document).ready(function () {
    inittoastrOptions();
    loadProductGridDataTable();
});

function inittoastrOptions(){
    toastr.options = { positionClass: "toast-top-center", "preventDuplicates": true };
}

function loadProductGridDataTable() {
    dataTable = $('#tblProductGrid').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAllProducts",
        },
        columns:[
            {
                'data': 'id',
                'render': function (data) {
                    return `<div class="w-75 btn-group" role="group"><a class="btn btn-primary mx-2" href="/Admin/Product/Upsert?id=${data}"><i class="bi bi-pencil-fill mr-2"></i>Edit</a><a class="btn btn-danger mx-2" onclick="DeleteProduct('/Admin/Product/Delete?id=${data}')"><i class="bi bi-trash"></i>Delete</a></div>`
                }
            },
            { 'data': 'title', 'width':'15%' },
            { 'data': 'isbn', 'width':'15%' },
            { 'data': 'author', 'width':'15%' },
            { 'data': 'price', 'width': '15%' },
            { 'data': 'category.name', 'width': '15%' }
        ]
    })
}

function DeleteProduct(url) {
    Swal.fire({
        title: "Confirmation",
        text: "Are you sure, you want to delete this Record?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText:"Yes"
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