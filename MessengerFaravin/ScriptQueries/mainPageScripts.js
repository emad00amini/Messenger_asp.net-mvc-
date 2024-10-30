var app = angular.module("MyApp", []);
app.controller("MyCtrl", function ($scope, $compile, $http) {

    $scope.openContact = function (phoneNumber, chatType) {


        //var d = id;
        //alert(d);
        $.ajax({
            type: "POST",
            url: "/MainPage/OpenContact",
            data: {
                phoneNumber : phoneNumber
            },
            //datatype: "json",
            success: function (data) {
                $('#_chatMessages').html(data);
            },
            error: function () {
                alert("error");
            }
        });
    }

    /*
    $scope.openMessage = function (phone) {

        $.ajax({
            type: "POST",
            url: "/MainPage/OpenContact",
            data: {
                contactId: id,
            },
            //datatype: "json",
            success: function (data) {
                $('#_chatMessages').html(data);
            },
            error: function () {
                alert("error");
            }
        });
    }
    */

    $scope.send = function (keyEvent) {

        if (keyEvent.which === 13) {

            $.ajax({
                type: "POST",
                url: "/MainPage/SendMessage",
                data: {
                    text: $("#_chatBox").val(),
                },
                //datatype: "json",
                success: function (data) {

                    $('#_chatMessages').html(data);
                    $("#_chatBox").val("");


                    $http.get('/MainPage/MessageList').then(function (response) {
                        var newContent = angular.element(response.data);
                        var compiledContent = $compile(newContent)($scope);
                        $('#_messagesList').empty().append(compiledContent);
                        // $('#_userContacts').html(response.data);
                    });
                },
                error: function () {
                    alert("error");
                }
            })

        }
    }


    $scope.addContact = function () {

        $.ajax({
            type: "POST",
            url: "/MainPage/addContact",
            data: {
                firstName: $scope.txtFirstName,
                lastName: $scope.txtLastName,
                phoneNumber: $scope.txtPhoneNumber
            },
            datatype: "json",
            success: function (data) {
                if (data["status"] == 1) {
                    alert("کاربر با این شماره یافت نشد");
                }
                else if (data["status"] == 2) {
                    alert(" به مخاطبین افزوده شد");
                    $http.get('/MainPage/UserContactsList').then(function (response) {
                        var newContent = angular.element(response.data);
                        var compiledContent = $compile(newContent)($scope);
                        $('#_userContacts').empty().append(compiledContent);
                       // $('#_userContacts').html(response.data);
                    });

                }
                else if (data["status"] == 3) {
                    alert("تکراری");
                }
            },
            error: function () {
                alert("error");
            }
        });
    }
});