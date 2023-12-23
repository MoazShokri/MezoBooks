var dataTable;

$(document).ready(function () {
    loadDataTable();
});

"render": function (data) {
    var today = new Date().getTime();
    var lockout = new Date(data.lockoutEnd).getTime();

    if (lockout > today) {
        return `
        <div class="text-center">
            <div class="btn-group" role="group">
                <a onclick="LockUnlock('${data.id}')" class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                    <i class="bi bi-lock-fill"></i> Lock
                </a> 
                <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                    <i class="bi bi-pencil-square"></i> Permission
                </a>
            </div>
        </div>
        `;
    } else {
        return `
        <div class="text-center">
            <div class="btn-group" role="group">
                <a onclick="LockUnlock('${data.id}')" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                    <i class="bi bi-unlock-fill"></i> Unlock
                </a>
                <a href="/admin/user/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                    <i class="bi bi-pencil-square"></i> Permission
                </a>
            </div>
        </div>
        `;
    }
},


function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}