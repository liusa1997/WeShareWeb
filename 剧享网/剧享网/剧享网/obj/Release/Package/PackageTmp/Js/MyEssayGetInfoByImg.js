function MyEssayGetInfoByImg(status) {
    //获取祖父节点信息
    var grandfathernode = status.parentElement.parentElement.parentElement;
    //获取祖先节点（就是我的文章、视频的父节点div）
    var AncisterNode = status.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement;
    //获取类型
    var GetType = AncisterNode.children[0];
    //获取父节点信息
    var fathernode = status.parentElement;
    //图片点击的话，则获取当前图片的连接路径
    var GetPath = fathernode.getAttribute("name");
    //获取当前的作者名
    var AuthorName = grandfathernode.children[1].children[1].innerHTML;
    //获取当前的作品名
    var WorkName = grandfathernode.children[1].children[0].children[0].innerHTML;
    //获取作品时间 
    var WorkTime = grandfathernode.children[1].children[0].children[2].innerHTML;
    console.log(WorkTime);
    var W_Id = 0;
    if (GetType.innerHTML == "我的文章") {
        W_Id = 1;
    }
    else {
        W_Id = 2;
    }
    window.open("" + GetPath + "?AuthorName=" + AuthorName + "&W_Id=" + W_Id + "&WorkName=" + WorkName + "&WorkTime=" + WorkTime+"", "_blank");
}