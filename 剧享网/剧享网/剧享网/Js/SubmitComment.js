function SubmitComment(status) {
    var fathernode = status.parentElement;
    var ResponseInfo = fathernode.children[0].value;
    if (ResponseInfo == "") {
        return alert("评论不能为空");
    }
    var grandfathernode = status.parentElement.parentElement;
    var BeCommentedUserName = grandfathernode.children[0].innerHTML;
    var BeCommentedInfo = grandfathernode.children[2].innerHTML;
    var CommentTime = grandfathernode.children[5].innerHTML;

    var AuthorName = document.getElementById("Author").innerHTML;
    var WorkName = document.getElementById("Article").innerHTML;

    var W_Id = 0;
    var JudgeW_Id = document.getElementsByTagName("title")[0].text;
    if (JudgeW_Id == "MyVideo") {
        W_Id = 2;
    }
    else {
        W_Id = 1;
    }
    //获取作品发布时间
    var GetTime = document.getElementById("SendTime").innerHTML;
    $.ajax({
        url: "/Test/SubmitComment/",
        type: "post",
        data: {
            BeCommentedUserName: BeCommentedUserName,
            BeCommentedInfo: BeCommentedInfo,
            Time: CommentTime,
            AuthorName: AuthorName,
            W_Id: W_Id,
            WorkName: WorkName,
            ResponseInfo: ResponseInfo,
            GetTime: GetTime
        },
        success: function (data) {
            if (data == "Warning") {
                return alert("评论前请先登录");
            }
            if (data == "Close") {
                return alert("评论功能暂时关闭");
            }
            //
            var label = document.createElement("label");
            label.className = "subinfo";
            var content = document.createTextNode(data);
            label.appendChild(content);

            console.log(content);
            console.log(grandfathernode.children[9]);
            if (grandfathernode.children[10] == undefined || grandfathernode.children[10] == "" || grandfathernode.children[10] == null) {
                var div = document.createElement("div");
                div.className = "sub";
                //先添加div再去追加label不然会报错的
                grandfathernode.appendChild(div);
                grandfathernode.children[10].appendChild(label);
                grandfathernode.children[4].style.display = "none";
            }
            else {
                grandfathernode.children[10].appendChild(label);
                grandfathernode.children[4].style.display = "none";
            }
            console.log("over..");
        },
        error: function () {
            alert("错误！！");
        }
    });

}