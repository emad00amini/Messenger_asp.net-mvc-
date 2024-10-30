var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    
    $scope.LoginUser = function () {

        $.ajax({
            type: "POST",
            url: "/LoginUser/Indexx",
            data: {
                userName: $scope.txtUserName,
                password: $scope.txtPassword
            },
            datatype: "json",
            success: function (data) {
                if (data["status"] == 1) {
                    alert("نام کاربری مورد نظر شما یافت نشد");
                }
                else if (data["status"] == 2) {
                    alert("رمز اشتباه است");
                }
                else if (data["status"] == 3) {
                    alert("خوش آمدید");
                   // var id =  @Session["userId"];
                    //alert(id);
                   
                    //$scope.goToMainPage(id);
                    window.location.replace('https://localhost:44361/MainPage/Index/'); 
                    // window.location.href = "/RegisterUser/Index";
                    //window.location.href =  '@Url.Action("Index", "MainPage")';
                }
            },
            error: function () {
                alert("خطا");
            }
        });
    }

    
    
    



    






















});