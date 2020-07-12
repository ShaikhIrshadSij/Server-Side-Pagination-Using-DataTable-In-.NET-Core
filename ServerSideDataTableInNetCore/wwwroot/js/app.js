$(document).ready(function () {

    $("#test-registers").DataTable({
        autoWidth: true,
        processing: true,
        serverSide: true,
        paging: true,
        searching: { regex: true },
        ajax: {
            url: "/Home/LoadTable",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (data) {
                return JSON.stringify(data);
            }
        },
        columns: [
            { data: "name" },
            { data: "firstSurname" },
            { data: "secondSurname" },
            { data: "street" },
            { data: "phone" },
            { data: "zipCode" },
            { data: "country" },
            {
                data: "notes",
                render: function (data, type, row) {
                    return `<span style="color: green;">${data}</span>`;
                }
            }
        ]
    });
});