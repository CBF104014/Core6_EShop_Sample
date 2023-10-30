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