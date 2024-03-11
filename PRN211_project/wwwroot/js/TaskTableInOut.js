const divId = document.querySelector("#taskId");
function loadData(id) {
    document.querySelector("#resetBtn").click();
    $.ajax({
        url: "/task/getData",
        type: "post", //send it through get method
        data: {
            taskId: id,
        },
        success: function (response) {
            var object = JSON.parse(response);
            console.log(object);
            document.querySelector("#Name").value = object.Name;
            // Assuming you have initialized TinyMCE as in your provided code
            var editor = tinymce.get('Description');
            // Check if the editor instance exists
            if (editor) {
                // Set the content of the editor
                editor.setContent(object.Description);
            } else {
                // If the editor instance doesn't exist, you can set the content using the HTML element directly
                document.querySelector("#Description").innerHTML = object.Description;
            }

            document.querySelector("#ListId").value = object.ListId;
            document.querySelector("#Status").value = object.Status;
            divId.value = object.Id;
            document.querySelector("#deleteBtn").href = '/task/delete?id=' + object.Id;


            var originalDueDate = object.DueDate;
            var formattedDueDate = originalDueDate.split('T')[0];
            document.querySelector("#DueDate").value = formattedDueDate;

        },
        error: function (xhr) {
            //Do Something to handle error
        }
    });
}
function updateSubmit() {
    // this is the id of the form
    $("#editTaskForm").submit(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var form = $(this);
        var actionUrl = form.attr('action');

        $.ajax({
            type: "POST",
            url: actionUrl,
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                var object = JSON.parse(data);
                changeNameText(object.Id, object.Name);
                showAlert("Saved all changes");
            }
        });
    });
}
function insertSubmit() {
    $("#insertTaskForm").submit(function (e) {

        e.preventDefault(); // avoid to execute the actual submit of the form.

        var form = $(this);
        var actionUrl = form.attr('action');

        $.ajax({
            type: "POST",
            url: actionUrl,
            data: form.serialize(), // serializes the form's elements.
            success: function (data) {
                setTimeout(function () {
                    location.reload();
                }, 300); // 2000 milliseconds = 2 seconds
            }
        });
    });
}
document.getElementById("deleteBtn").addEventListener("click", function () {
    if (confirm('are you sure to delete?')) {
        window.location.href = "/task/delete?id=" + document.getElementById("taskId").value;
    }
});
function changeNameText(taskid, taskName) {
    document.querySelector("#nameTask-" + taskid).innerText = taskName;
}

function checkBoxHandle(checkId) {
    $.ajax({
        url: '/task/checkStatus',
        type: "get",// send it through get method
        data: {
            id: checkId
        },
        success: function (response) {
            var object = JSON.parse(response);

            var textTask = document.querySelector("#nameTask-" + object.Id);
            if (object.Status == true) {
                textTask.style.setProperty("text-decoration", "line-through");
            } else {
                textTask.style.setProperty("text-decoration", "none");
            }
        },
        error: function (xhr) {
            //Do Something to handle error

        }
    });
}
function openNav() {
    document.getElementById("mySidebar").style.width = "500px";
    document.getElementById("main").style.marginRight = "500px";
}

function closeNav() {
    var insertTaskForm = document.getElementById("insertTaskForm");
    var editTaskForm = document.getElementById("editTaskForm");
    setTimeout(function () {
        insertTaskForm.style.display = "none";
        editTaskForm.style.display = "none";
    }, 300); // 2000 milliseconds = 2 seconds
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginRight = "0";
}