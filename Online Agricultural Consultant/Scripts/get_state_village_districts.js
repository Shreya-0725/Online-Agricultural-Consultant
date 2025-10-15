function findDistrictList() {
    debugger
    if ($("#StateName").val() == "") {
        $("#DistrictNAme").empty();
        $("#DistrictNAme").append("<option value=''>-Select District-</option>");
        return false;
    }
    $.get("/Home/getDistrictByStateCode", { statecode: $("#StateName").val() }, function (data) {
        $("#DistrictNAme").empty();
        $("#DistrictNAme").append("<option value=''>-Select District-</option>");
        $.each(data, function (index, row) {
            $("#DistrictNAme").append("<option value='" + row.DestrictCode + "'>" + row.DestrictName + "</option>")
        });
    })
}