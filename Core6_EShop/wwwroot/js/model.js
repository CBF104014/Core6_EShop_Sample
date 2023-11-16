
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
    Test() {
        debugger;
    }
    //取得瀏覽資料
    GetFile(_para, _uploadCallBack = () => { }) {
        return new Promise(async (resolve, reject) => {
            if (_para == null || _para.target == null || _para.target.files[0] == null) {
                resolve(false);
                return;
            }
            var fileData = _para.target.files[0];
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