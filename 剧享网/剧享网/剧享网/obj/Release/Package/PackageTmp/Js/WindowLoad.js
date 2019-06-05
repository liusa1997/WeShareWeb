//网页加载获取用户进入的时间
var entime;
window.onload = function () {
    var date = new Date();
    var Year = date.getFullYear();
    var Month = ((date.getMonth()) + 1);
    var Day = date.getDate();
    var Hour = date.getHours();
    var Minute = date.getMinutes();
    var Seconds = date.getSeconds();
    entime = Year + "-" + Month + "-" + Day + ' ' + Hour + ':' + Minute + ':' + Seconds;
};
//网页关闭时获取用户的退出时间
var quittime;
window.onbeforeunload = function () {
    var date = new Date();
    var Year = date.getFullYear();
    var Month = ((date.getMonth()) + 1);
    var Day = date.getDate();
    var Hour = date.getHours();
    var Minute = date.getMinutes();
    var Seconds = date.getSeconds();
    quittime = Year + "-" + Month + "-" + Day + ' ' + Hour + ':' + Minute + ':' + Seconds;
    $.ajax({
        url: "/T_User/GetClientIPv4Address/",
        method: "post",
        data: '{"EnTime":"' + entime + '","QuitTime":"' + quittime + '"}',
        contentType: "application/json",
        success: function () {
            console.log("over...");
        },
        error: function () {
            console.log("错误!!");
        }
    });
}