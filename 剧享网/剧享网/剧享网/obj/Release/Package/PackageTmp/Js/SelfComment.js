function SelfComment() {
    //自己的评论内容
    var Comment = document.getElementById("selfcomment").children[0].value;
    //作品的名字
    var WorkName = document.getElementById("Article").innerHTML;
    //作品的类型
    var W_Id = 0;
    //作品的作者名
    var AuthorName = document.getElementById("Author").innerHTML;
    //获取作品发布时间
    var GetTime = document.getElementById("SendTime").innerHTML;
    var JudgeW_Id = document.getElementsByTagName("title")[0].text;
    if (JudgeW_Id == "MyVideo") {
        W_Id = 2;
    }
    else {
        W_Id = 1;
    }
    if (Comment == "") {
        return alert("评论不能为空");
    }
    console.log(AuthorName);
    console.log(W_Id);
    console.log(WorkName);
    console.log(Comment);
    console.log(GetTime);

    $.ajax({
        url: "/Test/SelfComment/",
        type: "post",
        async: true,
        contentType: "application/json",//必须的
        data: '{"AuthorName":"' + AuthorName + '","W_Id":"' + W_Id + '","WorkName":"' + WorkName + '","Comment":"' + Comment + '","GetTime":"' + GetTime + '"}',
        success: function (data) {
            console.log(data);
            if (data == "Warning") {
                return alert("请先登录");
            }
            if (data == "Close") {
                return alert("该功能暂时关闭");
            }
            window.location.reload();
        },
        error: function () {
            alert("错误！！");
        }
    });
}