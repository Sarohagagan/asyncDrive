$(async function () {
    debugger
    await GetToken();
    await GetConfig();
    const apiBaseUrl = window.getApiBaseUrl();
    await loadDataTable();
    
    async function GetConfig() {
        try {
            const response = await fetch('https://localhost:7122/api/config');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            // Use the configuration values as needed
            const config = await response.json();
            window.getConfig = function () {
                return config;
            };
            window.getApiBaseUrl = function () {
                return config.apiBaseUrl;
            };
        } catch (error) {
            console.error('Failed to fetch configuration:', error);
        }
    }
    async function GetToken() {
        try {
           // const jwtToken = localStorage.getItem('jwtToken');

            //if (!jwtToken || jwtToken.trim() === '') {
                const response = await fetch('https://localhost:7122/api/Session/data', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                if (!response.ok) {
                    throw new Error(`Network response was not ok: ${response.statusText}`);
                }
                const data = await response.json();
                localStorage.setItem('jwtToken', data.sessionData);
            //}
        }
        catch (error) {
            console.error('Fetch error:', error);
        }
    }

    //async function refreshAccessToken() {
    //    const response = await fetch(`${apiUrl}/refresh`, {
    //        method: "POST",
    //        headers: {
    //            "Content-Type": "application/json"
    //        },
    //        body: JSON.stringify({ accessToken, refreshToken })
    //    });
    //    const data = await response.json();
    //    accessToken = data.accessToken;
    //    refreshToken = data.refreshToken;
    //    console.log("Token refreshed successfully");
    //}
    var dataTable;
    //using AJAX
    //function loadDataTable() {
    //    $.ajax({
    //        url: 'https://localhost:7075/api/Users?isAscending=true&pageNumber=1&pageSize=1000',
    //        method: 'GET',
    //        contentType: 'application/json',
    //        xhrFields: {
    //          withCredentials: true // Include credentials if necessary
    //        },
    //        success: function (data) {
    //            // Assuming response.data is the array you're passing to DataTable
    //            if (data) {
    //                $('#tblData').DataTable({
    //                    data: data,
    //                    columns: [
    //                        { data: 'id', width: "20%" },
    //                        { data: 'firstName', width: "15%" },
    //                        { data: 'email', width: "10%" },
    //                        { data: 'phoneNumber', width: "15%" },
    //                        { data: 'postalCode', width: "15%" },
    //                        { data: 'city', width: "15%" },
    //                        { data: 'state', width: "15%" },
    //                        {
    //                            data: 'Id',
    //                            "render": function (data) {
    //                                return `<div class="w-75 btn-group" role="group">
    //                                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">Edit</a>
    //                                        <a onClick="Delete('/admin/product/delete/${data}')" class="btn btn-danger mx-2">Delete</a>`
    //                            }
    //                        }
    //                    ]
    //                });
    //            } else {
    //                console.error('Data is undefined or in an unexpected format');
    //            }
    //        },
    //        error: function (xhr, status, error) {
    //            console.error('Error fetching data:', error);
    //        }
    //    });

    //}
    ////using fetch
    async function loadDataTable() {
        try {
            
            const response = await fetch(apiBaseUrl+'Users?isAscending=true&pageNumber=1&pageSize=1000', {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
                    'Content-Type': 'application/json'
                },
                //credentials: 'include' // Include credentials such as cookies in the request
            });

            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }

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
                        data: 'Id',
                        "render": function (data) {
                            return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">Edit</a>
                                <a onClick="Delete('/admin/product/delete/${data}')" class="btn btn-danger mx-2">Delete</a></div>`;
                        }
                    }
                ]
            });
        } catch (error) {
            console.error('Error:', error);
        }
    }

    function Delete(url) {
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
});