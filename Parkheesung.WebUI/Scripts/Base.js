//값 존재 여부 검사
function IsNullOrEmpty(str) {
    if (str == null || String(str).trim() == "") {
        return true;
    } else {
        return false;
    }
};

//페이지 이동
function gotoUrl(url) {
    location.href = url;
};