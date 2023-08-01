$(document).ready(function () {
    var url = new URL(window.location);
    if (url.searchParams.get('status') != null) {
        loadOrderGridDataTable(url.searchParams.get('status'));
    }
    else {
        loadOrderGridDataTable();
    }
})


function loadOrderGridDataTable(status) {
    dataTable = $('#tblOrderDetails').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAllOrders" + (status != null ? `?status=${status}`:""),
        },
        columns: [
            { 'data':'id','width':'5%' },
            { 'data': 'name', 'width': '20%' },
            { 'data': 'phoneNumber', 'width': '10%' },
            { 'data': 'applicationUser.email', 'width': '25%' },
            { 'data': 'orderStatus', 'width': '10%' },
            { 'data': 'orderTotal', 'width': '8%' },
            {
                'data': 'id',
                'width': '5%',
                'render': function (data) {
                    return `<a class="mx-2" href="/Admin/Order/Details?orderId=${data}"><i class="bi bi-info-circle-fill text-info mr-2"></i></a>`
                }
            }
        ]
    })
}