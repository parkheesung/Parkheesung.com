var NowPage = 1; //다음 목록 가져오기를 수행하기 위한 전역 변수

$(document).ready(function () {
    window.fbAsyncInit = function () {
        FB.init({
            appId: '1127075374030570',
            xfbml: true,
            cookie: true,
            version: 'v2.6'
        });
    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
});

//가입처리
function MemberJoin(frm) {
    var reg_email = /^([0-9a-zA-Z_\.-]+)@([0-9a-zA-Z_-]+)(\.[0-9a-zA-Z_-]+){1,2}$/;
    var reg_pwd = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");

    if (IsNullOrEmpty(frm.Email.value)) {
        alert("Email을 입력하세요.");
        $("#Email").focus();
    } else if (reg_email.test(frm.Email.value) == false) {
        alert("올바른 Email을 입력하세요.");
        $("#Email").focus();
    } else if (IsNullOrEmpty(frm.Password.value)) {
        alert("비밀번호를 입력하세요.");
        $("#Password").focus();
    } else if (reg_pwd.test(frm.Password.value) == false) {
        alert("비밀번호는 최소 7글자 이상, 대소문자,숫자,특수문자중 2가지 이상을 혼합하여 사용해 주세요.");
        $("#Password").focus();
    } else if (IsNullOrEmpty(frm.confirmPwd.value)) {
        alert("비밀번호 확인을 입력하세요.");
        $("#confirmPwd").focus();
    } else if (frm.Password.value != frm.confirmPwd.value) {
        alert("두 비밀번호가 일치하지 않습니다 .");
        $("#confirmPwd").focus();
    } else if (IsNullOrEmpty(frm.Name.value)) {
        alert("이름을 입력하세요.");
        $("#Name").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#joinFrm").toJson();

        $.post("/Member/JoinMemberProc", jsonData, function (rst) {
            if (rst.Check) {
                alert("추가되었습니다.");
                document.location.reload();
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }

    return false;
};

//로그인 처리
function MemberLogin(frm) {
    var reg_email = /^([0-9a-zA-Z_\.-]+)@([0-9a-zA-Z_-]+)(\.[0-9a-zA-Z_-]+){1,2}$/;

    if (IsNullOrEmpty(frm.UserMail.value)) {
        alert("Email을 입력하세요.");
        $("#UserMail").focus();
    } else if (reg_email.test(frm.UserMail.value) == false) {
        alert("올바른 Email을 입력하세요.");
        $("#UserMail").focus();
    } else if (IsNullOrEmpty(frm.UserPWD.value)) {
        alert("비밀번호를 입력하세요.");
        $("#UserPWD").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#loginFrm").toJson();

        $.post("/Member/LoginMemberProc", jsonData, function (rst) {
            if (rst.Check) {
                location.href = "/"
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }

    return false;
};

//문장 등록
function registSentence() {
    if (IsNullOrEmpty($("#Content").val())) {
        alert("문장을 입력해 주세요.");
        $("#Content").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#frm").toJson();

        $.post("/Reading/RegistSentenceProc", jsonData, function (rst) {
            if (rst.Check) {
                location.href = "/ReadingView/" + rst.Value;
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }
};

//문장 수정
function updateSentence() {
    if (IsNullOrEmpty($("#Content").val())) {
        alert("문장을 입력해 주세요.");
        $("#Content").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#frm").toJson();

        $.post("/Reading/UpdateSentenceProc", jsonData, function (rst) {
            if (rst.Check) {
                location.href = "/ReadingView/" + rst.Value;
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }
};

//해석 저장
function SaveAnswer() {
    var answer = $("#Answer").val();

    if (IsNullOrEmpty(answer)) {
        alert("내용을 작성해 주세요.");
        $("#Answer").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#frm").toJson();

        $.post("/Reading/RegistInterpret", jsonData, function (rst) {
            if (rst.Check) {
                document.location.reload();
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }
};

//해석 수정
function UpdateAnswer(token) {
    var answer = $("#Answer").val();

    if (IsNullOrEmpty(answer)) {
        alert("내용을 작성해 주세요.");
        $("#Answer").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#frm").toJson();

        $.post("/Reading/UpdateInterpret", jsonData, function (rst) {
            if (rst.Check) {
                location.href = "/ReadingView/" + token;
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }
};

//회원정보 수정
function SaveSetup() {
    var jsonData = $("#frm").toJson();
    var reg_pwd = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");

    if (IsNullOrEmpty(jsonData.Name)) {
        alert("이름을 작성해 주세요.");
        $("#Name").focus();
    } else if (!IsNullOrEmpty(jsonData.NewPass) && IsNullOrEmpty(jsonData.NowPass)) {
        alert("비밀번호를 바꾸시려면 현재 비밀번호를 작성해 주세요.");
        $("#NowPass").focus();
    } else if (IsNullOrEmpty(jsonData.NewPass) && !IsNullOrEmpty(jsonData.NowPass)) {
        alert("새 비밀번호를 작성해 주세요.\r\n비밀번호를 수정하지 않으시려면, 현재 비밀번호를 공란으로 두세요.");
        $("#NewPass").focus();
    } else if (!IsNullOrEmpty(jsonData.NewPass) && !IsNullOrEmpty(jsonData.NowPass) && reg_pwd.test(jsonData.NewPass) == false) {
        alert("새 비밀번호는 숫자,영문자,특수문자중 2가지 이상 혼합하여 7글자 이상 작성하셔야 합니다.");
        $("#NewPass").focus();
    } else if (!IsNullOrEmpty(jsonData.NewPass) && !IsNullOrEmpty(jsonData.NowPass) && IsNullOrEmpty(jsonData.NewPassConfirm)) {
        alert("비밀번호 확인란에 새 비밀번호와 동일한 암호를 입력해 주세요.");
        $("#NewPassConfirm").focus();
    } else if (!IsNullOrEmpty(jsonData.NewPass) && !IsNullOrEmpty(jsonData.NowPass) && jsonData.NewPassConfirm != jsonData.NewPass) {
        alert("새 비밀번호와 비밀번호 확인의 내용이 일치하지 않습니다.");
        $("#NewPassConfirm").focus();
    } else {
        $("body").loading(true);
        var jsonData = $("#frm").toJson();

        $.post("/Reading/SaveSetupProc", jsonData, function (rst) {
            if (rst.Check) {
                alert("저장하였습니다.");
                document.location.reload();
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }
};

//프로필 업로드창 호출
function profileUploader(memberID) {
    $("body").openLayer({
        url: "/Member/ProfileUpload?MemberID=" + memberID,
        frmw: 400,
        frmh: 220,
        IsScroll: false
    });
};

//프로필 이미지 업로드 체크 함수
function checkProfileUpload() {
    var fileURL = $("#FileURL").val();
    var memberID = $("#MemberID").val();

    if (IsNullOrEmpty(fileURL))
    {
        alert("업로드할 이미지를 선택해 주세요.");
        return false;
    }
    else
    {
        return true;
    }
};

//다음 목록 가져오기
function GetRows(mode, opt) {
    NowPage = NowPage + 1;

    switch (mode) {
        case "List":
            $.post("/Reading/AddListwithList", { curPage: NowPage }, function (list) {
                RowsWrite(list);
            });
            break;
        case "Best":
            $.post("/Reading/AddListwithBestList", { curPage: NowPage }, function (list) {
                RowsWrite(list);
            });
            break;
        case "MyList":
            $.post("/Reading/AddListwithMyList", { curPage: NowPage }, function (list) {
                RowsWrite(list);
            });
            break;
        case "Search":
            $.post("/Reading/AddListwithSearchList", { keyword: opt, curPage: NowPage }, function (list) {
                RowsWrite(list);
            });
            break;
    }
};

//가져온 목록을 태그로 작성하기
function RowsWrite(rows) {
    if (rows != null && rows.length > 0 && document.getElementById("appendList") != null) {
        var tags = "";
        for (var i = 0; i < rows.length; i++) {
            tags += "<a href=\"/ReadingView/" + rows[i].UrlToken;
            tags += "\" class=\"list-group-item\">";
            tags += "<span class=\"badge badge-primary\">" + rows[i].ItemCount;
            tags += "</span>" + rows[i].Content;
            tags += "</a>";
        }

        $("#appendList").append(tags);
    } else {
        alert("더이상 없습니다.");
    }
};

//추천하기
function InterpretRecommend(interpretID) {
    $.post("/Reading/AddRecommendProc", { InterpretID: interpretID }, function (rst) {
        if (rst.Check) {
            alert("추천하였습니다.");
            document.location.reload();
        } else {
            alert(rst.Message);
        }
    });
};

//비밀번호 찾기
function GetNewPassword() {
    var email = $("#UserMail").val();
    var reg_email = /^([0-9a-zA-Z_\.-]+)@([0-9a-zA-Z_-]+)(\.[0-9a-zA-Z_-]+){1,2}$/;

    if (IsNullOrEmpty(email)) {
        alert("이메일을 입력해 주세요.");
        $("#UserMail").focus();
    } else if (reg_email.test(email) == false) {
        alert("올바른 이메일을 입력해 주세요.");
        $("#UserMail").focus();
    } else {
        $("body").loading(true);

        $.post("/Member/NewPasswordToEmailProc", { UserMail: email }, function (rst) {
            if (rst.Check) {
                alert("발송하였습니다.");
                location.href = "/";
            } else {
                alert(rst.Message);
                $("body").loading(false);
            }
        });
    }

    return false;
};

//Facebook 관련
function FacebookLogin() {
    $("body").loading(true);
    FB.login(function (response) {
        if (response.status === 'connected') {
            FB.api('/me', { fields: 'name, email' }, function (result) {
                FacebookInfo(result.name, result.email, result.id, function (rst) {
                    if (rst.Check) {
                        $.post("/Member/LoginMemberProc", { UserMail: result.email, UserPWD: rst.Value }, function (rst) {
                            if (rst.Check) {
                                location.href = "/Main"
                            } else {
                                alert(rst.Message);
                                $("body").loading(false);
                            }
                        });
                    } else {
                        alert(rst.Message);
                        $("body").loading(false);
                    }
                });
            });
        } else {
            alert("페이스북에 먼저 로그인 해주세요!");
            $("body").loading(false);
        }
    }, { scope: 'public_profile,email' });
};

function FacebookJoin() {
    $("body").loading(true);
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            FB.api('/me', { fields: 'name, email' }, function (result) {
                FacebookInfo(result.name, result.email, result.id, function (rst) {
                    if (rst.Check) {
                        alert("이미 가입된 계정 정보입니다.\r\n로그인해 주세요.");
                        $("body").loading(false);
                    } else {
                        $.post("/Member/JoinMemberWithFacebookProc", { Email: result.email, Password: result.id, Name: result.name, FacebookID:result.id }, function (rst) {
                            if (rst.Check) {
                                alert("추가되었습니다.");
                                document.location.reload();
                            } else {
                                alert(rst.Message);
                                $("body").loading(false);
                            }
                        });
                    }
                });
            });
        } else {
            alert("페이스북에 먼저 로그인 해주세요!");
            $("body").loading(false);
        }
    });
};

function FacebookInfo(name, email, id, callback) {
    if (IsNullOrEmpty(email)) {
        alert("인증정보가 올바르지 않습니다.  다시 확인해 주세요.\r\n앱요청시 권한 승인을 하지 않았을수도 있습니다.  삭제후 다시 승인해 주세요.");
    } else if (IsNullOrEmpty(id)) {
        alert("인증정보가 올바르지 않습니다.  다시 확인해 주세요.\r\n앱요청시 권한 승인을 하지 않았을수도 있습니다.  삭제후 다시 승인해 주세요.");
    } else if (IsNullOrEmpty(name)) {
        alert("인증정보가 올바르지 않습니다.  다시 확인해 주세요.\r\n앱요청시 권한 승인을 하지 않았을수도 있습니다.  삭제후 다시 승인해 주세요.");
    } else {
        $.post("/Member/FindUserWithFacebook", { email: email, facebookID: id }, function (rst) {
            callback(rst);
        });
    }
};

function AccountConfirm() {
    if ($("#UserPWD").val() == "") {
        alert("비밀번호를 입력하세요.");
        $("#UserPWD").focus();
        return false;
    } else {
        $("body").loading(true);
        $.post("/Account/UserConfirm", { UserPWD: $("#UserPWD").val() }, function (data) {
            try {
                if (data.Check) {
                    location.href = "/Account/List";
                } else {
                    alert(data.Message);
                }
            } catch (e) {
                alert(e.message);
            } finally {
                $("body").loading(false);
            }
        });
        return false;
    }
};

function GroupEdit(GroupID, GroupName) {
    $("#GroupName").val(GroupName);
    $("#GroupID").val(GroupID);
};

function GroupReset() {
    $("#GroupName").val("");
    $("#GroupID").val("");
};

function GroupSave() {
    var groupname = $("#GroupName").val();
    var groupid = $("#GroupID").val();
    if (groupname == "") {
        alert("그룹명을 입력하세요.");
        $("#GroupName").focus();
    } else {
        $("body").loading(true);
        $.post("/Account/GroupSave", { GroupName: groupname, GroupID: groupid }, function (data) {
            try {
                if (data.Check) {
                    document.location.reload();
                } else {
                    alert(data.Message);
                }
            } catch (e) {
                alert(e.message);
            } finally {
                $("body").loading(false);
            }
        });
    }
};

function GroupErase(groupID) {
    if (document.getElementById("gList_" + String(groupID)) != null) {
        if (confirm("정말로 삭제하시겠습니까?")) {
            $("body").loading(true);
            $.post("/Account/GroupRemove", { GroupID: groupID }, function (data) {
                try {
                    if (data.Check) {
                        $("#gList_" + String(groupID)).remove();
                    } else {
                        alert(data.Message);
                    }
                } catch (e) {
                    alert(e.message);
                } finally {
                    $("body").loading(false);
                }
            });
        }
    }
};

function AccountSave(backURL) {
    var params = {
        Title: $("#Title").val(),
        UserID: $("#UserID").val(),
        UserPWD: $("#UserPWD").val(),
        AccessURL: $("#AccessURL").val(),
        Memo: $("#Memo").val(),
        GroupID: $("#GroupID").val()
    };

    if (IsNullOrEmpty(params.Title)) {
        alert("제목을 입력하세요.");
        $("#Title").focus();
    } else if (IsNullOrEmpty(params.UserID)) {
        alert("아이디를 입력하세요.");
        $("#UserID").focus();
    } else if (IsNullOrEmpty(params.UserPWD)) {
        alert("비밀번호를 입력하세요.");
        $("#UserPWD").focus();
    } else {
        $("body").loading(true);
        $.post("/Account/AccountSaveProc", params, function (data) {
            try {
                if (data.Check) {
                    alert("저장하였습니다.");
                    location.href = backURL + data.Value;
                } else {
                    alert(data.Message);
                }
            } catch (e) {
                alert(e.message);
            } finally {
                $("body").loading(false);
            }
        });
    }
};

function AccountUpdate(backURL) {
    var params = {
        AccountID : $("#AccountID").val(),
        Title: $("#Title").val(),
        UserID: $("#UserID").val(),
        UserPWD: $("#UserPWD").val(),
        AccessURL: $("#AccessURL").val(),
        Memo: $("#Memo").val(),
        GroupID: $("#GroupID").val()
    };

    if (IsNullOrEmpty(params.Title)) {
        alert("제목을 입력하세요.");
        $("#Title").focus();
    } else if (IsNullOrEmpty(params.UserID)) {
        alert("아이디를 입력하세요.");
        $("#UserID").focus();
    } else if (IsNullOrEmpty(params.UserPWD)) {
        alert("비밀번호를 입력하세요.");
        $("#UserPWD").focus();
    } else {
        $("body").loading(true);
        $.post("/Account/AccountSaveProc", params, function (data) {
            try {
                if (data.Check) {
                    alert("저장하였습니다.");
                    location.href = backURL;
                } else {
                    alert(data.Message);
                }
            } catch (e) {
                alert(e.message);
            } finally {
                $("body").loading(false);
            }
        });
    }
};

function AccountErase(accountID, backURL) {
    if (confirm("정말로 삭제하시겠습니까?")) {
        $("body").loading(true);
        $.post("/Account/AccountRemove", { AccountID: accountID }, function (data) {
            try {
                if (data.Check) {
                    location.href = backURL;
                } else {
                    alert(data.Message);
                }
            } catch (e) {
                alert(e.message);
            } finally {
                $("body").loading(false);
            }
        });
    }
};