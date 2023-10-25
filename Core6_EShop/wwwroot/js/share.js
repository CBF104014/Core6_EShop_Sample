//共用js
function $$showMsg(_parameter = {}) {
    return Swal.fire({
        title: _parameter.title == null ? '' : _parameter.title,
        text: _parameter.message == null ? '' : _parameter.message,
        icon: _parameter.icon == null ? 'info' : _parameter.icon,
        allowOutsideClick: false,
        showCloseButton: true,
        showCancelButton: false,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
    });
}
function $$loder(_start = true) {
    document.getElementById('loderElement').classList.remove('d-none');
    if (_start != true)
        document.getElementById('loderElement').classList.add('d-none');
}
function $$ajaxPromise(_url = '', _josnParameter = '', loaderAni = true) {
    return new Promise((_resolve, _reject) => {
        if (loaderAni == true)
            $$loder(true);
        $.ajax({
            type: "POST",
            url: _url,
            data: _josnParameter,
            contentType: "application/json",
            success: function (rs) {
                _resolve(rs);
                $$loder(false);
            },
            error: function (rs) {
                $$showMsg({ title: '錯誤', message: '錯誤', icon: 'error' });
                $$loder(false);
                _reject();
            }
        });
    });
}

function $$setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function $$getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}