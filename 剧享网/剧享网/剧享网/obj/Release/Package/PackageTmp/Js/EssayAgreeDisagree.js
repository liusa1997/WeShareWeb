function EssayAgreeDisagree(status) {
    //作品的名字
    var WorkName = document.getElementById("Article").innerHTML;
    //作品的作者名
    var AuthorName = document.getElementById("Author").innerHTML;
    //获取agree还是disagree
    var judge = status.getAttribute("name");
    //获取作品发布时间
    var GetTime = document.getElementById("SendTime").innerHTML;
    //获取父节点信息
    var fatherNode = status.parentElement;
    $.ajax({
        url: "/T_AgreeDisagree/EssayAgreeDisagree/",
        type: "post",
        async: true,
        contentType: "application/json",//必须的
        data: '{"judge":"' + judge + '","username":"' + AuthorName + '","workname":"' + WorkName + '","GetTime":"' + GetTime + '"}',
        success: function (data) {
            console.log("over..");
            console.log(data);
            if (data == "Warning") {
                return alert("请先登录");
            }
            if (data == "Close") {
                return alert("该功能暂时关闭");
            }
            if (data.split(",")[0] == "agree") {
                fatherNode.children[4].innerHTML = "[" + data.split(",")[1] + "]";
            }
            else {
                fatherNode.children[6].innerHTML = "[" + data.split(",")[1] + "]";
            }
        },
        error: function () {
            alert("错误！！");
        }
    });
}