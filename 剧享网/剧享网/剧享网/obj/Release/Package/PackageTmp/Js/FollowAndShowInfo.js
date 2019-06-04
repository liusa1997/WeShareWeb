$("document").ready(function () {
    $("body").on("mouseover", ".UserHeadImg", function () {
        var fathernode = this.parentElement;
        var subdiv = fathernode.children[2];
        subdiv.style.display = "block";
    })
    $("body").on("mouseout", ".UserHeadImg", function () {
        var fathernode = this.parentElement;
        var subdiv = fathernode.children[2];
        subdiv.style.display = "none";
    })
    $("body").on("click", ".FollowOK", function () {
        var fathernode = this.parentElement;
        var SearchUserName = fathernode.children[1].innerHTML;
        var Email = fathernode.children[2].children[2].innerHTML;
        $.ajax({
            url: "/SearchInfo/FollowOKUsers/",
            method: "post",
            data: {
                SearchUserName: SearchUserName,
                Email: Email
            },
            success: function (data) {
                if (data == "Followed") {
                    alert("你已经关注过该用户了");
                }
                else if (data == "Laugh") {
                    alert("太臭美了，不要关注自己哦~");
                }
                else {
                    alert("成功关注");
                }
            },
            error: function (data) {
            }
        })
    })
    $("body").on("click", ".FollowOver", function () {
        var fathernode = this.parentElement;
        var grandfathernode = this.parentElement.parentElement;
        //供我的关注信息取消关注后数量-1
        var Anciester = grandfathernode.parentElement;
        var SearchUserName = fathernode.children[1].innerHTML;
        var Email = fathernode.children[2].children[2].innerHTML;

        var FollowersCount = Anciester.children[1].innerHTML;
        //自定义正则匹配
        var reg = /[0-9]+/;
        //取出当前关注数量
        var Count = parseInt(reg.exec(FollowersCount));
        $.ajax({
            url: "/SearchInfo/FollowOverUsers/",
            method: "post",
            data: {
                SearchUserName: SearchUserName,
                Email: Email
            },
            success: function (data) {
                if (data == "NoneFollow") {
                    alert("你还未关注该用户");
                }
                else {
                    alert("成功取消关注");
                    grandfathernode.removeChild(fathernode);
                    try {
                        Count--;
                        Anciester.children[1].innerHTML = '(' + Count + ')';
                    }
                    finally { }
                }
            },
            error: function (data) {
            }
        })
    })
})