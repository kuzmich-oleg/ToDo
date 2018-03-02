$(document).ready(function () {
    var selectedCategory = $("select[name='selectCategory']");

    $("#loadList").on("click", function () {
        $.ajax({
            url: "home/getlist?categoryid=" + selectedCategory.val(),
            dataType: "html",
            success: function (html) {
                $("#listContainer").html(html);
                LoadListScripts();

                $("select[name='categoryId']>option[value=" + selectedCategory.val() + "]").attr("selected", "selected");
            }
        });
    });

    function LoadListScripts() {
        $("[data-toggle='tooltip']").tooltip();

        $("#send").on("click", function () {
            var taskName = $("input[name='taskName']");

            var urlAddress = "name=" + taskName.val() + "&priority=" + $("input[name='priority']").val() + "&status=" +
                $("select[name='statusId']").val() + "&category=" + $("select[name='categoryId']").val() +
                "&selectedcategory=" + selectedCategory.val();

            if (taskName.val())
                $.ajax({
                    url: "/home/add?" + urlAddress,
                    type: "put",
                    dataType: "html",
                    success: function (html) {
                        $("#listContainer").html(html);
                        LoadListScripts();
                    }
                });
            else
                alert("Fill all fields to add new task");
        });

        $(".glyphicon-remove").on("click", function (event) {
            var eventTarget = event.target;
            var taskId = eventTarget.getAttribute("id").split("_")[1];

            $.ajax({
                url: "/home/delete?taskid=" + taskId + "&selectedcategory=" + selectedCategory.val(),
                type: "delete",
                dataType: "html",
                success: function (html) {
                    $("#listContainer").html(html);
                    LoadListScripts();
                }
            });
        });

        $("select[name='statusName']").on("change", function (e) {
            var eventTarget = e.target;
            changeStatus(eventTarget.getAttribute("id"));
        });

        function changeStatus(taskId) {
            $.ajax({
                url: "/home/changestatus?taskid="
                + taskId.split("_")[1] + "&statusid=" + $("#" + taskId).val() + "&selectedcategory=" + selectedCategory.val(),
                type: "post",
                dataType: "html",
                success: function (html) {
                    $("#listContainer").html(html);
                    LoadListScripts();
                }
            });
        }

        $(".glyphicon-arrow-up, .glyphicon-arrow-down").on("click", function (event) {
            changePriority(event.target.getAttribute("id"));
        });

        function changePriority(taskId) {
            var params = taskId.split("_");

            if (params[2] == 1 && params[0] == "up" || params[2] == $("input[name='priority']").val() - 1 && params[0] == "down")
                alert("chose another task to move");
            else
                $.ajax({
                    url: "/home/changepriority?taskid=" + params[1] + "&priority=" + params[2] +
                    "&direction=" + params[0] + "&selectedcategory=" + selectedCategory.val(),
                    type: "post",
                    dataType: "html",
                    success: function (html) {
                        $("#listContainer").html(html);
                        LoadListScripts();
                    }
                });
        }
    }
});
