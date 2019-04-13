function CommentaryAgreeorDisagree(status) {
    //获取当前的agree还是disagree
    var judge = status.getAttribute("name");
    //获取父节点
    var fatherNode = status.parentElement;
    //获取父节点的第一个子节点的内容，被点赞的用户名
    var username = fatherNode.children[0].innerHTML;
    //获取父节点的第二个子节点的内容，被点赞的用户名回复的内容
    var commentary = fatherNode.children[2].innerHTML;
    //获取文章作者
    var C_UserName = document.getElementById("Author").innerHTML;
    //此为文章类型
    var W_Id = 0;
    var JudgeW_Id = document.getElementsByTagName("title")[0].text;
    if (JudgeW_Id == "MyVideo") {
        W_Id = 2;
    }
    else {
        W_Id = 1;
    }
    //获取文章名字
    var workname = document.getElementById("Article").innerHTML;
    //获取作品发布时间
    var GetTime = document.getElementById("SendTime").innerHTML;
    //获取评论时间
    var CommentTime = fatherNode.children[5].innerHTML;
    $.ajax({
        url: "/T_AgreeDisagree/CommentaryAgreeorDisagree/",
        type: "post",
        async: true,
        //contentType: "application/json",//必须的

        data: {
            judge: judge,
            username: username,//这样
            C_UserName: C_UserName,
            W_Id: W_Id,
            workname: workname,
            commentary: commentary,
            GetTime: GetTime,
            CommentTime:CommentTime
        },
        //data: '{"judge":"' + judge + '","username":"' + username + '","C_UserName":"' + C_UserName + '","W_Id":"' + W_Id + '","workname":"' + workname + '","commentary":"' + commentary + '"}',
        success: function (data) {
            if (data == "Warning") {
                return alert("请先登录");
            }
            if (data == "Close") {
                return alert("该功能暂时关闭");
            }
            if (data.split(",")[0] == "agree") {
                fatherNode.children[7].innerHTML = "[" + data.split(",")[1] + "]";
            }
            else {
                fatherNode.children[9].innerHTML = "[" + data.split(",")[1] + "]";
            }
        },
        error: function () {
            alert("错误！！");
        }
    });
}