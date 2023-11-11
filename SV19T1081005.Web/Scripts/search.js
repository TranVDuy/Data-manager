function doSearch(page) {
    var url = $("#searchInput").prop("action");//lấy link action của form
    var input = $("#searchInput").serializeArray();//lấy dữ liệu nhập trong form
    input.push({
        "name": "page",
        "value": page
    });

    //thực hiện lời gọi API để lấy dữ liệu về
    $.ajax({
        type: "GET",
        url: url,
        data: input,
        success: function (data) {
            $("#searchResult").html(data);
        }
    });
};

