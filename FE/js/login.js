$(document).ready(function () {
    function Login(username, password) {
        $.ajax({
            url: 'https://localhost:7225/User/Login',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                username: username,
                password: password,
            }),
            success: function (response) {
                var code = response.code;
                if (code === 200) {
                    document.getElementById('success').innerHTML = 'Login successful';
                    document.getElementById('error').innerHTML = '';
                    var role = response.data.roleId;
                    var userId = response.data.userId;
                    var username = response.data.username;
                    var token = response.data.access_Token;
                    sessionStorage.setItem('role', role.toString());
                    sessionStorage.setItem('userId', userId.toString());
                    sessionStorage.setItem('username', username);
                    sessionStorage.setItem('token', token);
                    window.location.href = 'home.html';
                } else {
                    document.getElementById('success').innerHTML = '';
                    document.getElementById('error').innerHTML = response.message;
                }
            },

            error: function (error) {
                console.log('Error: ', error);
            }
        })
    }

    document.getElementById('btnLogin').addEventListener('click', function () {
        var username = document.getElementById('username');
        var password = document.getElementById('password');
        const regexUsername = new RegExp("^[a-zA-Z0-9]*$");
        if (!regexUsername.test(username.value.toString()) || username.value.toString().length > 50 || username.value.toString().length < 6) {
            document.getElementById('success').innerHTML = '';
            document.getElementById('error').innerHTML = "The username starts with alphabet , contains only alphabet and numbers, at least 6 characters, max 50 characters";
        } else {
            document.getElementById('error').innerHTML = "";
            Login(username.value, password.value);
        }
    });

})