function ShowSecond_Page(Value) {
    try {
        //js在iframe子页面获取父页面元素代码如下:
        var d = window.parent.document.getElementById("Second_Page").getElementsByTagName("div");
        for (var i = 1; i < 21; ++i) {
            d[i].style.display = "none";
        }
        d[Value].style.display = "block";
        var ParentHidden_First = window.parent.document.getElementById("First_Page");
        ParentHidden_First.style.display = "none";
        var ParentHidden_Second = window.parent.document.getElementById("Second_Page");
        ParentHidden_Second.style.display = "block";
        var ChangeNav = window.parent.document.getElementById("nav");
        ChangeNav.style.height = 672 + "px";
    }
    catch (e) {
    }
}
function ShowFirstPage(Value) {
    try {
        if (Value == 1) {
            location.href = "#Movie";
            document.getElementById("First_Page").style.display = "block";
            document.getElementById("Second_Page").style.display = "none";
            var ChangeNav = window.parent.document.getElementById("nav");
            ChangeNav.style.height = 970 + "px";
        }
        else if (Value == 2) {
            location.href = "#Movie_Info";
            document.getElementById("First_Page").style.display = "block";
            document.getElementById("Second_Page").style.display = "none";
            var ChangeNav = window.parent.document.getElementById("nav");
            ChangeNav.style.height = 970 + "px";
        }
    }
    catch (e) {
    }
}
