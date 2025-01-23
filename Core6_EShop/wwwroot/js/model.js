
class $$kv {
    constructor(_key, _value, _disable) {
        this.key = _key;
        this.value = _value;
        this.disable = _disable;
    }
}
class $$dynamicBase {
    constructor(_object) {
        for (var item in _object) {
            this[item] = _object[item];
        }
    }
}
class $$fileBase {
    constructor(_fileObj = {}) {
        this.rankey = _fileObj.rankey == null ? 0 : _fileObj.rankey;
        this.fileId = _fileObj.fileId == null ? 0 : _fileObj.fileId;
        this.fileName = _fileObj.fileName == null ? '' : _fileObj.fileName;
        this.fileType = _fileObj.fileType == null ? '' : _fileObj.fileType;
        this.limitFileSize = _fileObj.limitFileSize == null ? 10 : _fileObj.limitFileSize;//MB
        this.limitFileTypeArr = {
            //'pdf': 'application/pdf',
            //'doc': 'application/msword',
            //'docx': 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            //'odt': 'application/vnd.oasis.opendocument.text',
            //'xls': 'application/vnd.ms-excel',
            //'xlsx': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            'jpeg': 'image/jpeg',
            'jpg': 'image/jpg',
            'png': 'image/png',
            'bmp': 'image/bmp',
        };
        this.byteData = _fileObj.byteData;
    }
    IsImage() {
        return this.fileType == 'png';
    }
    Test() {
        debugger;
    }
    //取得瀏覽資料
    GetFile(_targetFile, _uploadCallBack = () => { }) {
        return new Promise(async (resolve, reject) => {
            if (_targetFile == null) {
                resolve(false);
                return;
            }
            var fileData = _targetFile;
            //兩種驗證型態方式
            if ((fileData.type == '' && this.limitFileTypeArr[fileData.name.split('.').pop()] == null) ||
                (fileData.type != '' && !Object.values(this.limitFileTypeArr).includes(fileData.type))) {
                var msg = Object.keys(this.limitFileTypeArr).map(x => `.${x}`).join(' ');
                $$showMsg({ title: `上傳格式須為${msg}`, message: '', icon: 'warning' });
                resolve(false);
                return;
            }
            if (fileData.size / 1024 / 1024 > this.limitFileSize) {
                $$showMsg({ title: `附件大小需小於${this.limitFileSize}MB`, message: '', icon: 'warning' });
                resolve(false);
                return;
            }
            this.fileType = (/[.]/.exec(fileData.name)) ? /[^.]+$/.exec(fileData.name)[0] : null;
            if ($$isNullorEmpty(this.fileName)) {
                this.fileName = fileData.name.replace('.' + this.fileType, '');
            }
            var reader = new FileReader();
            reader.readAsDataURL(fileData);
            reader.onload = async (e) => {
                this.byteData = reader.result.split("base64,")[1];
                resolve(true);
            };
        });
    }
    //預覽附件
    PreView() {
        if ($$isNullorEmpty(this.byteData)) {
            $$showMsg({ title: `無資料可預覽`, message: '', icon: 'info' });
        } else {
            $$previewFile(this.fileName, this.fileType, this.byteData);
        }
    }
    //刪除附件
    DelFileWithOutAlert(_delCallBack = () => { }) {
        return new Promise(async (resolve, reject) => {
            if ($IsNullOrEmptyOrZero(this.Rankey())) {
                resolve(false);
                return;
            }
            var token = await $GetToken();
            $.AifAjax(`${SysInfo.ESS.SysUrl}/api/ESS/FileBase/FileReMove`, {
                data: {
                    SysNo: this.SysNo(),
                    Rankey: this.Rankey(),
                    token: token
                },
                success: (rs) => {
                    _delCallBack(rs);
                    resolve(rs.stateCode == 1);
                },
                error: (rs) => {
                    console.log(rs);
                    _delCallBack(rs);
                    resolve(false);
                }
            });
        });
    }
    //刪除附件(有提醒)
    DelFile(_delCallBack = () => { }) {
        return new Promise((resolve, reject) => {
            if ($IsNullOrEmptyOrZero(this.Rankey())) {
                resolve(false);
                return;
            }
            $.AifShowMsg(`確定刪除「${this.FileTitle()}」?`, {
                icon: 'question',
                showCloseButton: true,
                buttons: [
                    {
                        text: '確定',
                        click: async () => {
                            var rs = await this.DelFileWithOutAlert(_delCallBack);
                            resolve(rs);
                        }
                    },
                    {
                        text: '取消',
                        click: () => { }
                    }
                ]
            });
        });
    }
    //下載附件
    Dowmload() {
        if (!$IsNullOrEmpty(this.Object())) {
            $saveByteArray(this.FileTextName(), this.FileType(), $base64ToArrayBuffer(this.Object()));
        } else if (!$IsNullOrEmpty(this.Url())) {
            if (this.FileType().toUpperCase() == "PDF")
                $.openWindowWithPost(`${SysInfo.ESS.SysUrl}/api/ESS/FileBase/Download`, this.Rankey(), { SysNo: this.SysNo(), Rankey: this.Rankey() }, () => { });
            else
                $.locaWindowWithPost(`${SysInfo.ESS.SysUrl}/api/ESS/FileBase/Download`, { SysNo: this.SysNo(), Rankey: this.Rankey() });
        } else {
            $.AifShowMsg("無文件可下載");
        }
    }
    //轉PDF
    ToPDF() {
        return new Promise(async (resolve, reject) => {
            if ($IsNullOrEmpty(this.Object())) {
                $.AifShowMsg("無文件可轉換");
                resolve(false);
                return;
            }
            $.AifAjax(`/api/PUR/PURCommon/FileConvert`, {
                data: {
                    FileData: ko.toJSON({
                        FileName: this.FileTitle,
                        FileObject: this.Object,
                        FileType: this.FileType
                    }),
                    TargetType: 'pdf',
                },
                success: (rs) => {
                    $PreviewFile(this.FileTextName(), rs.payload.File.FileType, rs.payload.File.FileObject);
                    resolve(rs.payload.File);
                },
                error: (rs) => {
                    resolve(false);
                }
            });
        });
    }
    //取得Byte
    GetByte() {
        return new Promise(async (resolve, reject) => {
            if (!$IsNullOrEmpty(this.Object())) {
                resolve(this.Object());
            } else if (!$IsNullOrEmpty(this.Url())) {
                $.AifAjax(`/api/PUR/PURCommon/GetEssFileByte`, {
                    data: {
                        SysNo: this.SysNo(),
                        Rankey: this.Rankey(),
                    },
                    success: (rs) => {
                        resolve(rs.payload.ByteData);
                    },
                    error: (rs) => {
                        resolve(null);
                    }
                });
            } else {
                resolve(null);
            }
        });
    }
}
class $$dataTables {
    constructor(_url, _tableId) {
        this.url = _url;
        this.table = null;
        this.tableId = _tableId;
        this.tableColumns = [];
        this.tableLanguage = {
            "processing": "處理中...",
            "loadingRecords": "載入中...",
            "paginate": {
                "first": "第一頁",
                "previous": "上一頁",
                "next": "下一頁",
                "last": "最後一頁"
            },
            "emptyTable": "目前沒有資料",
            "datetime": {
                "previous": "上一頁",
                "next": "下一頁",
                "hours": "時",
                "minutes": "分",
                "seconds": "秒",
                "amPm": [
                    "上午",
                    "下午"
                ],
                "unknown": "未知",
                "weekdays": [
                    "週日",
                    "週一",
                    "週二",
                    "週三",
                    "週四",
                    "週五",
                    "週六"
                ],
                "months": [
                    "一月",
                    "二月",
                    "三月",
                    "四月",
                    "五月",
                    "六月",
                    "七月",
                    "八月",
                    "九月",
                    "十月",
                    "十一月",
                    "十二月"
                ]
            },
            "searchBuilder": {
                "add": "新增條件",
                "condition": "條件",
                "button": {
                    "_": "複合查詢 (%d)",
                    "0": "複合查詢"
                },
                "clearAll": "清空",
                "conditions": {
                    "array": {
                        "contains": "含有",
                        "equals": "等於",
                        "empty": "空值",
                        "not": "不等於",
                        "notEmpty": "非空值",
                        "without": "不含"
                    },
                    "date": {
                        "after": "大於",
                        "before": "小於",
                        "between": "在其中",
                        "empty": "為空",
                        "equals": "等於",
                        "not": "不為",
                        "notBetween": "不在其中",
                        "notEmpty": "不為空"
                    },
                    "number": {
                        "between": "在其中",
                        "empty": "為空",
                        "equals": "等於",
                        "gt": "大於",
                        "gte": "大於等於",
                        "lt": "小於",
                        "lte": "小於等於",
                        "not": "不為",
                        "notBetween": "不在其中",
                        "notEmpty": "不為空"
                    },
                    "string": {
                        "contains": "含有",
                        "empty": "為空",
                        "endsWith": "字尾為",
                        "equals": "等於",
                        "not": "不為",
                        "notEmpty": "不為空",
                        "startsWith": "字首為",
                        "notContains": "不含",
                        "notStartsWith": "開頭不是",
                        "notEndsWith": "結尾不是"
                    }
                },
                "data": "欄位",
                "leftTitle": "群組條件",
                "logicAnd": "且",
                "logicOr": "或",
                "rightTitle": "取消群組",
                "title": {
                    "_": "複合查詢 (%d)",
                    "0": "複合查詢"
                },
                "value": "內容",
                "deleteTitle": "刪除篩選條件"
            },
            "editor": {
                "close": "關閉",
                "create": {
                    "button": "新增",
                    "title": "新增資料",
                    "submit": "送出新增"
                },
                "remove": {
                    "button": "刪除",
                    "title": "刪除資料",
                    "submit": "送出刪除",
                    "confirm": {
                        "_": "您確定要刪除您所選取的 %d 筆資料嗎？",
                        "1": "您確定要刪除您所選取的 1 筆資料嗎？"
                    }
                },
                "error": {
                    "system": "系統發生錯誤(更多資訊)"
                },
                "edit": {
                    "button": "修改",
                    "title": "修改資料",
                    "submit": "送出修改"
                },
                "multi": {
                    "title": "多重值",
                    "info": "您所選擇的多筆資料中，此欄位包含了不同的值。若您想要將它們都改為同一個值，可以在此輸入，要不然它們會保留各自原本的值。",
                    "restore": "復原",
                    "noMulti": "此輸入欄需單獨輸入，不容許多筆資料一起修改"
                }
            },
            "autoFill": {
                "cancel": "取消"
            },
            "buttons": {
                "copySuccess": {
                    "_": "複製了 %d 筆資料",
                    "1": "複製了 1 筆資料"
                },
                "copyTitle": "已經複製到剪貼簿",
                "excel": "Excel",
                "pdf": "PDF",
                "print": "列印",
                "copy": "複製",
                "colvis": "欄位顯示",
                "colvisRestore": "重置欄位顯示",
                "csv": "CSV",
                "pageLength": {
                    "-1": "顯示全部",
                    "_": "顯示 %d 筆"
                },
                "createState": "建立狀態",
                "removeAllStates": "移除所有狀態",
                "removeState": "移除",
                "renameState": "重新命名",
                "savedStates": "儲存狀態",
                "stateRestore": "狀態 %d",
                "updateState": "更新",
                "collection": "更多"
            },
            "searchPanes": {
                "collapse": {
                    "_": "搜尋面版 (%d)",
                    "0": "搜尋面版"
                },
                "emptyPanes": "沒搜尋面版",
                "loadMessage": "載入搜尋面版中...",
                "clearMessage": "清空",
                "count": "{total}",
                "countFiltered": "{shown} ({total})",
                "showMessage": "顯示全部",
                "collapseMessage": "摺疊全部",
                "title": "篩選條件 - %d"
            },
            "stateRestore": {
                "emptyError": "名稱不能空白。",
                "creationModal": {
                    "button": "建立",
                    "columns": {
                        "search": "欄位搜尋",
                        "visible": "欄位顯示"
                    },
                    "name": "名稱：",
                    "order": "排序",
                    "paging": "分頁",
                    "scroller": "卷軸位置",
                    "search": "搜尋",
                    "searchBuilder": "複合查詢",
                    "select": "選擇",
                    "title": "建立新狀態",
                    "toggleLabel": "包含："
                },
                "duplicateError": "此狀態名稱已經存在。",
                "emptyStates": "名稱不可空白。",
                "removeConfirm": "確定要移除 %s 嗎？",
                "removeError": "移除狀態失敗。",
                "removeJoiner": "和",
                "removeSubmit": "移除",
                "removeTitle": "移除狀態",
                "renameButton": "重新命名",
                "renameLabel": "%s 的新名稱：",
                "renameTitle": "重新命名狀態"
            },
            "select": {
                "columns": {
                    "_": "選擇了 %d 欄資料",
                    "1": "選擇了 1 欄資料"
                },
                "rows": {
                    "1": "選擇了 1 筆資料",
                    "_": "選擇了 %d 筆資料"
                },
                "cells": {
                    "1": "選擇了 1 格資料",
                    "_": "選擇了 %d 格資料"
                }
            },
            "zeroRecords": "沒有符合的資料",
            "aria": {
                "sortAscending": "：升冪排列",
                "sortDescending": "：降冪排列"
            },
            "info": "顯示第 _START_ 至 _END_ 筆結果，共 _TOTAL_ 筆",
            "infoEmpty": "顯示第 0 至 0 筆結果，共 0 筆",
            "infoThousands": ",",
            "lengthMenu": "顯示 _MENU_ 筆結果",
            "search": "搜尋：",
            "searchPlaceholder": "請輸入關鍵字",
            "thousands": ",",
            "infoFiltered": "(從 _MAX_ 筆結果中篩選)"
        };
        this.tablePageLength = 10;
        this.tableRowOrder = [[0, 'asc']];
        this.tableRowId = 'rankey';
        this.isSelectable = true;
        this.isMultiple = false;
        this.hasTableSearch = true;
        this.hasTableLength = true;
        this.hasTablePageInfo = true;
        this.hasTablePaging = true;
        this.dataParameter = {};
    };
    loadTable() {
        return new Promise((resolve, reject) => {
            this.table = new DataTable('#' + this.tableId, {
                ajax: {
                    type: 'POST',
                    url: this.url,
                    contentType: "application/json",
                    dataType: "json",
                    cache: false,
                    data: (d) => {
                        return JSON.stringify($.extend({}, d, {
                            draw: d.draw,
                            start: d.start,
                            length: d.length,
                            searchValue: d.search.value,
                            sortColumn: d.columns[d.order[0].column].data,
                            sortDirection: d.order[0].dir,
                            dataParameter: this.dataParameter,
                        }));
                    }
                },
                columns: this.tableColumns,
                columnsDefs: [],
                processing: true,
                serverSide: true,
                responsive: true,
                select: this.isSelectable == true,
                pageLength: this.tablePageLength,
                language: this.tableLanguage,
                order: this.tableRowOrder,
                rowGroup: {},
                rowId: this.tableRowId,
                createdRow: (row, data, index) => {
                    this.afterCreatedRow(row, data, index);
                },
                initComplete: (settings, json) => {
                    if (this.isSelectable) {
                        if (this.isMultiple === true) {
                            this.table.on('click', 'tbody tr', (e) => {
                                e.currentTarget.classList.toggle('selected');
                                this.onSelected(this.table.rows('.selected').data().toArray());
                            });
                        } else {
                            this.table.on('click', 'tbody tr', (e) => {
                                var classList = e.currentTarget.classList;
                                if (classList.contains('selected')) {
                                    classList.remove('selected');
                                    this.onDeselected(e);
                                }
                                else {
                                    this.table.rows('.selected').nodes().each((row) => row.classList.remove('selected'));
                                    classList.add('selected');
                                    this.onSelected(this.table.rows('.selected').data().toArray());
                                }
                            });
                        }
                    }
                    if (this.table.header != null) {
                        $(this.table.header()[0]).addClass('table-light');
                    }
                    if (this.hasTableLength == false) {
                        $('#' + this.tableId + '_wrapper')
                            .find('.dt-length')
                            .addClass('d-none');
                    }
                    if (this.hasTableSearch == false) {
                        $('#' + this.tableId + '_wrapper')
                            .find('.dt-search')
                            .addClass('d-none');
                    }
                    if (this.hasTablePageInfo == false) {
                        $('#' + this.tableId + '_wrapper')
                            .find('.dt-info')
                            .addClass('d-none');
                    }
                    if (this.hasTablePaging == false) {
                        $('#' + this.tableId + '_wrapper')
                            .find('.dt-paging')
                            .addClass('d-none');
                    }
                    this.afterInitComplete(settings, json);
                    resolve();
                },
                drawCallback: (settings, json) => {
                    this.afterDrawCallback(settings, json);
                },
                footerCallback: (row, data, start, end, display) => {
                    this.afterFooterCallback(row, data, start, end, display);
                }
            });
        });
    };
    afterCreatedRow(_settings, _json) { /*自行override*/ };
    afterInitComplete(_settings, _json) { /*自行override*/ };
    afterDrawCallback(_settings, _json) { /*自行override*/ };
    afterFooterCallback(_row, _data, _start, _end, _display) { /*自行override*/ };
    onSelected(_event) { /*自行override*/ };
    onDeselected(_event) { /*自行override*/ };
    getSelectedRowDatas() {
        //多筆
        return this.table.rows('.selected').data().toArray();
    };
    getSelectedRowData() {
        //單筆
        return this.table.rows('.selected').data().length == 0 ? null : this.table.rows('.selected').data()[0];
    };
    reloadAll() {
        this.table.ajax.reload();
    };
}