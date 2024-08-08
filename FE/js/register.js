$(document).ready(function () {
    function register(fullName, phone, email, username) {
        $.ajax({
            url: 'https://localhost:7225/User/Create',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                fullName: fullName,
                phone: phone,
                email: email,
                username: username
            }),

            success: function (response) {
                let code = response.code;
                if (code === 200) {
                    document.getElementById('success').innerHTML = response.message;
                    document.getElementById('error').innerHTML = '';
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

    document.getElementById('btnCreate').addEventListener('click', function () {
        const fullNameInput = document.getElementById("FullName");
        const phoneInput = document.getElementById("phone");
        const regexUsername = new RegExp("^[a-zA-Z0-9]*$");
        const regexNumber = new RegExp("^[0-9]");
        const usernameInput = document.getElementById("username");
        const emailInput = document.getElementById("email");
        if (fullNameInput.value.toString().trim().length === 0) {
            document.getElementById("error").innerHTML = "You have to input your name";
            document.getElementById("success").innerHTML = "";
        } else if (phoneInput.value.toString().length !== 0 && phoneInput.value.toString().length !== 10) {
            document.getElementById("error").innerHTML = "Phone must be 10 numbers";
            document.getElementById("success").innerHTML = "";
        } else if (!regexUsername.test(username.value.toString()) || usernameInput.value.toString().length > 50 || usernameInput.value.toString().length < 6 || regexNumber.test(usernameInput.value.toString())) {
            document.getElementById("error").innerHTML = "The username starts with alphabet , contains only alphabet and numbers, at least 6 characters, max 50 characters";
            document.getElementById("success").innerHTML = "";
        } else {
            register(fullNameInput.value, phoneInput.value, emailInput.value, usernameInput.value);
        }
    })
})