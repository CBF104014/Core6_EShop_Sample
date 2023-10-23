//共用js
function $$showMsg(_title = '-', _message = '-', _icon = 'info') {
    return Swal.fire({
        title: _title,
        text: _message,
        icon: _icon,
        allowOutsideClick: false,
        showCloseButton: true,
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
    });
}