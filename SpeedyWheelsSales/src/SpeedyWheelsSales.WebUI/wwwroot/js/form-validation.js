document.addEventListener("DOMContentLoaded", function () {
    var forms = document.getElementsByTagName("form");

    for (var i = 0; i < forms.length; i++) {
        forms[i].addEventListener("submit", function (event) {
            var inputs = this.querySelectorAll("input, textarea");
            var hasChanged = Array.from(inputs).some(function (input) {
                return input.value !== input.defaultValue;
            });

            if (!hasChanged) {
                alert("Please make at least one change before submitting the form.");
                event.preventDefault();
            }
        });
    }
});