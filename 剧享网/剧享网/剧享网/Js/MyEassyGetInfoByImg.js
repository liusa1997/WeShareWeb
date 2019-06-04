﻿function MyEassyGetInfoByImg(status) {
    //获取祖父节点信息
    var grandfathernode = status.parentElement.parentElement.parentElement;
    //获取父节点信息
    var fathernode = status.parentElement;
    //图片点击的话，则获取当前图片的连接路径
    var GetPath = fathernode.getAttribute("name");
    //获取当前的作者名
    var AuthorName = grandfathernode.children[1].children[2].children[1].innerHTML;
    //获取当前的作品名
    var WorkName = grandfathernode.children[1].children[0].children[1].innerHTML;
    //获取作品时间 
    var WorkTime = grandfathernode.children[1].children[1].children[1].innerHTML;
    console.log(GetPath);
    window.open("" + GetPath + "?AuthorName=" + AuthorName + "&W_Id=" + 1 + "&WorkName=" + WorkName + "&WorkTime=" + WorkTime+"", "_blank");
}