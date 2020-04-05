﻿window.onload = function () {
    render();
};

function render() {
    window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier('recaptha-container');
    recaptchaVerifier.render();
}

function phoneAuth(num) {
    // get the number
    //var number = document.getElementById('Phoneno').value;
    var number = num;
    console.log(number);
    // phone no authentication function of firebase
    //it takes two parameter first one is number,,,second one is recaptcha

    firebase.auth().signInWithPhoneNumber(number, window.recaptchaVerifier).then(function (confirmationResult) {
        //s is in lowercase
        window.confirmationResult = confirmationResult;
        coderesult = confirmationResult;
        console.log(coderesult);
        alert("Message sent");
    }).catch(function (error) {
        alert(error.message);
    });
}

function codeverify() {
    var code = document.getElementById('verificationCode').value;
    coderesult.confirm(code).then(function (result) {
        alert("Successfully registered");
        var user = result.user;
        console.log(user);
    }).catch(function (error) {
        alert(error.message);
    });
}