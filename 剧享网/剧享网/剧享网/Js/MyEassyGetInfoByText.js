function MyEassyGetInfoByText(status) {
    //获取祖父节点信息
    var grandfathernode = status.parentElement.parentElement;
    //文字点击的话，则获取当前文字的连接路径
    var GetPath = status.getAttribute("name");
    //获取当前的作者名
    var AuthorName = grandfathernode.children[2].children[1].innerHTML;
    //获取当前的作品名
    var WorkName = grandfathernode.children[0].children[1].innerHTML;
    //获取作品时间 
    var WorkTime = grandfathernode.children[1].children[1].innerHTML;
    console.log(GetPath);
    console.log(AuthorName);
    console.log(W_Id);
    console.log(WorkName);
    console.log(WorkTime);
    window.open("" + GetPath + "?AuthorName=" + AuthorName + "&W_Id=" + 1 + "&WorkName=" + WorkName + "&WorkTime=" + WorkTime+"");
}