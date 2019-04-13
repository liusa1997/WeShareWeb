var n = 0;
var AutoStart;
function GetValue(value) {
    n = value;
    SetBackground(value);
    PlayImg(value);
}
function SetBackground(value) {
    for (var i = 0; i < 20; ++i) {
        document.getElementById("t" + i).className = "sleep";
    }
    document.getElementById("t" + (value - 1)).className = "active";
}
function PlayImg(value) {
    try {
        with (ShowImg) {
            for (var i = 0; i < 20; ++i) {
                if (i == (value - 1)) {
                    children[i].style.display = "block";
                }
                else {
                    children[i].style.display = "none";
                }
            }
        }
    }
    catch (e) {
        var d = document.getElementById("ShowImg").getElementsByTagName("div");
        for (var i = 0; i < 20; ++i) {
            if (i == value) {
                d[i].style.display = "block";
            }
            else {
                d[i].style.display = "none";
            }
        }
    }
}
function Auto() {
    n++;
    if (n > 20) {
        n = 1;
    }
    GetValue(n);
}
function SetAuto() {
    AutoStart = setInterval("Auto(n)", 1000);
}
function ClearAuto() {
    clearInterval(AutoStart);
}
SetAuto();