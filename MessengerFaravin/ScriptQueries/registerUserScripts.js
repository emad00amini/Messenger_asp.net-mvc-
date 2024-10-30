
/* raw_js

var pass = document.getElementById('password');
var cpass = document.getElementById('confirmPassword');

function ValidatePassword() {
    if ((pass.value) != (cpass.value)) {
        alert("not equal");
    }

}
*/

var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    var firstName = document.getElementById('firstName');
    $scope.alertt = function () {
        var FirstName = $scope.txtFirstName;
        alert(FirstName);
    }

    $("#phoneNumber").keyup(function () {
        var phoneNumberValue = $(this).val();
        $("#userName").val(phoneNumberValue);
    });

    $("#confirmPassword").change(function () {
        if ($("#password").val() != $("#confirmPassword").val()) {
            $("#sss").text("گذرواژه تطابق ندارد");
        }
        else $("#sss").text("");
    });

    /*doesn't work

    $scope.toggle = function () {
        $scope.switch3 = !$scope.switch3;
    };
    */

    function hideForm() {

        if (regForm.style.display == "none") {
            regForm.style.display = "block";
        } else {
            regForm.style.display = "none";
        }
    }

    $scope.RegisterUsers = function () {
        //     alert($scope.txtBirthDate);
        //    alert(Date.parse($scope.txtBirthDate));

        //alert(new Date($scope.txtBirthDate).toLocaleDateString('en-US', {
        //    day: '2-digit',
        //    month: '2-digit',
        //    year: 'numeric',
        //}));

        //var Fname = $scope.txtFirstName;
        //var Lname = $scope.txtLastName;
        //var Id = $scope.txtPhoneNumber;
        //var joinDate = Date.now();

        // alert(new Date(Date.now()).toLocaleDateString());


        //alert(Fname + ' ' + Lname + ' ' + Id);

        //if (!Fname || !Lname || !Id ) {
        //    //alert("وارد کردن تمامی موارد ضروری است");
        //    $("#sss").text("وارد کردن تمامی موارد ضروری است");
        //}
        //else if (Password != ConfirmPassword) {
        //    alert("رمز عبور و تکرار آن مطابقت ندارند");
        //}
        //else
        var firstName = $scope.txtFirstName;
        var lastName = $scope.txtLastName;
        var phoneNumber = $scope.txtPhoneNumber;
        var password = $scope.txtPassword;
        var confirmPassword = $scope.txtConfirmPassword;
        var birthDate = $scope.txtBirthDate;
        if (!firstName || !lastName || !phoneNumber || !password || !confirmPassword || !birthDate) {
            $("#sss").text("تمام موارد وارد شود");
        }
        else if (password != confirmPassword) {
            $("#sss").text("گذرواژه تطابق ندارد");
        }
        else {
            $("#sss").text("");

            $.ajax({
                type: "POST",
                url: "/RegisterUser/Indexx",
                data: {
                    firstName: $scope.txtFirstName,
                    lastName: $scope.txtLastName,
                    phoneNumber: $scope.txtPhoneNumber,
                    birthDate: $scope.txtBirthDate,
                    password: $scope.txtPassword,
                    //joinDate: new Date(Date.now()).toLocaleDateString(),
                    birthDate: new Date($scope.txtBirthDate).toLocaleDateString('en-US', {
                        day: '2-digit',
                        month: '2-digit',
                        year: 'numeric',
                    })
                },
                datatype: "json",
                success: function (data) {
                    if (data["status"] == 1) {
                        alert("نام کاربری مورد نظر شما تکراری است");
                    }
                    else if (data["status"] == 2) {
                        alert(" اطلاعات با موفقیت ثبت شدددد");
                        $http.get('/RegisterUser/List').then(function (response) {
                            $('#_userList').html(response.data);
                        });
                    }
                },
                error: function () {
                    alert("error");
                }
            });
        }
    }





});