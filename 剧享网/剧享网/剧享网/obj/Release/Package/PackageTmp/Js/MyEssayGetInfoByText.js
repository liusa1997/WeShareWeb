function MyEssayGetInfoByText(status) {
    //获取祖父节点信息
    var grandfathernode = status.parentElement.parentElement;
    //获取祖先节点（就是我的文章、视频的父节点div）
    var AncisterNode = status.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement;
    //获取类型
    var GetType = AncisterNode.children[0];
    //文字点击的话，则获取当前文字的连接路径
    var GetPath = status.getAttribute("name");
    //获取当前的作者名
    var AuthorName = grandfathernode.children[1].innerHTML;
    //获取当前的作品名
    var WorkName = grandfathernode.children[0].children[0].innerHTML;
    //获取作品时间 
    var WorkTime = grandfathernode.children[0].children[2].innerHTML;
    var W_Id = 0;
    if (GetType.innerHTML == "我的文章") {
        W_Id = 1;
    }
    else {
        W_Id = 2;
    }
    console.log(GetPath);
    console.log(AuthorName);
    console.log(W_Id);
    console.log(WorkName);
    console.log(WorkTime);
    window.open("" + GetPath + "?AuthorName=" + AuthorName + "&W_Id=" + W_Id + "&WorkName=" + WorkName + "&WorkTime=" + WorkTime+"");
}