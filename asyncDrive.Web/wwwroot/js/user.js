$(async function () {
    window.delete = function (url) {
        debugger
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                })
            }
        });
    }
 
    await GetToken();
    await GetConfig();
    const apiBaseUrl = window.getApiBaseUrl();
    await loadDataTable();
    var dataTable;    
    async function loadDataTable() {
        try {
            debugger
            const response = await fetch(apiBaseUrl+'Users?isAscending=true&pageNumber=1&pageSize=1000', {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
                    'Content-Type': 'application/json'
                },
                //credentials: 'include' // Include credentials such as cookies in the request
            });

            if (!response.ok) {
                if (response.status === 401) {
                    refreshToken();
                    //alert('Unauthorized. Please log in again.');
                }
                //throw new Error('Network response was not ok ' + response.statusText);
            }
            else {
                const data = await response.json();

                $('#tblData').DataTable({
                    data: data, // Assuming data is an array of arrays or array of objects
                    columns: [
                        { data: 'id', width: "20%" },
                        { data: 'firstName', width: "15%" },
                        { data: 'email', width: "10%" },
                        { data: 'phoneNumber', width: "15%" },
                        { data: 'postalCode', width: "15%" },
                        { data: 'city', width: "15%" },
                        { data: 'state', width: "15%" },
                        {
                            data: 'id',
                            "render": function (data) {
                                return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">Edit</a>
                                <a onClick="delete('/admin/user/delete/${data}')" class="btn btn-danger mx-2">Delete</a></div>`;
                            }
                        }
                    ]
                });
            }
        } catch (error) {
            console.error('Error:', error);
        }
    }
    
});