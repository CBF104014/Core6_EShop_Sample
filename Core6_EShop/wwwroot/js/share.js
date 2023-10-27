//共用js
function $$showMsg(_parameter = {}) {
    if (_parameter == null)
        _parameter = {};
    _parameter.title = _parameter.title == null ? '' : _parameter.title;
    _parameter.text = _parameter.message == null ? '' : _parameter.message;
    _parameter.icon = _parameter.icon == null ? 'info' : _parameter.icon;
    _parameter.allowOutsideClick = _parameter.allowOutsideClick == null ? false : _parameter.allowOutsideClick;
    _parameter.showCloseButton = _parameter.showCloseButton == null ? true : _parameter.showCloseButton;
    _parameter.showCancelButton = _parameter.showCancelButton == null ? false : _parameter.showCancelButton;
    _parameter.confirmButtonText = _parameter.confirmButtonText == null ? '確定' : _parameter.confirmButtonText;
    _parameter.cancelButtonText = _parameter.cancelButtonText == null ? '取消' : _parameter.cancelButtonText;
    return Swal.fire(_parameter);
}
function $$loder(_start = true) {
    document.getElementById('loderElement').classList.remove('d-none');
    if (_start != true)
        document.getElementById('loderElement').classList.add('d-none');
}
function $$isNullorEmpty(_val) {
    return _val === null || _val === undefined || _val === ''; 
}
function $$ajaxPromise(_url = '', _josnParameter = '', loaderAni = true) {
    //若須驗證需在後端加上[Authorize]屬性
    return new Promise((_resolve, _reject) => {
        if (loaderAni == true)
            $$loder(true);
        const jwtToken = localStorage.getItem('jwtToken');
        $.ajax({
            type: "POST",
            url: _url,
            headers: {
                'Authorization': 'Bearer ' + jwtToken
            },
            data: _josnParameter,
            contentType: "application/json",
            success: function (rs) {
                _resolve(rs);
                $$loder(false);
            },
            error: function (rs) {
                $$loder(false);
                _reject(rs);
                if (rs.status == 401) {
                    //未授權(未登入)
                    localStorage.clear();
                    $$showMsg({
                        title: '請先登入',
                        message: '',
                        icon: '',
                        timer: 10000,
                        timerProgressBar: true,
                    }).then(() => {
                        location.href = window.location.origin + '/Account/Login';
                    });
                } else {
                    $$showMsg({ title: '錯誤', message: '錯誤', icon: 'error' });
                }
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