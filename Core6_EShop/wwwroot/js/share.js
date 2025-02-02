//=======
//共用js
//=======
//Vue擴充
const vueComponent =
{
    //只允許輸入整數
    'integer-only': Vue.defineComponent({
        props: {
            modelValue: {
                type: Number,
                required: true
            }
        },
        data() {
            return {
                isInputActive: false
            }
        },
        methods: {
            inputOnFocus: (_event) => {
                _event.target.select();
                this.isInputActive = true;
            },
        },
        computed: {
            displayValue: {
                get() {
                    if (this.isInputActive) {
                        return this.modelValue.toString();
                    } else {
                        return $$toCurrency(this.modelValue);
                    }
                },
                set(_modifiedValue) {
                    let newValue = parseFloat(_modifiedValue.replace(/[^0-9]/g, ''));
                    if (isNaN(newValue)) {
                        newValue = 0
                    }
                    this.$emit('update:modelValue', newValue);
                }
            }
        },
        template: `<input type="tel" class="form-control" v-model="displayValue" @@blur="isInputActive=false" @@focus="this.isInputActive = true">`
    }),
};
//開啟視窗
async function $$openPanel(_partialViewPath, _viewParameter = {}) {
    return new Promise((resolve, reject) => {
        var cloneModalElement = document
            .getElementById('modal_Template').firstElementChild
            .cloneNode(true);
        $(cloneModalElement).css('background-color', '#4f4e4e82');
        if (_viewParameter == null) {
            _viewParameter = {};
        }
        //Id
        cloneModalElement.id = 'Modal_' + new Date().getTime();
        _viewParameter.modalId = cloneModalElement.id;
        //視窗大小(modalSize)
        if (_viewParameter.modalSize != null) {
            var modalSize = 'modal-xl';
            if (_viewParameter.modalSize == 'lg')
                modalSize = 'modal-lg';
            else if (_viewParameter.modalSize == 'sm')
                modalSize = 'modal-sm';
            $(cloneModalElement)
                .find('.modal-dialog')
                .removeClass('modal-xl');
            $(cloneModalElement)
                .find('.modal-dialog')
                .addClass(modalSize);
        }
        var myModal = new bootstrap
            .Modal(cloneModalElement, {});
        //關閉時
        cloneModalElement
            .addEventListener('hidden.bs.modal', function (event) {
                cloneModalElement.remove();
                resolve();
            });
        //開啟時
        cloneModalElement
            .addEventListener('shown.bs.modal', function (event) {
                //標頭(modalTitle)
                if (_viewParameter.modalTitle != null) {
                    $(cloneModalElement)
                        .find('.modal-title')
                        .text(_viewParameter.modalTitle);
                } else {
                    $(cloneModalElement)
                        .find('.modal-title')
                        .text('');
                }
                //內容
                $(cloneModalElement)
                    .find('.modal-body')
                    .load(_partialViewPath, _viewParameter, () => { });
            });
        myModal.show();
    });
}
//關閉視窗
function $$closePanel() {
    var modalDatas = $('.modal .btn-close');
    var len = modalDatas.length;
    if (len > 1)
        modalDatas[len - 1].click();
}
//開啟訊息視窗
function $$showMsg(_parameter = {}) {
    if (_parameter == null)
        _parameter = {};
    _parameter.title = _parameter.title == null ? '' : _parameter.title;
    _parameter.text = _parameter.message == null ? '' : _parameter.message;
    _parameter.icon = _parameter.icon == null ? 'info' : _parameter.icon;
    _parameter.timer = _parameter.timer == null ? 5000 : _parameter.timer;
    _parameter.timerProgressBar = true;
    _parameter.allowOutsideClick = _parameter.allowOutsideClick == null ? false : _parameter.allowOutsideClick;
    _parameter.showCloseButton = _parameter.showCloseButton == null ? true : _parameter.showCloseButton;
    _parameter.showCancelButton = _parameter.showCancelButton == null ? false : _parameter.showCancelButton;
    _parameter.confirmButtonText = _parameter.confirmButtonText == null ? '確定' : _parameter.confirmButtonText;
    _parameter.cancelButtonText = _parameter.cancelButtonText == null ? '取消' : _parameter.cancelButtonText;
    return Swal.fire(_parameter);
}
//開啟訊息視窗
function $$showToast(_parameter = {}) {
    _parameter.title = _parameter.title == null ? '' : _parameter.title;
    _parameter.icon = _parameter.icon == null ? 'success' : _parameter.icon;
    const Toast = Swal.mixin({
        toast: true,
        position: "bottom-end",
        showConfirmButton: false,
        timer: 2000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    return Toast.fire(_parameter);
}
//顯示資料驗證結果
function $$showValidationResult(_containerId = '', _validateDatas = []) {
    if (_containerId == null)
        return;
    if (_validateDatas == null || _validateDatas.length == 0)
        return;
    $('#' + _containerId +' .myValidation').remove();
    for (var i = 0; i < _validateDatas.length; i++) {
        var errors = "";
        if (_validateDatas[i].errorDatas != null && _validateDatas[i].errorDatas.length > 0) {
            errors = _validateDatas[i].errorDatas
                .join('\r\n');
        }
        $('#' + _containerId)
            .find('[name="' + _validateDatas[i].fieldFullName + '"]')
            .after(`<div class="myValidation invalid-feedback m-0"><i class="bi bi-exclamation-circle"></i> ${errors}</div>`);
    }
    if ($(".myValidation").length > 0) {
        $('#' + _containerId).parent().animate({
            scrollTop: 0
        }, 0);
        $('#' + _containerId).parent().animate({
            scrollTop: $(".myValidation").offset().top - 100
        }, 1500);
    }
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
function $$ajaxPromise(_url = '', _parameter = {} , _ajaxType = 'POST', loaderAni = true) {
    //若須驗證需在後端加上[Authorize]屬性
    return new Promise((_resolve, _reject) => {
        if (loaderAni == true)
            $$loder(true);
        //const jwtToken = $$getCookie('jwtToken');
        $.ajax({
            type: _ajaxType,
            url: _url,
            data: _ajaxType.toLowerCase() == 'post' ? JSON.stringify(_parameter) : _parameter,
            contentType: "application/json",
            success: function (rs) {
                if (loaderAni == true)
                    $$loder(false);
                _resolve(rs);
            },
            error: function (rs) {
                if (loaderAni == true)
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
function $$getNow() {
    const date = new Date();
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, '0');
    const dd = String(date.getDate()).padStart(2, '0');
    const hh = String(date.getHours()).padStart(2, '0');
    const min = String(date.getMinutes()).padStart(2, '0');
    const ss = String(date.getSeconds()).padStart(2, '0');
    const fff = String(date.getMilliseconds()).padStart(3, '0');
    return `${yyyy}-${mm}-${dd} ${hh}:${min}:${ss} ${fff}`;
}
function $$exePromise(_promiseArr = []) {
    if (_promiseArr == null || _promiseArr.length == 0)
        return;
    console.log($$getNow());
    $$loder(true);
    return new Promise((_resolve, _reject) => {
        Promise.all(_promiseArr).then(
            (values) => {
                console.log($$getNow());
                _resolve(values);
                $$loder(false);
            },
            (reason) => {
                _resolve(reason);
                $$loder(false);
            },
        );
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
    if (String(_num).indexOf(',') > -1) {
        _num = _num.replaceAll(',', '');
    }
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
    $$setCookie(_name, '', -1);
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
//複製資料
function $$clone(_obj = {}) {
    if (_obj === null)
        return null;
    else if (_obj === undefined)
        return undefined;
    return JSON.parse(JSON.stringify(_obj));
}