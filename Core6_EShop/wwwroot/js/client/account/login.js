
document.addEventListener("DOMContentLoaded", () => {
    const { createApp, ref } = Vue;
    const appMain = createApp({
        data() {
            return {
                logoHtml: document.getElementById('logo_Template').innerHTML,
                email: '',
                password: '',
            }
        },
        methods: {
            checkUser: function (item) {
                debugger
                $.ajax({
                    type: "POST",
                    url: "/Account/CheckUser",
                    data: JSON.stringify({
                        Id: this.email,
                        Password: this.password,
                    }),
                    contentType: "application/json",
                    success: function (rs) {
                        console.log(rs);
                        if (rs.success == true)
                            $$showMsg(rs.userData.name, rs.userData.name);
                    },
                    error: function () { }
                });
            }
        }
    });
    appMain.mount('#loginContainer');
});