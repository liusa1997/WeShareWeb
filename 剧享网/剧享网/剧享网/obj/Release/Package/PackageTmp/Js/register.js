function sendFormContent() {
    U_UserName = document.getElementById('U_UserName');
    U_UserPassWord = document.getElementById('U_UserPassWord');
    passWordAgain = document.getElementById('passWordAgain');
    U_UserTelNum = document.getElementById('U_UserTelNum');
    if (U_UserName.value === "") {
        alert("用户名不能为空");
        return false;
    }

    if (U_UserPassWord.value === "") {
        alert("请输入密码");
        return false;
    }
    if (U_UserPassWord.value.length < 6 || U_UserPassWord.value.length > 20) {
        alert('密码为6-20位!');
        return false;
    }
    if (passWordAgain.value === "") {
        alert("请输入确认密码");
        return false;
    }
    if (passWordAgain.value !== U_UserPassWord.value) {
        alert("密码前后不一致，请重新输入");
        return false;
    }
    //正则表达式判断格式（参考QQ聊天里面有对应代码）
    if (U_UserTelNum.value === "") {
        alert("请输入手机号");
        return false;
    }
    if (U_UserTelNum.value < 9999999999 || U_UserTelNum.value > 99999999999) {
        alert('请输入11位电话号码');
        return false;
    }
}