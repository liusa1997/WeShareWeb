function Login() {
    U_UserName = document.getElementById('U_UserName');
    U_UserPassWord = document.getElementById('U_UserPassWord');
    if (U_UserName.value === "") {
        alert("用户名不能为空");
        return false;
    }
    else if (U_UserPassWord.value === "") {
        alert("请输入密码");
        return false;
    }
    else if (U_UserEmail.value === "") {
        alert("请输入QQ邮箱");
        return false;
    }
}