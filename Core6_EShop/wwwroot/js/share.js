//=======
//共用js
//=======

//開啟訊息視窗
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
//載入動畫
function $$loder(_start = true) {
    document.getElementById('loderElement').classList.remove('d-none');
    if (_start != true)
        document.getElementById('loderElement').classList.add('d-none');
}
//是否為空值或null
function $$isNullorEmpty(_val) {
    return _val === null || _val === undefined || _val === ''; 
}
//ajax使用promise
function $$ajaxPromise(_url = '', _josnParameter = '', loaderAni = true) {
    //若須驗證需在後端加上[Authorize]屬性
    return new Promise((_resolve, _reject) => {
        if (loaderAni == true)
            $$loder(true);
        const jwtToken = $$getCookie('jwtToken');
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
                    $$deleteCookie('jwtToken');
                    $$deleteCookie('userName');
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
//數字千分位
function $$toCurrency(_num, _decimalLenght = null) {
    if (_num == null) return;
    var parts = _num.toString().split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    if (!$$isNullorEmpty(_decimalLenght) && !$$isNullorEmpty(parts[1])) {
        parts[1] = parts[1].substring(0, _decimalLenght);
        if (parts[1] == '')
            parts.splice(1, 1);
    }
    return parts.join('.');
}
//小數處理
function $$safeRound(v, n) {
    if (v % 1 !== 0) {
        v = parseFloat(v.toPrecision(15));
    }
    var t = Math.pow(10, n);
    var nv = v * t;
    if (nv % 1 !== 0) {
        nv = parseFloat(nv.toPrecision(15));
    }
    return Math.round(nv) / t;
}
//轉數字
function $$toNum(_num, _decimalPlace = 2) {
    if ($$isNullorEmpty(_num))
        return 0;
    return $$safeRound(_num, _decimalPlace);
}
//設置cookie
function $$setCookie(_name, _val, _hour = 1) {
    var date = new Date();
    date.setTime(date.getTime() + ((60 * 60 * 1000) * _hour));
    document.cookie = `${_name}=${_val};expires=${date.toGMTString()};path=/`;
}
function $$getCookie(_name) {
    var nameEQ = _name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function $$deleteCookie(_name) {
    document.cookie = _name + '=; Max-Age=-99999999;';
}

//附件預覽
function $$previewFile(name, type, obj) {
    type = String(type).toLowerCase();
    function $$base64ToArrayBuffer(base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }
    function $$saveByteArray(filename, type, byte) {
        var blob = new Blob([byte], { type: `application/${type}` });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = `${filename}.${type}`;
        link.click();
    };
    switch (type) {
        case "pdf":
            if (name == null || name == '') name = '附件';
            var byteArray = $$base64ToArrayBuffer(obj);
            var file = new Blob([byteArray], { type: `application/${type};base64` });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL, `預覽檔案${new Date().getTime()}`, "height=85%");
            break;
        case "jpeg":
        case "jpg":
        case "bmp":
        case "png":
        case "gif":
            var img = `<img src='data:image/${type};base64,${obj}'">`;
            var win = window.open("", `預覽檔案${new Date().getTime()}`, "height=85%");
            win.document.write(img);
            break;
        default:
            if (obj == "" || obj == null) {
                $$showMsg({ title: `無文件可下載`, message: '', icon: 'info' });
                return;
            }
            var base64Arr = $$base64ToArrayBuffer(obj);
            if (name == '' || name == null)
                name = `附件${new Date().getTime()}`;
            $$saveByteArray(name, type, base64Arr);
            break;
    }
}